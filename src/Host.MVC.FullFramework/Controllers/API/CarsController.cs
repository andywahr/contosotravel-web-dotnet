using ContosoTravel.Web.Application.Interfaces.MVC;
using ContosoTravel.Web.Application.Models;
using System.Web.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ContosoTravel.Web.Host.MVC.Core.Controllers.Api
{
    [RoutePrefix("api/[controller]")]
    public class CarsController : ApiController
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

        [HttpPost()]
        [Route("search")]
        public async Task<CarReservationModel> Search([FromBody]SearchModel searchRequest, CancellationToken cancellationToken)
        {
            return await _carsController.Search(searchRequest, cancellationToken);
        }

        [HttpPost()]
        [Route("purchase")]
        public async Task Purchase([FromBody]CarReservationModel car, CancellationToken cancellationToken)
        {
            await _carsController.Purchase(car, cancellationToken);
        }
    }
}