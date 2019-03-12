using ContosoTravel.Web.Application.Messages;
using ContosoTravel.Web.Function.EventGrid;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ContosoTravel.Web.Host.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseItineraryController : ControllerBase
    {
        private ILogger _logger;
        private readonly IHostingEnvironment _hostingEnvironment;

        public PurchaseItineraryController(ILogger<PurchaseItineraryController> logger, IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost()]
        public async Task<IActionResult> Purchase([FromBody]PurchaseItineraryMessage purchseItnierary, CancellationToken cancellationToken)
        {
            Microsoft.Azure.WebJobs.ExecutionContext context = new Microsoft.Azure.WebJobs.ExecutionContext();
            context.FunctionAppDirectory = _hostingEnvironment.ContentRootPath;
            return await PurchaseItineraryEventGrid.Run(purchseItnierary, _logger, cancellationToken, context);
        }
    }
}
