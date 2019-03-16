using ContosoTravel.Web.Application.Interfaces.MVC;
using ContosoTravel.Web.Application.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace ContosoTravel.Web.Host.MVC.Core.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
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

        [HttpPost("search")]
        public async Task<HotelReservationModel> Search(SearchModel searchRequest, CancellationToken cancellationToken)
        {
            return await _hotelController.Search(searchRequest, cancellationToken);
        }

        [HttpPost("purchase")]
        public async Task Purchase(HotelReservationModel hotel, CancellationToken cancellationToken)
        {
            await _hotelController.Purchase(hotel, cancellationToken);
        }
    }
}