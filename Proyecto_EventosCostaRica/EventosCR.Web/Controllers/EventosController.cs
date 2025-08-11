using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using EventosCR.Web.Models;

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

        public async Task<IActionResult> Detalle(int id)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var response = await client.GetAsync($"api/eventos/{id}");
            var asientos = await client.GetFromJsonAsync<List<AsientoViewModel>>(
                $"api/Asientos/disponibles/{id}");

            ViewBag.Asientos = asientos ?? new List<AsientoViewModel>();

            if (!response.IsSuccessStatusCode)
            {
                // Agregar vista de error despues
                return NotFound(); // o View("Error");
            }

            var json = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound(); 
            }

            var evento = JsonSerializer.Deserialize<EventoViewModel>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(evento);

        }
    }
}
