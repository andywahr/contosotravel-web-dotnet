using ContosoTravel.Web.Application.Interfaces;
using Dapper;
using MySql.Data.MySqlClient;
using System;
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
            SqlMapper.RemoveTypeMap(typeof(DateTimeOffset));
            SqlMapper.RemoveTypeMap(typeof(DateTimeOffset?));

            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());

            MySql.Data.MySqlClient.MySqlTrace.Switch.Level = System.Diagnostics.SourceLevels.Verbose;

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
