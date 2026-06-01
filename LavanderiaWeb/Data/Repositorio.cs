using LavanderiaWeb.Models;

namespace LavanderiaWeb.Data
{
    // Clase estática que actúa como almacenamiento en memoria durante la sesión
    public static class Repositorio
    {
        public static List<Subscripcion> Subscripciones { get; set; } = new()
        {
            new Subscripcion { idSubscripcion = 1, nombre = "Básico",   precio = 9990,  frecuenciaRecogida = "Semanal", limitePrendas = 10 },
            new Subscripcion { idSubscripcion = 2, nombre = "Premium",  precio = 19990, frecuenciaRecogida = "Diaria",  limitePrendas = 30 },
            new Subscripcion { idSubscripcion = 3, nombre = "Estándar", precio = 14990, frecuenciaRecogida = "Semanal", limitePrendas = 20 },
        };

        public static List<Cliente> Clientes { get; set; } = new()
        {
            new Cliente(1, "Juan Pérez",   "912345678") { tipoSub = null },
            new Cliente(2, "María López",  "987654321") { tipoSub = null },
            new Cliente(3, "Carlos Soto",  "956781234") { tipoSub = null },
        };

        public static List<TipoPrenda> TiposPrenda { get; set; } = new()
        {
            new TipoPrenda { idPrenda = 1, nombrePrenda = "Camisa",   precioLavado = 1500, descripcion = "Lavado y planchado" },
            new TipoPrenda { idPrenda = 2, nombrePrenda = "Pantalón", precioLavado = 2000, descripcion = "Lavado en seco"    },
            new TipoPrenda { idPrenda = 3, nombrePrenda = "Abrigo",   precioLavado = 4500, descripcion = "Lavado especial"   },
        };

        public static List<OrdenServicio> Ordenes { get; set; } = new();

        // Contadores para IDs autoincrementales
        private static int _nextCliente   = 4;
        private static int _nextPrenda    = 4;
        private static int _nextSub       = 4;
        private static int _nextOrden     = 1;
        private static int _nextPrendaOrden = 1;

        public static int SiguienteIdCliente()    => _nextCliente++;
        public static int SiguienteIdPrenda()     => _nextPrenda++;
        public static int SiguienteIdSub()        => _nextSub++;
        public static int SiguienteIdOrden()      => _nextOrden++;
        public static int SiguienteIdPrendaOrden() => _nextPrendaOrden++;
    }
}
