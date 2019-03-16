using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ContosoTravel.Web.Application.Data.Mock;
using ContosoTravel.Web.Application.Interfaces.MVC;
using ContosoTravel.Web.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace ContosoTravel.Web.Host.MVC.Core.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
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

        [HttpPost("search")]
        public async Task<FlightReservationModel> Search(SearchModel searchRequest, CancellationToken cancellationToken)
        {
            return await _flightsController.Search(searchRequest, cancellationToken);
        }

        [HttpPost("purchase")]
        public async Task Purchase(FlightReservationModel flight, CancellationToken cancellationToken)
        {
            await _flightsController.Purchase(flight, cancellationToken);
        }
    }
}