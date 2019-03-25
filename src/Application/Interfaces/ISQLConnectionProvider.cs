using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace ContosoTravel.Web.Application.Interfaces
{
    public interface ISQLConnectionProvider
    {
        Task<IDbConnection> GetOpenConnection(CancellationToken cancellationToken);
    }
}
