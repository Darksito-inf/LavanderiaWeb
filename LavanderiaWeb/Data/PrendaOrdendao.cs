using System.Data.SqlClient;
using LavanderiaWeb.Models;

namespace LavanderiaWeb.Data
{
    public class PrendaOrdenDAO : BaseHelper
    {
        public PrendaOrdenDAO(IConfiguration configuration) : base(configuration) { }

        /// <summary>
        /// Devuelve todas las PrendaOrden de una orden específica, con su TipoPrenda hidratado.
        /// </summary>
        public async Task<List<PrendaOrden>> GetPrendasPorOrden(int idOrden)
        {
            var prendas = new List<PrendaOrden>();

            using var connection = await GetOpenConnectionAsync();

            const string sql = @"
                SELECT po.idPrendaOrden, po.cantidad,
                       tp.idPrenda, tp.nombrePrenda, tp.precioLavado, tp.descripcion
                FROM PrendaOrden po
                INNER JOIN TipoPrenda tp ON po.idPrenda = tp.idPrenda
                WHERE po.idOrden = @idOrden";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@idOrden", idOrden);

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                prendas.Add(new PrendaOrden
                {
                    idPrendaOrden = reader.GetInt32(reader.GetOrdinal("idPrendaOrden")),
                    cantidad = reader.GetInt32(reader.GetOrdinal("cantidad")),
                    tipoPrenda = new TipoPrenda
                    {
                        idPrenda = reader.GetInt32(reader.GetOrdinal("idPrenda")),
                        nombrePrenda = reader.GetString(reader.GetOrdinal("nombrePrenda")),
                        precioLavado = reader.GetFloat(reader.GetOrdinal("precioLavado")),
                        descripcion = reader.GetString(reader.GetOrdinal("descripcion"))
                    }
                });
            }

            return prendas;
        }

        /// <summary>
        /// Inserta una prenda dentro de una orden existente.
        /// </summary>
        public async Task InsertPrendaOrden(int idOrden, PrendaOrden prendaOrden)
        {
            using var connection = await GetOpenConnectionAsync();

            const string sql = @"
                INSERT INTO PrendaOrden (idOrden, idPrenda, cantidad)
                VALUES (@idOrden, @idPrenda, @cantidad)";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@idOrden", idOrden);
            command.Parameters.AddWithValue("@idPrenda", prendaOrden.tipoPrenda.idPrenda);
            command.Parameters.AddWithValue("@cantidad", prendaOrden.cantidad);

            await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Elimina una prenda de una orden por su ID.
        /// </summary>
        public async Task DeletePrendaOrden(int idPrendaOrden)
        {
            using var connection = await GetOpenConnectionAsync();

            const string sql = "DELETE FROM PrendaOrden WHERE idPrendaOrden = @idPrendaOrden";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@idPrendaOrden", idPrendaOrden);

            await command.ExecuteNonQueryAsync();
        }
    }
}