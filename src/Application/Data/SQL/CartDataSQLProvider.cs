using ContosoTravel.Web.Application.Interfaces;
using ContosoTravel.Web.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace ContosoTravel.Web.Application.Data.SQL
{
    public class CartDataSQLProvider : ICartDataProvider
    {
        private readonly SQLProvider _sqlServerProvider;

        public CartDataSQLProvider(SQLProvider sqlServerProvider)
        {
            _sqlServerProvider = sqlServerProvider;
        }

        private class CartIdParams
        {
            public Guid IdP { get; set; }
        }

        public async Task DeleteCart(string cartId, CancellationToken cancellationToken)
        {
            await _sqlServerProvider.Execute<CartIdParams>("DeleteCart", new CartIdParams()
                                                                         {
                                                                             IdP = Guid.Parse(cartId)
                                                                         }, cancellationToken);
        }

        public async Task<CartPersistenceModel> GetCart(string cartId, CancellationToken cancellationToken)
        {
            return (await _sqlServerProvider.Query<CartIdParams, CartPersistenceModel>("GetCartById", new CartIdParams()
                                                                                                    {
                                                                                                        IdP = Guid.Parse(cartId)
                                                                                                    }, cancellationToken)).FirstOrDefault();
        }

        private class UpdateCartCarParms : CartIdParams
        {
            public int? CarReservationP { get; set; }
            public double? CarReservationDurationP { get; set; }
        }

        public async Task<CartPersistenceModel> UpsertCartCar(string cartId, int carId, double numberOfDays, CancellationToken cancellationToken)
        {
            await _sqlServerProvider.Execute<UpdateCartCarParms>("UpsertCartCar", new UpdateCartCarParms()
            {
                IdP = Guid.Parse(cartId),
                CarReservationP = _sqlServerProvider.NullIfZero(carId),
                CarReservationDurationP = _sqlServerProvider.NullIfZero(numberOfDays)
            }, cancellationToken);

            return await GetCart(cartId, cancellationToken);
        }

        private class UpsertCartFlightsParms : CartIdParams
        {
            public int? DepartingFlightP { get; set; }
            public int? ReturningFlightP { get; set; }
        }

        public async Task<CartPersistenceModel> UpsertCartFlights(string cartId, int departingFlightId, int returningFlightId, CancellationToken cancellationToken)
        {
            await _sqlServerProvider.Execute<UpsertCartFlightsParms>("UpsertCartFlights", new UpsertCartFlightsParms()
            {
                IdP = Guid.Parse(cartId),
                DepartingFlightP = _sqlServerProvider.NullIfZero(departingFlightId),
                ReturningFlightP = _sqlServerProvider.NullIfZero(returningFlightId),
            }, cancellationToken);

            return await GetCart(cartId, cancellationToken);
        }

        private class UpdateCartHotelParms : CartIdParams
        {
            public int? HotelReservationP { get; set; }
            public int? HotelReservationDurationP { get; set; }
        }

        public async Task<CartPersistenceModel> UpsertCartHotel(string cartId, int hotelId, int numberOfDays, CancellationToken cancellationToken)
        {
            await _sqlServerProvider.Execute<UpdateCartHotelParms>("UpsertCartHotel", new UpdateCartHotelParms()
            {
                IdP = Guid.Parse(cartId),
                HotelReservationP = _sqlServerProvider.NullIfZero(hotelId),
                HotelReservationDurationP = _sqlServerProvider.NullIfZero(numberOfDays)
            }, cancellationToken);

            return await GetCart(cartId, cancellationToken);
        }
    }
}
