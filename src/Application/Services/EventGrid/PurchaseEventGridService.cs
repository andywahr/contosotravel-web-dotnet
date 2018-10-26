﻿using ContosoTravel.Web.Application.Interfaces;
using ContosoTravel.Web.Application.Messages;
using ContosoTravel.Web.Application.Models;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.Services.AppAuthentication;
using Newtonsoft.Json;
using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace ContosoTravel.Web.Application.Services.EventGrid
{
    public class PurchaseEventGridService : IPurchaseService
    {
        private readonly string KeysURL;
        private readonly string TopicHostName;
        private readonly AsyncLazy<EventGridClient> _eventGridClient;

        private class EventGridKeyResponse
        {
            public string key1 { get; set; }
            public string key2 { get; set; }
        }

        public PurchaseEventGridService()
        {
            KeysURL = $"https://management.azure.com/subscriptions/{Configuration.SubscriptionId}/resourceGroups/{Configuration.ResourceGroupName}/providers/Microsoft.EventGrid/topics/{Configuration.ServicesMiddlewareAccountName}/listKeys?api-version=2018-05-01-preview";
            TopicHostName = $"{Configuration.ServicesMiddlewareAccountName}.{Configuration.AzureRegion}-1.eventgrid.azure.net";

            _eventGridClient = new AsyncLazy<EventGridClient>(async () =>
            {
                return new EventGridClient(new TopicCredentials(await GetEventGridKey()));
            });
        }

        private async Task<string> GetEventGridKey()
        {
            var tokenProvider = new AzureServiceTokenProvider();
            string msiKey = await tokenProvider.GetAccessTokenAsync("https://management.azure.com/");

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", msiKey);
                client.DefaultRequestHeaders.Add("x-ms-version", "2018-05-01-preview");

                var httpResponse = await client.PostAsync(KeysURL, new StringContent(string.Empty));

                httpResponse.EnsureSuccessStatusCode();

                string jsonResponse = await httpResponse.Content.ReadAsStringAsync();
                EventGridKeyResponse response = JsonConvert.DeserializeObject<EventGridKeyResponse>(jsonResponse);

                return response.key1;
            }
        }

        public async Task<bool> SendForProcessing(string cartId, CancellationToken cancellationToken)
        {
            PurchaseItineraryMessage purchaseItineraryMessage = new PurchaseItineraryMessage() { CartId = cartId };

            List<EventGridEvent> events = new List<EventGridEvent>();

            events.Add(new EventGridEvent()
            {
                Id = Guid.NewGuid().ToString(),
                EventType = "ContosoTravel.Web.Application.Messages.PurchaseItineraryMessage",
                Data = new PurchaseItineraryMessage() { CartId = cartId },
                EventTime = DateTime.UtcNow,
                Subject = "PurchaseItinerary",
                DataVersion = "1.0"
            });

            var client = await _eventGridClient;
            await client.PublishEventsAsync(TopicHostName, events, cancellationToken);

            return true;
        }
    }
}
