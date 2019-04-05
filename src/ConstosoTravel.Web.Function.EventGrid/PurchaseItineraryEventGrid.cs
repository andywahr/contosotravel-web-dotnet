using Autofac;
using ContosoTravel.Web.Application;
using ContosoTravel.Web.Application.Messages;
using ContosoTravel.Web.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ExecutionContext = Microsoft.Azure.WebJobs.ExecutionContext;

namespace ContosoTravel.Web.Function.EventGrid
{
    public static class PurchaseItineraryEventGrid
    {
        public static IConfiguration _configuration;
        public static Assembly _thisAssembly;

        static PurchaseItineraryEventGrid()
        {
            _configuration = new ConfigurationBuilder().AddJsonFile("appSettings.json", true).AddEnvironmentVariables().Build();
            _thisAssembly = typeof(PurchaseItineraryEventGrid).Assembly;
        }

        // Do the deserialization manually, allow for better DateTimeOffset support

        [FunctionName("PurchaseItinerary")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequest req, ILogger log, CancellationToken cancellationToken, ExecutionContext context)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            PurchaseItineraryMessage pimMsg = Newtonsoft.Json.JsonConvert.DeserializeObject<PurchaseItineraryMessage>(requestBody);
            return await RunWithMessage(pimMsg, log, cancellationToken, context);
        }

        public static async Task<IActionResult> RunWithMessage(PurchaseItineraryMessage pimMsg, ILogger log, CancellationToken cancellationToken, ExecutionContext context)
        {
            log.LogInformation($"Starting to finalize purchase of {pimMsg.CartId}");

            log.LogDebug("Resolving the FulfillmentServices");
            var container = Setup.InitCotosoWithOneTimeLock(_configuration["KeyVaultUrl"], context.FunctionAppDirectory, _thisAssembly);
            var fulfillmentService = container.Resolve<FulfillmentService>();

            log.LogDebug("Calling Purchase method");
            string recordLocator = await fulfillmentService.Purchase(pimMsg.CartId, pimMsg.PurchasedOn, cancellationToken);

            if (string.IsNullOrEmpty(recordLocator))
            {
                log.LogError($"Finalization purchase of {pimMsg.CartId} failed");
                return new BadRequestResult();
            }
            else
            {
                log.LogInformation($"Record Locator {recordLocator} complete for cart {pimMsg.CartId}");
                return new OkResult();
            }
        }
    }
}
