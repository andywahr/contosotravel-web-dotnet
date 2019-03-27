using ContosoTravel.Web.Application.Interfaces;
using ContosoTravel.Web.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace ContosoTravel.Web.Application.Data.SQL
{
    public class ItineraryDataSQLProvider : IItineraryDataProvider
    {
        private readonly SQLProvider _sqlServerProvider;

        public ItineraryDataSQLProvider(SQLProvider sqlServerProvider)
        {
            _sqlServerProvider = sqlServerProvider;
        }

        private class ItineraryIdParams
        {
            public Guid IdP { get; set; }
        }

        public async Task<ItineraryPersistenceModel> FindItinerary(string cartId, CancellationToken cancellationToken)
        {
            return (await _sqlServerProvider.Query<ItineraryIdParams, ItineraryPersistenceModel>("GetItineraryById", new ItineraryIdParams()
            {
                IdP = Guid.Parse(cartId)
            }, cancellationToken)).FirstOrDefault();
        }

        private class ItineraryByRecordLocatorParams
        {
            public string RecordLocatorP { get; set; }
        }

        public async Task<ItineraryPersistenceModel> GetItinerary(string recordLocator, CancellationToken cancellationToken)
        {
            return (await _sqlServerProvider.Query<ItineraryByRecordLocatorParams, ItineraryPersistenceModel>("GetItineraryByRecordLocatorId", new ItineraryByRecordLocatorParams()
            {
                RecordLocatorP = recordLocator
            }, cancellationToken)).FirstOrDefault();
        }

        private class ItineraryUpsertParams
        {
            public Guid IdP { get; set; }
            public int? DepartingFlightP { get; set; }
            public int? ReturningFlightP { get; set; }
            public int? CarReservationP { get; set; }
            public double? CarReservationDurationP { get; set; }
            public int? HotelReservationP { get; set; }
            public int? HotelReservationDurationP { get; set; }
            public string RecordLocatorP { get; set; }
            public DateTimeOffset PurchasedOnP { get; set; }
        }

        public async Task UpsertItinerary(ItineraryPersistenceModel itinerary, CancellationToken cancellationToken)
        {
            await _sqlServerProvider.Execute<ItineraryUpsertParams>("UpsertItinerary", new ItineraryUpsertParams()
            {
                IdP = Guid.Parse(itinerary.Id),
                DepartingFlightP = _sqlServerProvider.NullIfZero(itinerary.DepartingFlight),
                ReturningFlightP = _sqlServerProvider.NullIfZero(itinerary.ReturningFlight),
                CarReservationP = _sqlServerProvider.NullIfZero(itinerary.CarReservation),
                CarReservationDurationP = _sqlServerProvider.NullIfZero(itinerary.CarReservationDuration),
                HotelReservationP = _sqlServerProvider.NullIfZero(itinerary.HotelReservation),
                HotelReservationDurationP = _sqlServerProvider.NullIfZero(itinerary.HotelReservationDuration),
                RecordLocatorP = itinerary.RecordLocator,
                PurchasedOnP = itinerary.PurchasedOn
            }, cancellationToken);
        }
    }
}
