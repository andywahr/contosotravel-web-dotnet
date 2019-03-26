using ContosoTravel.Web.Application.Interfaces;
using ContosoTravel.Web.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace ContosoTravel.Web.Application.Data.SQL
{
    public class FlightDataSQLProvider : IFlightDataProvider, IWritableDataProvider<FlightModel>
    {
        private readonly SQLProvider _sqlServerProvider;
        private readonly IAirportDataProvider _airportDataProvider;

        public FlightDataSQLProvider(SQLProvider sqlServerProvider, IAirportDataProvider airportDataProvider)
        {
            _sqlServerProvider = sqlServerProvider;
            _airportDataProvider = airportDataProvider;
        }

        public async Task<FlightModel> FindFlight(int flightId, CancellationToken cancellationToken)
        {
            return ResolveAirport((await _sqlServerProvider.Query<SQLServerFindByIdParam, FlightModel>("FindFlightById",
                                                                                        new SQLServerFindByIdParam() { Id = flightId },
                                                                                        cancellationToken)).FirstOrDefault(), await _airportDataProvider.GetAll(cancellationToken));
        }

        private class FindFlightsParams
        {
            public string DepartingFrom { get; set; }
            public string ArrivingAt { get; set; }
            public DateTimeOffset DesiredTime { get; set; }
            public int SecondsOffset { get; set; }
        }

        public async Task<IEnumerable<FlightModel>> FindFlights(string departingFrom, string arrivingAt, DateTimeOffset desiredTime, TimeSpan offset, CancellationToken cancellationToken)
        {
            return await ResolveAirport(await _sqlServerProvider.Query<FindFlightsParams, FlightModel>("FindFlights",
                                                                                   new FindFlightsParams()
                                                                                   {
                                                                                       DepartingFrom = departingFrom,
                                                                                       ArrivingAt = arrivingAt,
                                                                                       DesiredTime = desiredTime,
                                                                                       SecondsOffset = (int)offset.TotalSeconds
                                                                                   },
                                                                                   cancellationToken), cancellationToken);
        }


        private class CreateFlightParams
        {
            public int Id { get; set; }
            public string DepartingFrom { get; set; }
            public string ArrivingAt { get; set; }
            public DateTimeOffset DepartureTime { get; set; }
            public DateTimeOffset ArrivalTime { get; set; }
            public TimeSpan Duration { get; set; }
            public double Cost { get; set; }
        }

        public async Task<bool> Persist(FlightModel instance, CancellationToken cancellationToken)
        {
            await _sqlServerProvider.Execute<CreateFlightParams>("CreateFlight", new CreateFlightParams()
            {
                Id = instance.Id,
                DepartingFrom = instance.DepartingFromAiport.AirportCode,
                ArrivingAt = instance.ArrivingAtAiport.AirportCode,
                DepartureTime = instance.DepartureTime,
                ArrivalTime = instance.ArrivalTime,
                Cost = instance.Cost,
                Duration = instance.Duration
            }, cancellationToken);
            return true;
        }

        private FlightModel ResolveAirport(FlightModel flightModel, IEnumerable<AirportModel> allAirports)
        {
            if (!string.IsNullOrEmpty(flightModel?.DepartingFrom))
            {
                flightModel.DepartingFromAiport = allAirports.SingleOrDefault(air => string.Equals(air.AirportCode, flightModel.DepartingFrom, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(flightModel?.ArrivingAt))
            {
                flightModel.ArrivingAtAiport = allAirports.SingleOrDefault(air => string.Equals(air.AirportCode, flightModel.ArrivingAt, StringComparison.OrdinalIgnoreCase));
            }
            return flightModel;
        }

        private async Task<IEnumerable<FlightModel>> ResolveAirport(IEnumerable<FlightModel> flightModels, CancellationToken cancellationToken)
        {
            if (flightModels != null && flightModels.Any())
            {
                var allAirports = await _airportDataProvider.GetAll(cancellationToken);

                foreach (var flight in flightModels)
                {
                    ResolveAirport(flight, allAirports);
                }
            }

            return flightModels;
        }
    }
}
