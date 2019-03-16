using ContosoTravel.Web.Application.Interfaces.MVC;
using ContosoTravel.Web.Application.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace ContosoTravel.Web.Host.MVC.Core.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly ICarsController _carsController;

        public CarsController(ICarsController carsController)
        {
            _carsController = carsController;
        }

        [HttpGet]
        public async Task<SearchModel> Index(CancellationToken cancellationToken)
        {
            return await _carsController.Index(cancellationToken);
        }

        [HttpPost("search")]
        public async Task<CarReservationModel> Search([FromBody]SearchModel searchRequest, CancellationToken cancellationToken)
        {
            return await _carsController.Search(searchRequest, cancellationToken);
        }

        [HttpPost("purchase")]
        public async Task Purchase([FromBody]CarReservationModel car, CancellationToken cancellationToken)
        {
            await _carsController.Purchase(car, cancellationToken);
        }
    }
}