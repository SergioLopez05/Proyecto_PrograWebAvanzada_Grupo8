using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Security.Claims;
using EventosCR.Web.Models;

namespace EventosCR.Web.Services
{
    public class ApiAuthService
    {
        private readonly HttpClient _httpClient;

        public ApiAuthService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Api");  // Usa el cliente con nombre "Api"
        }

        public async Task<LoginResult?> LoginAsync(LoginViewModel model)
        {
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/auth/login", content);

            if (!response.IsSuccessStatusCode) return null;

            var result = await response.Content.ReadAsStringAsync();

            // intentar deserializar como { "token":"..." } o recibir directamente el token
            string token;
            try
            {
                var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (dict != null && dict.ContainsKey("token"))
                {
                    token = dict["token"];
                }
                else
                {
                    // si la API devolviera directamente el token como string (raro), lo cogemos
                    token = result.Trim('"');
                }
            }
            catch
            {
                token = result.Trim('"');
            }

            if (string.IsNullOrWhiteSpace(token)) return null;

            // Parsear el JWT para obtener rol, nombre e id sin tener que cambiar la API
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            // buscar claims con distintas convenciones
            var role = jwt.Claims.FirstOrDefault(c =>
                c.Type == ClaimTypes.Role || c.Type.Equals("role", StringComparison.OrdinalIgnoreCase))?.Value;

            var name = jwt.Claims.FirstOrDefault(c =>
                c.Type == ClaimTypes.Name || c.Type.Equals("unique_name", StringComparison.OrdinalIgnoreCase)
                || c.Type.Equals(JwtRegisteredClaimNames.UniqueName, StringComparison.OrdinalIgnoreCase)
                || c.Type.Equals("name", StringComparison.OrdinalIgnoreCase))?.Value;

            var userId = jwt.Claims.FirstOrDefault(c =>
                c.Type == ClaimTypes.NameIdentifier || c.Type.Equals(JwtRegisteredClaimNames.Sub, StringComparison.OrdinalIgnoreCase)
                || c.Type.Equals("sub", StringComparison.OrdinalIgnoreCase))?.Value;

            return new LoginResult
            {
                Token = token,
                Role = role,
                UserName = name,
                UserId = userId
            };
        }

        public async Task<bool> RegisterAsync(RegisterViewModel model)
        {
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/auth/register", content);
            return response.IsSuccessStatusCode;
        }
    }
}
