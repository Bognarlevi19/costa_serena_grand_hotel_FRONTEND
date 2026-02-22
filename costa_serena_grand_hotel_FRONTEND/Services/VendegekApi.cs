using costa_serena_grand_hotel_FRONTEND.Dtos;

namespace costa_serena_grand_hotel_FRONTEND.Services
{
    public class VendegekApi
    {
        private readonly IHttpClientFactory _f;
        public VendegekApi(IHttpClientFactory f) => _f = f;

        public async Task<List<VendegDto>> GetAllAsync()
            => await _f.CreateClient("costa_serena_grand_hotel_API")
                .GetFromJsonAsync<List<VendegDto>>("api/Vendeg") ?? new();

        public async Task<VendegDto?> GetByIdAsync(int id)
            => await _f.CreateClient("costa_serena_grand_hotel_API")
                .GetFromJsonAsync<VendegDto>($"api/Vendeg/{id}");

        public async Task CreateAsync(VendegDto dto)
        {
            var r = await _f.CreateClient("costa_serena_grand_hotel_API")
                .PostAsJsonAsync("api/Vendeg", dto);

            r.EnsureSuccessStatusCode();
        }

        public async Task UpdateAsync(int id, VendegDto dto)
        {
            var r = await _f.CreateClient("costa_serena_grand_hotel_API")
                .PutAsJsonAsync($"api/Vendeg/{id}", dto);

            r.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int id)
        {
            var r = await _f.CreateClient("costa_serena_grand_hotel_API")
                .DeleteAsync($"api/Vendeg/{id}");

            r.EnsureSuccessStatusCode();
        }
    }
}
