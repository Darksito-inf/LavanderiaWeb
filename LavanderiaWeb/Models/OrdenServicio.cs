using System;
using System.Collections.Generic;

namespace LavanderiaWeb.Models
{
    public class OrdenServicio
    {
        public int idOrden { get; set; }
        public string estado { get; set; }
        public Cliente cliente { get; set; }
        public DateTime fecha { get; set; }
        public List<PrendaOrden> prendas { get; set; }
        public float total { get; set; }

        public float CalcularTotal()
        {
            float total = 0;
            foreach (PrendaOrden prenda in prendas)
            {
                total += prenda.CalcularSubtotal();
            }
            return total;
        }
    }

}
