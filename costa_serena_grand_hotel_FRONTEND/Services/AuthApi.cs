using costa_serena_grand_hotel_FRONTEND.Dtos;
using System.Text.Json;

namespace costa_serena_grand_hotel_FRONTEND.Services
{
    public class AuthApi
    {
        private readonly IHttpClientFactory _f;

        public AuthApi(IHttpClientFactory f)
        {
            _f = f;
        }

        public async Task<string> RegisterAsync(RegisterDto dto, CancellationToken ct = default)
        {
            var client = _f.CreateClient("costa_serena_grand_hotel_API");

            var resp = await client.PostAsJsonAsync("api/auth/register", dto, ct);

            var content = await resp.Content.ReadAsStringAsync(ct);

            if (!resp.IsSuccessStatusCode)
                throw new Exception($"Backend hiba: {content}");

            var data = JsonSerializer.Deserialize<RegisterResponse>(
                content,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (data == null || string.IsNullOrWhiteSpace(data.UserId))
                throw new Exception("A backend nem adott vissza userId-t.");

            return data.UserId;
        }

        public async Task<string> LoginAsync(string email, string password, CancellationToken ct = default)
        {
            var client = _f.CreateClient("costa_serena_grand_hotel_API");

            var resp = await client.PostAsJsonAsync("api/auth/login", new
            {
                email,
                password
            }, ct);

            var content = await resp.Content.ReadAsStringAsync(ct);

            if (!resp.IsSuccessStatusCode)
                throw new Exception($"Backend hiba: {content}");

            var data = JsonSerializer.Deserialize<LoginResponse>(
                content,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return data?.Token ?? throw new InvalidOperationException("Nincs token a válaszban.");
        }

        private sealed class RegisterResponse
        {
            public string UserId { get; set; } = "";
        }

        private sealed class LoginResponse
        {
            public string Token { get; set; } = "";
        }
    }
}
