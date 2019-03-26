using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ContosoTravel.Web.Application.Data.Mock;
using ContosoTravel.Web.Application.Interfaces.MVC;
using ContosoTravel.Web.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace ContosoTravel.Web.Host.MVC.Core.Controllers
{
    [Route("[controller]")]
    public class FlightsController : Controller
    {
        private readonly IFlightsController _flightsController;

        public FlightsController(IFlightsController flightController)
        {
            _flightsController = flightController;
        }

        [HttpGet]
        [Route("/")]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View("Search", await _flightsController.Index(cancellationToken));
        }

        [HttpPost]
        [Route("search")]
        public async Task<IActionResult> Search(SearchModel searchRequest, CancellationToken cancellationToken)
        {
            return View("FlightResults", await _flightsController.Search(searchRequest, cancellationToken));
        }

        [HttpPost]
        [Route("purchase")]
        public async Task<IActionResult> Purchase(FlightReservationModel flight, CancellationToken cancellationToken)
        {
            await _flightsController.Purchase(flight, cancellationToken);
            return Redirect("/Cart");
        }
    }
}