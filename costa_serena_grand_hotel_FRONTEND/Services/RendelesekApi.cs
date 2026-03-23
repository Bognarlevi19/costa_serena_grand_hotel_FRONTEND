using costa_serena_grand_hotel_FRONTEND.Dtos;
using System.Net.Http.Json;

namespace costa_serena_grand_hotel_FRONTEND.Services
{
    public class RendelesekApi
    {
        private readonly IHttpClientFactory _f;

        public RendelesekApi(IHttpClientFactory f)
        {
            _f = f;
        }

        public async Task<OrderResultDto> CreateAsync(CreateOrderDto dto)
        {
            var response = await _f.CreateClient("costa_serena_grand_hotel_API")
                .PostAsJsonAsync("api/Rendeles", dto);

            if (!response.IsSuccessStatusCode)
            {
                var hiba = await response.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(hiba)
                    ? "A rendelés mentése nem sikerült."
                    : hiba);
            }

            var result = await response.Content.ReadFromJsonAsync<OrderResultDto>();

            if (result == null)
                throw new Exception("A rendelés mentése sikerült, de az API nem adott vissza érvényes választ.");

            return result;
        }
    }
}