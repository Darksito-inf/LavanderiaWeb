namespace LavanderiaWeb.Models
{
    public class Cliente
    {
        public int idCliente { get; set; }
        public string nombre { get; set; }
        public string telefono { get; set; }
        public Subscripcion? tipoSub { get; set; }

        public Cliente()
        {

        }



        public Cliente(int id, string nombre, string telefono)
        {
            idCliente = id;
            this.nombre = nombre;
            this.telefono = telefono;
        }

    }
}