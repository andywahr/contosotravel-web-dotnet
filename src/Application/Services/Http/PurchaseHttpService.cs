using ContosoTravel.Web.Application.Interfaces;
using ContosoTravel.Web.Application.Messages;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ContosoTravel.Web.Application.Services.Http
{
    public class PurchaseHttpService : IPurchaseService
    {
        private readonly HttpClient _httpClient;
        private readonly string _pathAndQuery;
        private readonly ContosoConfiguration _contosoConfig;

        public PurchaseHttpService(ContosoConfiguration contosoConfig)
        {
            _contosoConfig = contosoConfig;
            Uri serviceUri = new Uri(_contosoConfig.ServiceFQDN);

            _pathAndQuery = serviceUri.PathAndQuery;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = serviceUri;
        }

        public async Task<bool> SendForProcessing(string cartId, System.DateTimeOffset PurchasedOn, CancellationToken cancellationToken)
        {
            PurchaseItineraryMessage purchaseItineraryMessage = new PurchaseItineraryMessage() { CartId = cartId, PurchasedOn = PurchasedOn };

            var httpResponse = await _httpClient.PostAsync(_pathAndQuery, new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(purchaseItineraryMessage)));

            httpResponse.EnsureSuccessStatusCode();

            return true;
        }
    }
}
