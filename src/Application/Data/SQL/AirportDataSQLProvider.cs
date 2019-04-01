using ContosoTravel.Web.Application.Interfaces;
using ContosoTravel.Web.Application.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContosoTravel.Web.Application.Data.SQL
{
    public class AirportDataSQLProvider : IAirportDataProvider, IWritableDataProvider<AirportModel>
    {
        private readonly SQLProvider _sqlServerProvider;

        public AirportDataSQLProvider(SQLProvider sqlServerProvider)
        {
            _sqlServerProvider = sqlServerProvider;
        }

        private class FindAiportByCodeParams
        {
            public string AirportCodeP { get; set; }
        }

        public async Task<AirportModel> FindByCode(string airportCode, CancellationToken cancellationToken)
        {
            return (await _sqlServerProvider.Query<FindAiportByCodeParams, AirportModel>("FindAirportByCode", 
                                                                                    new FindAiportByCodeParams () { AirportCodeP = airportCode }, 
                                                                                    cancellationToken)).FirstOrDefault();
        }

        public async Task<IEnumerable<AirportModel>> GetAll(CancellationToken cancellationToken)
        {
            return (await _sqlServerProvider.Query<SQLServerEmptyParams, AirportModel>("GetAllAirports",
                                                                                        new SQLServerEmptyParams(),
                                                                                        cancellationToken));
        }

        public async Task<bool> Persist(AirportModel instance, CancellationToken cancellationToken)
        {
            await _sqlServerProvider.Execute<BasicAirportModel>("CreateAirport", new BasicAirportModel(instance), cancellationToken);
            return true;
        }
    }
}
