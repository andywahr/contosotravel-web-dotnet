﻿using ContosoTravel.Web.Application.Interfaces.MVC;
using ContosoTravel.Web.Application.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace ContosoTravel.Web.Host.MVC.Core.Controllers
{
    [Route("[controller]")]
    [ResponseCache(CacheProfileName = "NoCache")]
    public class HotelsController : Controller
    {
        private readonly IHotelsController _hotelController;

        public HotelsController(IHotelsController hotelController)
        {
            _hotelController = hotelController;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View("Search", await _hotelController.Index(cancellationToken));
        }

        [HttpPost]
        [Route("search")]
        public async Task<IActionResult> Search(SearchModel searchRequest, CancellationToken cancellationToken)
        {
            return View("HotelResults", await _hotelController.Search(searchRequest, cancellationToken));
        }

        [HttpPost]
        [Route("purchase")]
        public async Task<IActionResult> Purchase(HotelReservationModel hotel, CancellationToken cancellationToken)
        {
            await _hotelController.Purchase(hotel, cancellationToken);
            return Redirect("/Cart");
        }
    }
}