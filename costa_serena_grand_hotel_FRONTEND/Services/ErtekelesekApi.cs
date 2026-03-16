using costa_serena_grand_hotel_FRONTEND.Dtos;
using System.Net.Http.Headers;

namespace costa_serena_grand_hotel_FRONTEND.Services
{
    public class ErtekelesekApi
    {
        private readonly IHttpClientFactory _factory;
        private readonly AuthSession _authSession;

        public ErtekelesekApi(IHttpClientFactory factory, AuthSession authSession)
        {
            _factory = factory;
            _authSession = authSession;
        }

        public async Task<List<ErtekelesDto>> GetAllAsync(CancellationToken ct = default)
        {
            var client = _factory.CreateClient("costa_serena_grand_hotel_API");

            var result = await client.GetFromJsonAsync<List<ErtekelesDto>>("api/ertekelesek", ct);
            return result ?? new List<ErtekelesDto>();
        }

        public async Task CreateAsync(int rating, string comment, CancellationToken ct = default)
        {
            var client = _factory.CreateClient("costa_serena_grand_hotel_API");

            var token = _authSession.GetToken();
            if (string.IsNullOrWhiteSpace(token))
                throw new Exception("Nincs bejelentkezett felhasználó.");

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsJsonAsync("api/ertekelesek", new
            {
                Rating = rating,
                Comment = comment
            }, ct);

            var content = await response.Content.ReadAsStringAsync(ct);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Backend hiba: {content}");
        }
    }
}
