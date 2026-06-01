using System.Data.SqlClient;
using LavanderiaWeb.Models;

namespace LavanderiaWeb.Data
{
    public class OrdenServicioDAO : BaseHelper
    {
        public OrdenServicioDAO(IConfiguration configuration) : base(configuration) { }

        public async Task<List<OrdenServicio>> GetOrdenes()
        {
            var ordenes = new List<OrdenServicio>();

            using var connection = await GetOpenConnectionAsync();

            // Query 1: cabecera de cada orden + datos del cliente
            const string sqlOrdenes = @"
                SELECT o.idOrden, o.estado, o.fecha, o.total,
                       c.idCliente, c.nombre, c.telefono
                FROM OrdenServicio o
                INNER JOIN Cliente c ON o.idCliente = c.idCliente";

            using var cmdOrdenes = new SqlCommand(sqlOrdenes, connection);
            using var readerOrdenes = await cmdOrdenes.ExecuteReaderAsync();

            while (await readerOrdenes.ReadAsync())
            {
                ordenes.Add(new OrdenServicio
                {
                    idOrden = readerOrdenes.GetInt32(readerOrdenes.GetOrdinal("idOrden")),
                    estado = readerOrdenes.GetString(readerOrdenes.GetOrdinal("estado")),
                    fecha = readerOrdenes.GetDateTime(readerOrdenes.GetOrdinal("fecha")),
                    total = readerOrdenes.GetFloat(readerOrdenes.GetOrdinal("total")),   // float
                    cliente = new Cliente
                    {
                        idCliente = readerOrdenes.GetInt32(readerOrdenes.GetOrdinal("idCliente")),
                        nombre = readerOrdenes.GetString(readerOrdenes.GetOrdinal("nombre")),
                        telefono = readerOrdenes.GetString(readerOrdenes.GetOrdinal("telefono"))
                    },
                    prendas = new List<PrendaOrden>()
                });
            }

            // Query 2: prendas de cada orden (requiere nueva lectura, el reader anterior ya cerró)
            const string sqlPrendas = @"
                SELECT po.idPrendaOrden, po.cantidad, po.idOrden,
                       tp.idPrenda, tp.nombrePrenda, tp.precioLavado, tp.descripcion
                FROM PrendaOrden po
                INNER JOIN TipoPrenda tp ON po.idPrenda = tp.idPrenda
                WHERE po.idOrden = @idOrden";

            foreach (var orden in ordenes)
            {
                using var cmdPrendas = new SqlCommand(sqlPrendas, connection);
                cmdPrendas.Parameters.AddWithValue("@idOrden", orden.idOrden);

                using var readerPrendas = await cmdPrendas.ExecuteReaderAsync();
                while (await readerPrendas.ReadAsync())
                {
                    orden.prendas.Add(new PrendaOrden
                    {
                        idPrendaOrden = readerPrendas.GetInt32(readerPrendas.GetOrdinal("idPrendaOrden")),
                        cantidad = readerPrendas.GetInt32(readerPrendas.GetOrdinal("cantidad")),
                        tipoPrenda = new TipoPrenda
                        {
                            idPrenda = readerPrendas.GetInt32(readerPrendas.GetOrdinal("idPrenda")),
                            nombrePrenda = readerPrendas.GetString(readerPrendas.GetOrdinal("nombrePrenda")),
                            precioLavado = readerPrendas.GetFloat(readerPrendas.GetOrdinal("precioLavado")),
                            descripcion = readerPrendas.GetString(readerPrendas.GetOrdinal("descripcion"))
                        }
                    });
                }
            }

            return ordenes;
        }

        public async Task<int> InsertOrden(OrdenServicio orden)
        {
            using var connection = await GetOpenConnectionAsync();

            // INSERT orden y recuperar el ID generado (IDENTITY)
            const string sqlOrden = @"
                INSERT INTO OrdenServicio (idCliente, estado, fecha, total)
                VALUES (@idCliente, @estado, @fecha, @total);
                SELECT SCOPE_IDENTITY();";

            using var cmdOrden = new SqlCommand(sqlOrden, connection);
            cmdOrden.Parameters.AddWithValue("@idCliente", orden.cliente.idCliente);
            cmdOrden.Parameters.AddWithValue("@estado", orden.estado);
            cmdOrden.Parameters.AddWithValue("@fecha", orden.fecha);
            cmdOrden.Parameters.AddWithValue("@total", orden.total);

            // SCOPE_IDENTITY() devuelve decimal; convertimos a int
            var idGenerado = Convert.ToInt32(await cmdOrden.ExecuteScalarAsync());

            // INSERT de cada PrendaOrden asociada
            if (orden.prendas != null)
            {
                const string sqlPrenda = @"
                    INSERT INTO PrendaOrden (idOrden, idPrenda, cantidad)
                    VALUES (@idOrden, @idPrenda, @cantidad)";

                foreach (var prenda in orden.prendas)
                {
                    using var cmdPrenda = new SqlCommand(sqlPrenda, connection);
                    cmdPrenda.Parameters.AddWithValue("@idOrden", idGenerado);
                    cmdPrenda.Parameters.AddWithValue("@idPrenda", prenda.tipoPrenda.idPrenda);
                    cmdPrenda.Parameters.AddWithValue("@cantidad", prenda.cantidad);
                    await cmdPrenda.ExecuteNonQueryAsync();
                }
            }

            return idGenerado;
        }
    }
}