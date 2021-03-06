﻿using ContosoTravel.Web.Application.Interfaces;
using ContosoTravel.Web.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContosoTravel.Web.Application.Data.SQL
{
    public class CarDataSQLProvider : ICarDataProvider, IWritableDataProvider<CarModel>
    {
        private readonly SQLProvider _sqlServerProvider;
        private readonly IAirportDataProvider _airportDataProvider;

        public CarDataSQLProvider(SQLProvider sqlServerProvider, IAirportDataProvider airportDataProvider)
        {
            _sqlServerProvider = sqlServerProvider;
            _airportDataProvider = airportDataProvider;
        }

        public async Task<CarModel> FindCar(int carId, CancellationToken cancellationToken)
        {
            return await ResolveAirport((await _sqlServerProvider.Query<SQLServerFindByIdParam, CarModel>("FindCarById",
                                                                                    new SQLServerFindByIdParam() { IdP = carId },
                                                                                    cancellationToken)).FirstOrDefault(), cancellationToken);
        }

        private class FindCarsParms
        {
            public string LocationP { get; set; }
            public DateTimeOffset DesiredTimeP { get; set; }
        }

        public async Task<IEnumerable<CarModel>> FindCars(string location, DateTimeOffset desiredTime, CancellationToken cancellationToken)
        {
            return await ResolveAirport(await _sqlServerProvider.Query<FindCarsParms, CarModel>("FindCars",
                                                                            new FindCarsParms() { LocationP = location, DesiredTimeP = desiredTime },
                                                                            cancellationToken), cancellationToken);
        }

        private class CreateCarParams
        {
            public int Id { get; set; }
            public string Location { get; set; }
            public DateTimeOffset StartingTime { get; set; }
            public DateTimeOffset EndingTime { get; set; }
            public double Cost { get; set; }
            public int CarType { get; set; }
        }

        public async Task<bool> Persist(CarModel instance, CancellationToken cancellationToken)
        {
            await _sqlServerProvider.Execute<CreateCarParams>("CreateCar", new CreateCarParams()
            {
                Id = instance.Id,
                Location = instance.LocationAirport.AirportCode,
                StartingTime = instance.StartingTime,
                EndingTime = instance.EndingTime,
                Cost = instance.Cost,
                CarType = (int)instance.CarType
            }, cancellationToken);
            return true;
        }

        private async Task<CarModel> ResolveAirport(CarModel carModel, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(carModel?.Location))
            {
                carModel.LocationAirport = await _airportDataProvider.FindByCode(carModel.Location, cancellationToken);
            }

            return carModel;
        }

        private async Task<IEnumerable<CarModel>> ResolveAirport(IEnumerable<CarModel> carModels, CancellationToken cancellationToken)
        {
            if (carModels != null && carModels.Any())
            {
                foreach (var car in carModels)
                {
                    await ResolveAirport(car, cancellationToken);
                }
            }

            return carModels;
        }
    }
}
