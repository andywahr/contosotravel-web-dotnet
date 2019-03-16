using ContosoTravel.Web.Application.Interfaces.MVC;
using ContosoTravel.Web.Application.Models;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace ContosoTravel.Web.Host.MVC.Core.Controllers.Api
{
    [RoutePrefix("api/[controller]")]
    public class HotelsController : ApiController
    {
        private readonly IHotelsController _hotelController;

        public HotelsController(IHotelsController hotelController)
        {
            _hotelController = hotelController;
        }

        [HttpGet]
        public async Task<SearchModel> Index(CancellationToken cancellationToken)
        {
            return await _hotelController.Index(cancellationToken);
        }

        [HttpPost()]
        [Route("search")]
        public async Task<HotelReservationModel> Search(SearchModel searchRequest, CancellationToken cancellationToken)
        {
            return await _hotelController.Search(searchRequest, cancellationToken);
        }

        [HttpPost()]
        [Route("purchase")]
        public async Task Purchase(HotelReservationModel hotel, CancellationToken cancellationToken)
        {
            await _hotelController.Purchase(hotel, cancellationToken);
        }
    }
}