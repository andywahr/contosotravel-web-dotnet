using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ContosoTravel.Web.Application.Interfaces;
using ContosoTravel.Web.Application.Models;
using System.Web.Http;


namespace ContosoTravel.Web.Host.MVC.Core.Controllers.Api
{
    [RoutePrefix("api/[controller]")]
    public class TestController : ApiController
    {
        private readonly IAirportDataProvider _airportDataProvider;

        public TestController(IAirportDataProvider airportDataProvider)
        {
            _airportDataProvider = airportDataProvider;
        }

        [HttpGet]
        public async Task<TestSettings> Index(CancellationToken cancellationToken)
        {
            return await Task.FromResult(ContosoTravel.Web.Application.Models.TestSettings.GetNewTest(await _airportDataProvider.GetAll(cancellationToken)));
        }
    }
}