using costa_serena_grand_hotel_FRONTEND.Dtos;

namespace costa_serena_grand_hotel_FRONTEND.Services
{
    public class SzobakApi
    {
        private readonly IHttpClientFactory _f;
        public SzobakApi(IHttpClientFactory f) => _f = f;

        public async Task<List<SzobaDto>> GetAllAsync()
            => await _f.CreateClient("costa_serena_grand_hotel_API")
                .GetFromJsonAsync<List<SzobaDto>>("api/Szoba") ?? new();

        public async Task<SzobaDto?> GetByIdAsync(int id)
            => await _f.CreateClient("costa_serena_grand_hotel_API")
                .GetFromJsonAsync<SzobaDto>($"api/Szoba/{id}");

        public async Task CreateAsync(SzobaDto dto)
        {
            var r = await _f.CreateClient("costa_serena_grand_hotel_API")
                .PostAsJsonAsync("api/Szoba", dto);

            r.EnsureSuccessStatusCode();
        }

        public async Task UpdateAsync(int id, SzobaDto dto)
        {
            var r = await _f.CreateClient("costa_serena_grand_hotel_API")
                .PutAsJsonAsync($"api/Szoba/{id}", dto);

            r.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int id)
        {
            var r = await _f.CreateClient("costa_serena_grand_hotel_API")
                .DeleteAsync($"api/Szoba/{id}");

            r.EnsureSuccessStatusCode();
        }
    }
}
