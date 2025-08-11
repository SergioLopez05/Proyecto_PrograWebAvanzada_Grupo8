using Microsoft.AspNetCore.Mvc;
using EventosCR.Web.Models;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Text.Json;

namespace EventosCR.MVC.Controllers
{
    [Authorize]
    public class BoletosController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BoletosController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: /Boletos/Comprar?eventoId=1
        public async Task<IActionResult> Comprar(int eventoId)
        {
            var client = _httpClientFactory.CreateClient("Api");

            var eventoResponse = await client.GetAsync($"api/eventos/{eventoId}");
            if (!eventoResponse.IsSuccessStatusCode)
                return NotFound();

            var eventoJson = await eventoResponse.Content.ReadAsStringAsync();
            var evento = JsonSerializer.Deserialize<EventoViewModel>(eventoJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var asientos = await client.GetFromJsonAsync<List<AsientoViewModel>>($"api/Asientos/evento/{eventoId}");

            var model = new CompraBoletosViewModel
            {
                EventoId = eventoId,
                EventoNombre = evento?.Nombre ?? "Evento",
                Asientos = asientos ?? new List<AsientoViewModel>()
            };

            return View(model);
        }

        // POST: /Boletos/comprar?usuarioId=5&asientoId=78
        [HttpPost]
        public async Task<IActionResult> Comprar(int EventoId, int AsientoId)
        {
            var client = _httpClientFactory.CreateClient("Api");

            var userEmail = User.Identity.Name;
            if (string.IsNullOrEmpty(userEmail))
            {
                ModelState.AddModelError("", "No se pudo obtener el usuario autenticado.");
                return await Comprar(EventoId);
            }

            var userResponse = await client.GetAsync($"api/usuarios/email/{userEmail}");
            if (!userResponse.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "No se pudo encontrar el usuario.");
                return await Comprar(EventoId);
            }

            var usuario = await userResponse.Content.ReadFromJsonAsync<UsuarioViewModel>();
            if (usuario == null)
            {
                ModelState.AddModelError("", "El usuario no fue encontrado.");
                return await Comprar(EventoId);
            }

            // Cambia a PascalCase
            var compra = new
            {
                UsuarioId = usuario.Id,
                AsientoId = AsientoId
            };

            var response = await client.PostAsync(
                $"api/boletos/comprar?usuarioId={usuario.Id}&asientoId={AsientoId}", null);

            var responseContent = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine($"[DEBUG] Response Status: {response.StatusCode}");
            System.Diagnostics.Debug.WriteLine($"[DEBUG] Response Content: {responseContent}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("MisBoletos");
            }
            else
            {
                ModelState.AddModelError("", $"Error de la API: {responseContent}");
                return await Comprar(EventoId);
            }
        }




            //if (response.IsSuccessStatusCode)
            //{
            //    return RedirectToAction("MisBoletos");
            //}
            //else
            //{
            //    ModelState.AddModelError("", "No se pudo completar la compra. El asiento puede estar ocupado.");
            //    return await Comprar(EventoId);
            //}
        




        // GET: /Boletos/MisBoletos
        public async Task<IActionResult> MisBoletos()
        {
            var client = _httpClientFactory.CreateClient("Api");

            var userEmail = User.Identity.Name;

            var userResponse = await client.GetAsync($"api/usuarios/email/{userEmail}");


            var usuario = await userResponse.Content.ReadFromJsonAsync<UsuarioViewModel>();



            var response = await client.GetAsync($"api/boletos/usuario/{usuario.Id}");

            if (!response.IsSuccessStatusCode)
            {
                return View(new List<BoletoViewModel>());
            }

            var json = await response.Content.ReadAsStringAsync();

            var boletos = JsonSerializer.Deserialize<List<BoletoViewModel>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(boletos);
        }
    }
}
