using Dapper;
using MySql.Data.MySqlClient;
using System.Data;

namespace Services
{
    public class DataService
    {
        private readonly string _connectionString;

        public DataService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<T>> ExecuteStoredProcedure<T>(string storedProcedure, object parameters = null)
        {
            var projectPath = AppDomain.CurrentDomain.BaseDirectory;
            var certificatePath = Path.Combine(projectPath, "CAcurrency.pem");

            var connStringBuilder = new MySqlConnectionStringBuilder()
            {
                Server = "currency-db-currency-exchange.a.aivencloud.com",
                Port = 10147,
                Database = "defaultdb",
                UserID = "avnadmin",
                Password = "AVNS_Oy5XKrX0jOaUtPb5GdQ",
                SslMode = MySqlSslMode.Required,
                SslCa = certificatePath
            };

            using var connection = new MySqlConnection(connStringBuilder.ConnectionString);
            await connection.OpenAsync();

            var result = await connection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

            return result.AsList();
        }

    }
}
