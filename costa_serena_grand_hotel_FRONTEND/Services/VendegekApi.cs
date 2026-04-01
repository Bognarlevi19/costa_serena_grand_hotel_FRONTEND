using costa_serena_grand_hotel_FRONTEND.Dtos;
using System.Net.Http.Json;

namespace costa_serena_grand_hotel_FRONTEND.Services
{
    public class VendegekApi
    {
        private readonly IHttpClientFactory _f;

        public VendegekApi(IHttpClientFactory f)
        {
            _f = f;
        }

        public async Task<List<VendegDto>> GetAllAsync()
        {
            return await _f.CreateClient("costa_serena_grand_hotel_API")
                .GetFromJsonAsync<List<VendegDto>>("api/Vendeg")
                ?? new List<VendegDto>();
        }

        public async Task<VendegDto?> GetByIdAsync(int id)
        {
            return await _f.CreateClient("costa_serena_grand_hotel_API")
                .GetFromJsonAsync<VendegDto>($"api/Vendeg/{id}");
        }

        public async Task<VendegDto?> GetCurrentAsync()
        {
            return await _f.CreateClient("costa_serena_grand_hotel_API")
                .GetFromJsonAsync<VendegDto>("api/Vendeg/me");
        }

        public async Task CreateAsync(VendegDto dto)
        {
            var response = await _f.CreateClient("costa_serena_grand_hotel_API")
                .PostAsJsonAsync("api/Vendeg", dto);

            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateAsync(int id, VendegDto dto)
        {
            var response = await _f.CreateClient("costa_serena_grand_hotel_API")
                .PutAsJsonAsync($"api/Vendeg/{id}", dto);

            response.EnsureSuccessStatusCode();
        }
        public async Task UpdateOwnAsync(VendegDto dto)
        {
            var response = await _f.CreateClient("costa_serena_grand_hotel_API")
                .PutAsJsonAsync("api/Vendeg/me", dto);

            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _f.CreateClient("costa_serena_grand_hotel_API")
                .DeleteAsync($"api/Vendeg/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var hiba = await response.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(hiba) ? "A törlés nem sikerült." : hiba);
            }
        }
    }
}