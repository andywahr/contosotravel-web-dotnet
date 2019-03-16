using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ContosoTravel.Web.Application.Data.Mock;
using ContosoTravel.Web.Application.Interfaces.MVC;
using ContosoTravel.Web.Application.Models;
using System.Web.Http;

namespace ContosoTravel.Web.Host.MVC.Core.Controllers.Api
{
    [RoutePrefix("api/[controller]")]
    public class FlightsController : ApiController
    {
        private readonly IFlightsController _flightsController;

        public FlightsController(IFlightsController flightController)
        {
            _flightsController = flightController;
        }

        [HttpGet]
        public async Task<SearchModel> Index(CancellationToken cancellationToken)
        {
            return await _flightsController.Index(cancellationToken);
        }

        [HttpPost()]
        [Route("search")]
        public async Task<FlightReservationModel> Search(SearchModel searchRequest, CancellationToken cancellationToken)
        {
            return await _flightsController.Search(searchRequest, cancellationToken);
        }

        [HttpPost()]
        [Route("purchase")]
        public async Task Purchase(FlightReservationModel flight, CancellationToken cancellationToken)
        {
            await _flightsController.Purchase(flight, cancellationToken);
        }
    }
}