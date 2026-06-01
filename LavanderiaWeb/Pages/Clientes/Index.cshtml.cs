using LavanderiaWeb.Data;
using LavanderiaWeb.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LavanderiaWeb.Pages.Clientes
{
    public class IndexModel : PageModel
    {
        private readonly ClienteDAO _clienteDAO;

        public IndexModel(ClienteDAO clienteDAO)
        {
            _clienteDAO = clienteDAO;
        }

        public List<Cliente> Clientes { get; set; } = new();

        public async Task OnGetAsync()
        {
            Clientes = await _clienteDAO.GetClientes();
        }
    }
}
