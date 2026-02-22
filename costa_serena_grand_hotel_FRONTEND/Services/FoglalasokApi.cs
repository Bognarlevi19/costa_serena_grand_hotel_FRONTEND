using costa_serena_grand_hotel_FRONTEND.Dtos;

namespace costa_serena_grand_hotel_FRONTEND.Services
{
    public class FoglalasokApi
    {
        private readonly IHttpClientFactory _f;
        public FoglalasokApi(IHttpClientFactory f) => _f = f;

        public async Task<List<FoglalasDto>> GetAllAsync()
            => await _f.CreateClient("costa_serena_grand_hotel_API")
                .GetFromJsonAsync<List<FoglalasDto>>("api/Foglalas") ?? new();

        public async Task<FoglalasDto?> GetByIdAsync(int id)
            => await _f.CreateClient("costa_serena_grand_hotel_API")
                .GetFromJsonAsync<FoglalasDto>($"api/Foglalas/{id}");

        public async Task CreateAsync(FoglalasDto dto)
        {
            var r = await _f.CreateClient("costa_serena_grand_hotel_API")
                .PostAsJsonAsync("api/Foglalas", dto);

            r.EnsureSuccessStatusCode();
        }

        public async Task UpdateAsync(int id, FoglalasDto dto)
        {
            var r = await _f.CreateClient("costa_serena_grand_hotel_API")
                .PutAsJsonAsync($"api/Foglalas/{id}", dto);

            r.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int id)
        {
            var r = await _f.CreateClient("costa_serena_grand_hotel_API")
                .DeleteAsync($"api/Foglalas/{id}");

            r.EnsureSuccessStatusCode();
        }
    }
}
