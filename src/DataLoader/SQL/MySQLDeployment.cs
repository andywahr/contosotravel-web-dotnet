using ContosoTravel.Web.Application;
using ContosoTravel.Web.Application.Interfaces;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using MySql.Data.MySqlClient;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DataLoader.SQL
{
    public class MySQLDeployment : IDataDeployment
    {
        private readonly ISQLConnectionProvider _connectionProvider;
        private readonly ContosoConfiguration _contosoConfiguration;

        public MySQLDeployment(ISQLConnectionProvider connectionProvider, ContosoConfiguration contosoConfiguration)
        {
            _connectionProvider = connectionProvider;
            _contosoConfiguration = contosoConfiguration;
        }

        public async Task Configure(CancellationToken cancellationToken)
        {
            string connstring = $"Server={_contosoConfiguration.DataAccountName}.mysql.database.azure.com; Port=3306; Database={_contosoConfiguration.DatabaseName}; Uid={_contosoConfiguration.DataAdministratorLogin}@{_contosoConfiguration.DataAccountName}; Pwd={ _contosoConfiguration.DataAdministratorLoginPassword}; SslMode=Preferred;";
            
            using (MySqlConnection connection = new MySqlConnection(connstring))
            {
                await connection.OpenAsync(cancellationToken);

                await Run(connection, cancellationToken, $"CREATE USER '{_contosoConfiguration.DataAccountUserName}'@'%' IDENTIFIED BY '{_contosoConfiguration.DataAccountPassword}';\nGRANT EXECUTE ON *.* TO '{_contosoConfiguration.DataAccountUserName}'@'%' WITH GRANT OPTION;");
                await Run(connection, cancellationToken, ReadResource("Airports.sql"));
                await Run(connection, cancellationToken, ReadResource("Cars.sql"));
                await Run(connection, cancellationToken, ReadResource("Flights.sql"));
                await Run(connection, cancellationToken, ReadResource("Hotels.sql"));
                await Run(connection, cancellationToken, ReadResource("Carts.sql"));
                await Run(connection, cancellationToken, ReadResource("Itineraries.sql"));
            }
        }

        private async Task Run(MySqlConnection connection, CancellationToken cancellationToken, string script)
        {
            MySqlScript mySqlScript = new MySqlScript(connection, script);
            await mySqlScript.ExecuteAsync(cancellationToken);
        }

        private string ReadResource(string name)
        {
            var assembly = typeof(SQLServerDeployment).Assembly;
            using (StreamReader resource = new StreamReader(assembly.GetManifestResourceStream($"DataLoader.SQL.MySQL.{name}")))
            {
                return resource.ReadToEnd();
            }
        }
    }    
}
