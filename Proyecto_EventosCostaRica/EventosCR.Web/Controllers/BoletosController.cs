using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EventosCR.Web.Controllers
{
    public class BoletosController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BoletosController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public async Task<IActionResult> Comprar(int eventoId, int asientoId)
        {
            var usuarioId = 2; // Aquí pondrías el usuario logueado

            var client = _httpClientFactory.CreateClient("EventosAPI");
            var response = await client.PostAsync($"boletos/comprar?usuarioId={usuarioId}&eventoId={eventoId}&asientoId={asientoId}", null);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("MisBoletos");

            TempData["Error"] = "No se pudo completar la compra.";
            return RedirectToAction("Detalle", "Eventos", new { id = eventoId });
        }

        public async Task<IActionResult> MisBoletos()
        {
            var usuarioId = 2; // Usuario logueado
            var client = _httpClientFactory.CreateClient("EventosAPI");

            var response = await client.GetAsync($"boletos/usuario/{usuarioId}");
            var json = await response.Content.ReadAsStringAsync();

            var boletos = JsonSerializer.Deserialize<List<Models.BoletoViewModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(boletos);
        }
    }
}