using System.Data.SqlClient;
using LavanderiaWeb.Models;

namespace LavanderiaWeb.Data
{
    public class ClienteDAO : BaseHelper
    {
        public ClienteDAO(IConfiguration configuration) : base(configuration) { }

        public async Task<List<Cliente>> GetClientes()
        {
            var clientes = new List<Cliente>();

            using var connection = await GetOpenConnectionAsync();

            // LEFT JOIN para incluir clientes sin suscripción asignada (tipoSub = null)
            const string sql = @"
                SELECT c.idCliente, c.nombre, c.telefono,
                       s.idSubscripcion, s.nombre AS nombreSub, s.precio,
                       s.frecuenciaRecogida, s.limitePrendas
                FROM Cliente c
                LEFT JOIN Subscripcion s ON c.idSubscripcion = s.idSubscripcion";

            using var command = new SqlCommand(sql, connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                // Si idSubscripcion es NULL, tipoSub queda null (igual que el Repositorio original)
                Subscripcion? sub = null;
                if (!reader.IsDBNull(reader.GetOrdinal("idSubscripcion")))
                {
                    sub = new Subscripcion
                    {
                        idSubscripcion = reader.GetInt32(reader.GetOrdinal("idSubscripcion")),
                        nombre = reader.GetString(reader.GetOrdinal("nombreSub")),
                        precio = Convert.ToSingle(reader["precio"]),
                        frecuenciaRecogida = reader.GetString(reader.GetOrdinal("frecuenciaRecogida")),
                        limitePrendas = reader.GetInt32(reader.GetOrdinal("limitePrendas"))
                    };
                }

                clientes.Add(new Cliente
                {
                    idCliente = reader.GetInt32(reader.GetOrdinal("idCliente")),
                    nombre = reader.GetString(reader.GetOrdinal("nombre")),
                    telefono = reader.GetString(reader.GetOrdinal("telefono")),
                    tipoSub = sub
                });
            }

            return clientes;
        }

        public async Task InsertCliente(Cliente cliente)
        {
            using var connection = await GetOpenConnectionAsync();

            const string sql = @"
                INSERT INTO Cliente (nombre, telefono, idSubscripcion)
                VALUES (@nombre, @telefono, @idSubscripcion)";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@nombre", cliente.nombre);
            command.Parameters.AddWithValue("@telefono", cliente.telefono);

            // Si no tiene suscripción, insertar NULL explícitamente
            if (cliente.tipoSub != null)
                command.Parameters.AddWithValue("@idSubscripcion", cliente.tipoSub.idSubscripcion);
            else
                command.Parameters.AddWithValue("@idSubscripcion", DBNull.Value);

            await command.ExecuteNonQueryAsync();
        }
    }
}