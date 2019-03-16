using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ContosoTravel.Web.Application.Interfaces;
using ContosoTravel.Web.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace ContosoTravel.Web.Host.MVC.Core.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
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