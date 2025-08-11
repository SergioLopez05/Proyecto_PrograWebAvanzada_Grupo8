using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using EventosCR.Web.Models;

namespace EventosCR.Web.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

        public UsuariosController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("Api");
            var resp = await client.GetAsync("api/usuarios");
            if (!resp.IsSuccessStatusCode) return View(new List<UsuarioViewModel>());

            var json = await resp.Content.ReadAsStringAsync();
            var usuarios = JsonSerializer.Deserialize<List<UsuarioViewModel>>(json, _jsonOptions);
            return View(usuarios);
        }

        public async Task<IActionResult> Details(int id)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var resp = await client.GetAsync($"api/usuarios/{id}");
            if (!resp.IsSuccessStatusCode) return NotFound();

            var json = await resp.Content.ReadAsStringAsync();
            var usuario = JsonSerializer.Deserialize<UsuarioViewModel>(json, _jsonOptions);
            return View(usuario);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsuarioViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var client = _httpClientFactory.CreateClient("Api");
            var payload = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var resp = await client.PostAsync("api/usuarios", payload);

            if (resp.IsSuccessStatusCode) return RedirectToAction(nameof(Index));

            ModelState.AddModelError(string.Empty, "No se pudo crear el usuario.");
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var resp = await client.GetAsync($"api/usuarios/{id}");
            if (!resp.IsSuccessStatusCode) return NotFound();

            var json = await resp.Content.ReadAsStringAsync();
            var usuario = JsonSerializer.Deserialize<UsuarioViewModel>(json, _jsonOptions);
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UsuarioViewModel model)
        {
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);

            var client = _httpClientFactory.CreateClient("Api");
            var payload = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var resp = await client.PutAsync($"api/usuarios/{id}", payload);

            if (resp.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
            ModelState.AddModelError(string.Empty, "No se pudo actualizar el usuario.");
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var resp = await client.GetAsync($"api/usuarios/{id}");
            if (!resp.IsSuccessStatusCode) return NotFound();

            var json = await resp.Content.ReadAsStringAsync();
            var usuario = JsonSerializer.Deserialize<UsuarioViewModel>(json, _jsonOptions);
            return View(usuario);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var resp = await client.DeleteAsync($"api/usuarios/{id}");

            if (resp.IsSuccessStatusCode) return RedirectToAction(nameof(Index));

            ModelState.AddModelError(string.Empty, "No se pudo eliminar el usuario.");
            return RedirectToAction(nameof(Delete), new { id });
        }
    }
}

