using System;
using System.IO;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace LavanderiaWeb.Data
{
    public abstract class BaseHelper
    {
        private readonly string _connectionString;

        protected BaseHelper(IConfiguration configuration)
        {
            string rawConnection = configuration.GetConnectionString("LavanderiaConnection")
                ?? throw new InvalidOperationException(
                    "No se encontró la cadena de conexión 'LavanderiaConnection' en appsettings.json.");

            string dataDir = AppDomain.CurrentDomain.GetData("DataDirectory")?.ToString()
                ?? Directory.GetCurrentDirectory();
            _connectionString = rawConnection.Replace("|DataDirectory|", dataDir);
        }
        protected async Task<SqlConnection> GetOpenConnectionAsync()
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();   
            return connection;
        }
    }
}