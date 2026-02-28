namespace costa_serena_grand_hotel_FRONTEND.Services
{
    public class AuthApi
    {
        private readonly IHttpClientFactory _f;
        public AuthApi(IHttpClientFactory f) => _f = f;
        public async Task RegisterAsync(string email, string password, CancellationToken ct = default)
        {
            var client = _f.CreateClient("costa_serena_grand_hotel_API");
            var resp = await client.PostAsJsonAsync("api/auth/register", new { email, password }, ct);
            resp.EnsureSuccessStatusCode();
        }
        public async Task<string> LoginAsync(string email, string password, CancellationToken ct = default)
        {
            var client = _f.CreateClient("costa_serena_grand_hotel_API");
            var resp = await client.PostAsJsonAsync("api/auth/login", new { email, password }, ct);
            resp.EnsureSuccessStatusCode();
            var data = await resp.Content.ReadFromJsonAsync<LoginResponse>(cancellationToken: ct);
            return data?.Token ?? throw new InvalidOperationException("Nincs token a válaszban.");
        }
        private sealed class LoginResponse
        {
            public string Token { get; set; } = "";
        }
    }
}
