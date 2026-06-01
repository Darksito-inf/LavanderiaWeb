namespace LavanderiaWeb.Models
{
    public class PrendaOrden
    {
        public int idPrendaOrden { get; set; }
        public TipoPrenda tipoPrenda { get; set; }
        public int cantidad { get; set; }

        public float CalcularSubtotal()
        {
            float subtotal = tipoPrenda.precioLavado * cantidad;
            return subtotal;
        }
    }
}
