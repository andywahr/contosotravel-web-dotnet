using ContosoTravel.Web.Application.Interfaces;
using MySql.Data.MySqlClient;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace ContosoTravel.Web.Application.Data.MySQL
{
    public class MySQLProvider : ISQLConnectionProvider
    {
        private readonly ContosoConfiguration _contosoConfig;

        public MySQLProvider(ContosoConfiguration contosoConfig)
        {
            _contosoConfig = contosoConfig;
        }

        public async Task<IDbConnection> GetOpenConnection(CancellationToken cancellationToken)
        {
            string connstring = $"Server={_contosoConfig.DataAccountName}.mysql.database.azure.com; Port=3306; Database={_contosoConfig.DatabaseName}; Uid={_contosoConfig.DataAccountUserName}@{_contosoConfig.DataAccountName}; Pwd={ _contosoConfig.DataAccountPassword}; SslMode=Preferred;";
            MySqlConnection connection = new MySqlConnection(connstring);
            await connection.OpenAsync(cancellationToken);
            return connection;
        }
    }
}
