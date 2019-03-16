using ContosoTravel.Web.Application.Interfaces.MVC;
using ContosoTravel.Web.Application.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace ContosoTravel.Web.Host.MVC.Core.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItineraryController : ControllerBase
    {
        private readonly IItineraryController _itineraryController;

        public ItineraryController(IItineraryController itineraryController)
        {
            _itineraryController = itineraryController;
        }

        [HttpGet]
        public async Task<ItineraryModel> Index(CancellationToken cancellationToken, string recordLocator = "")
        {
            if (string.IsNullOrEmpty(recordLocator))
            {
                return await _itineraryController.GetByCartId(cancellationToken);
            }
            else
            {
                return await _itineraryController.GetByRecordLocator(recordLocator, cancellationToken);
            }
        }
    }
}