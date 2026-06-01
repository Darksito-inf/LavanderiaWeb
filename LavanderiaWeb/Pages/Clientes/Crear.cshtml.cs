using LavanderiaWeb.Data;
using LavanderiaWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LavanderiaWeb.Pages.Clientes
{
    public class CrearModel : PageModel
    {
        private readonly ClienteDAO _clienteDAO;
        private readonly SubscripcionDAO _subscripcionDAO;

        public CrearModel(ClienteDAO clienteDAO, SubscripcionDAO subscripcionDAO)
        {
            _clienteDAO = clienteDAO;
            _subscripcionDAO = subscripcionDAO;
        }

        [BindProperty]
        public Cliente NuevoCliente { get; set; } = new();

        // ID de suscripción elegida en el formulario (0 = sin suscripción)
        [BindProperty]
        public int SubscripcionSeleccionada { get; set; }

        // Lista para poblar el <select>
        public List<SelectListItem> OpcionesSubscripcion { get; set; } = new();

        public async Task OnGetAsync()
        {
            await CargarSubscripcionesAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await CargarSubscripcionesAsync();
                return Page();
            }

            // Asignar suscripción si se seleccionó una
            if (SubscripcionSeleccionada > 0)
            {
                var subs = await _subscripcionDAO.GetSubscripciones();
                NuevoCliente.tipoSub = subs.FirstOrDefault(s => s.idSubscripcion == SubscripcionSeleccionada);
            }

            await _clienteDAO.InsertCliente(NuevoCliente);
            return RedirectToPage("/Clientes/Index");
        }

        private async Task CargarSubscripcionesAsync()
        {
            var subs = await _subscripcionDAO.GetSubscripciones();
            OpcionesSubscripcion = subs
                .Select(s => new SelectListItem
                {
                    Value = s.idSubscripcion.ToString(),
                    Text = $"{s.nombre} — ${s.precio:N0}/mes"
                })
                .ToList();

            // Opción vacía al inicio
            OpcionesSubscripcion.Insert(0, new SelectListItem
            {
                Value = "0",
                Text = "Sin suscripción"
            });
        }
    }
}