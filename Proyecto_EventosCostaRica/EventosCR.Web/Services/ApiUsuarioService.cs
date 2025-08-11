using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using EventosCR.Web.Models;

namespace EventosCR.Web.Services
{
    public class ApiUsuarioService
    {
        private readonly HttpClient _httpClient;

        public ApiUsuarioService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Api");
        }

        public async Task<UsuarioViewModel?> GetUsuarioByEmailAsync(string email)
        {
            var url = $"api/usuarios/por-email?email={email}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<UsuarioViewModel>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
