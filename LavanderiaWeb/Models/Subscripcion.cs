namespace LavanderiaWeb.Models
{
    public class Subscripcion
    {
        public int idSubscripcion { get; set; }
        public string nombre { get; set; }
        public float precio { get; set; }
        public string frecuenciaRecogida { get; set; }
        public int limitePrendas { get; set; }
    }
}
