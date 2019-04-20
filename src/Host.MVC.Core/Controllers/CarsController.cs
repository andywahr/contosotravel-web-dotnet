using ContosoTravel.Web.Application.Interfaces.MVC;
using ContosoTravel.Web.Application.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace ContosoTravel.Web.Host.MVC.Core.Controllers
{
    [Route("[controller]")]
    [ResponseCache(CacheProfileName="NoCache")]
    public class CarsController : Controller
    {
        private readonly ICarsController _carsController;

        public CarsController(ICarsController carsController)
        {
            _carsController = carsController;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View("Search", await _carsController.Index(cancellationToken));
        }

        [HttpPost]
        [Route("search")]
        public async Task<IActionResult> Search(SearchModel searchRequest, CancellationToken cancellationToken)
        {
            return View("CarResults", await _carsController.Search(searchRequest, cancellationToken));
        }

        [HttpPost]
        [Route("purchase")]
        public async Task<IActionResult> Purchase(CarReservationModel car, CancellationToken cancellationToken)
        {
            await _carsController.Purchase(car, cancellationToken);
            return Redirect("/Cart");
        }
    }
}