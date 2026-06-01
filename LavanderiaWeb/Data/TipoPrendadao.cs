using System.Data.SqlClient;
using LavanderiaWeb.Models;

namespace LavanderiaWeb.Data
{
    public class TipoPrendaDAO : BaseHelper
    {
        public TipoPrendaDAO(IConfiguration configuration) : base(configuration) { }

        public async Task<List<TipoPrenda>> GetTiposPrenda()
        {
            var tipos = new List<TipoPrenda>();

            using var connection = await GetOpenConnectionAsync();

            const string sql = @"
                SELECT idPrenda, nombrePrenda, precioLavado, descripcion
                FROM TipoPrenda";

            using var command = new SqlCommand(sql, connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                tipos.Add(new TipoPrenda
                {
                    idPrenda = reader.GetInt32(reader.GetOrdinal("idPrenda")),
                    nombrePrenda = reader.GetString(reader.GetOrdinal("nombrePrenda")),
                    precioLavado = reader.GetFloat(reader.GetOrdinal("precioLavado")),     // float
                    descripcion = reader.GetString(reader.GetOrdinal("descripcion"))
                });
            }

            return tipos;
        }

        public async Task InsertTipoPrenda(TipoPrenda tipoPrenda)
        {
            using var connection = await GetOpenConnectionAsync();

            const string sql = @"
                INSERT INTO TipoPrenda (nombrePrenda, precioLavado, descripcion)
                VALUES (@nombrePrenda, @precioLavado, @descripcion)";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@nombrePrenda", tipoPrenda.nombrePrenda);
            command.Parameters.AddWithValue("@precioLavado", tipoPrenda.precioLavado);
            command.Parameters.AddWithValue("@descripcion", tipoPrenda.descripcion);

            await command.ExecuteNonQueryAsync();
        }
    }
}