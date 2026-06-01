using System.Data.SqlClient;
using LavanderiaWeb.Models;

namespace LavanderiaWeb.Data
{
    public class SubscripcionDAO : BaseHelper
    {
        public SubscripcionDAO(IConfiguration configuration) : base(configuration) { }

        public async Task<List<Subscripcion>> GetSubscripciones()
        {
            var subscripciones = new List<Subscripcion>();

            using var connection = await GetOpenConnectionAsync();

            const string sql = @"
                SELECT idSubscripcion, nombre, precio, frecuenciaRecogida, limitePrendas
                FROM Subscripcion";

            using var command = new SqlCommand(sql, connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                subscripciones.Add(new Subscripcion
                {
                    idSubscripcion = reader.GetInt32(reader.GetOrdinal("idSubscripcion")),
                    nombre = reader.GetString(reader.GetOrdinal("nombre")),
                    precio = Convert.ToSingle(reader["precio"]),
                    frecuenciaRecogida = reader.GetString(reader.GetOrdinal("frecuenciaRecogida")),
                    limitePrendas = reader.GetInt32(reader.GetOrdinal("limitePrendas"))
                });
            }

            return subscripciones;
        }

        public async Task InsertSubscripcion(Subscripcion subscripcion)
        {
            using var connection = await GetOpenConnectionAsync();

            const string sql = @"
                INSERT INTO Subscripcion (nombre, precio, frecuenciaRecogida, limitePrendas)
                VALUES (@nombre, @precio, @frecuenciaRecogida, @limitePrendas)";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@nombre", subscripcion.nombre);
            command.Parameters.AddWithValue("@precio", subscripcion.precio);
            command.Parameters.AddWithValue("@frecuenciaRecogida", subscripcion.frecuenciaRecogida);
            command.Parameters.AddWithValue("@limitePrendas", subscripcion.limitePrendas);

            await command.ExecuteNonQueryAsync();
        }
    }
}