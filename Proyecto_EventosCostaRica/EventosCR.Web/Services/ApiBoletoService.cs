using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EventosCR.Web.Models;

namespace EventosCR.Web.Services
{
    public class ApiBoletoService
    {
        private readonly HttpClient _httpClient;

        public ApiBoletoService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Api");
        }

        public async Task<BoletoViewModel?> ComprarBoletoAsync(int usuarioId, int asientoId)
        {
            var url = $"api/boletos/comprar?usuarioId={usuarioId}&asientoId={asientoId}";

            var response = await _httpClient.PostAsync(url, null);

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<BoletoViewModel>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<List<BoletoViewModel>> GetBoletosUsuarioAsync(int usuarioId)
        {
            var response = await _httpClient.GetAsync($"api/boletos/usuario/{usuarioId}");

            if (!response.IsSuccessStatusCode)
                return new List<BoletoViewModel>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<BoletoViewModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<BoletoViewModel>();
        }
    }
}
