using ContosoTravel.Web.Application;
using ContosoTravel.Web.Application.Interfaces;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DataLoader.SQL
{
    public class SQLServerDeployment : IDataDeployment
    {
        private readonly ISQLConnectionProvider _connectionProvider;
        private readonly ContosoConfiguration _contosoConfiguration;

        public SQLServerDeployment(ISQLConnectionProvider connectionProvider, ContosoConfiguration contosoConfiguration)
        {
            _connectionProvider = connectionProvider;
            _contosoConfiguration = contosoConfiguration;
        }

        public async Task Configure(CancellationToken cancellationToken)
        {
            using (SqlConnection connection = new SqlConnection($"Server={_contosoConfiguration.DataAccountName}.database.windows.net;Database=Master;User Id={_contosoConfiguration.DataAdministratorLogin};Password={_contosoConfiguration.DataAdministratorLoginPassword};"))
            {
                await connection.OpenAsync(cancellationToken);
                Server server = new Server(new ServerConnection(connection));
                server.ConnectionContext.ExecuteNonQuery($"IF EXISTS (SELECT * FROM sys.sql_logins WHERE name = N'{_contosoConfiguration.DataAccountUserName}')\nBEGIN\nCREATE LOGIN {_contosoConfiguration.DataAccountUserName} WITH PASSWORD = '{_contosoConfiguration.DataAccountPassword}';\nEND\nGO");
            }

            using (SqlConnection connection = new SqlConnection($"Server={_contosoConfiguration.DataAccountName}.database.windows.net;Database={_contosoConfiguration.DatabaseName};User Id={_contosoConfiguration.DataAdministratorLogin};Password={_contosoConfiguration.DataAdministratorLoginPassword};"))
            {
                await connection.OpenAsync(cancellationToken);
                Server server = new Server(new ServerConnection(connection));

                server.ConnectionContext.ExecuteNonQuery($"IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'{_contosoConfiguration.DataAccountUserName}')\nBEGIN\nCREATE USER {_contosoConfiguration.DataAccountUserName} FROM LOGIN {_contosoConfiguration.DataAccountUserName};\nGRANT EXECUTE TO {_contosoConfiguration.DataAccountUserName};\nEND\nGO");
                server.ConnectionContext.ExecuteNonQuery(ReadResource("Airports.sql"));
                server.ConnectionContext.ExecuteNonQuery(ReadResource("Cars.sql"));
                server.ConnectionContext.ExecuteNonQuery(ReadResource("Flights.sql"));
                server.ConnectionContext.ExecuteNonQuery(ReadResource("Hotels.sql"));
                server.ConnectionContext.ExecuteNonQuery(ReadResource("Carts.sql"));
                server.ConnectionContext.ExecuteNonQuery(ReadResource("Itineraries.sql"));
            }
        }

        private string ReadResource(string name)
        {
            var assembly = typeof(SQLServerDeployment).Assembly;
            using (StreamReader resource = new StreamReader(assembly.GetManifestResourceStream($"DataLoader.SQL.SQLServer.{name}")))
            {
                return resource.ReadToEnd();
            }
        }
    }    
}
