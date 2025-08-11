using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using EventosCR.Web.Models;
using Microsoft.AspNetCore.Authorization;

namespace EventosCostaRica.MVC.Controllers
{
    public class EventosController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EventosController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("Api"); 
            var response = await client.GetAsync("api/eventos"); 

            if (!response.IsSuccessStatusCode)
                return View(new List<EventoViewModel>());

            var json = await response.Content.ReadAsStringAsync();
            var eventos = JsonSerializer.Deserialize<List<EventoViewModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(eventos);
        }

        [Authorize]
        public async Task<IActionResult> Detalle(int id)
        {
            System.Diagnostics.Debug.WriteLine("#####################################");
            var client = _httpClientFactory.CreateClient("Api");

            // Obtener detalles del evento
            var responseEvento = await client.GetAsync($"api/eventos/{id}");
            if (!responseEvento.IsSuccessStatusCode)
                return NotFound();

            var jsonEvento = await responseEvento.Content.ReadAsStringAsync();
            var evento = JsonSerializer.Deserialize<EventoViewModel>(jsonEvento, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Obtener asientos disponibles
            var asientos = await client.GetFromJsonAsync<List<AsientoViewModel>>($"api/asientos/disponibles/{id}");
            ViewBag.AsientosDisponibles = asientos ?? new List<AsientoViewModel>();

            // Determinar si está agotado
            ViewBag.EstaAgotado = (asientos == null || asientos.Count == 0);

            return View(evento);
        }


    }
}
