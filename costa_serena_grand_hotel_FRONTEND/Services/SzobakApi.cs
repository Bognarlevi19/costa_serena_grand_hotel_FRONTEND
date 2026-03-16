using costa_serena_grand_hotel_FRONTEND.Dtos;
using System.Net.Http.Json;

namespace costa_serena_grand_hotel_FRONTEND.Services
{
    public class SzobakApi
    {
        private readonly IHttpClientFactory _f;

        public SzobakApi(IHttpClientFactory f)
        {
            _f = f;
        }

        public async Task<List<SzobaDto>> GetAllAsync()
        {
            return await _f.CreateClient("costa_serena_grand_hotel_API")
                .GetFromJsonAsync<List<SzobaDto>>("api/Szoba")
                ?? new List<SzobaDto>();
        }

        public async Task<List<SzobaDto>> GetByKategoriaAsync(int kategoriaId)
        {
            return await _f.CreateClient("costa_serena_grand_hotel_API")
                .GetFromJsonAsync<List<SzobaDto>>($"api/Szoba/kategoria/{kategoriaId}")
                ?? new List<SzobaDto>();
        }

        public async Task<SzobaDto?> GetByIdAsync(int id)
        {
            return await _f.CreateClient("costa_serena_grand_hotel_API")
                .GetFromJsonAsync<SzobaDto>($"api/Szoba/{id}");
        }

        public async Task CreateAsync(SzobaDto dto)
        {
            var response = await _f.CreateClient("costa_serena_grand_hotel_API")
                .PostAsJsonAsync("api/Szoba", dto);

            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateAsync(int id, SzobaDto dto)
        {
            var response = await _f.CreateClient("costa_serena_grand_hotel_API")
                .PutAsJsonAsync($"api/Szoba/{id}", dto);

            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _f.CreateClient("costa_serena_grand_hotel_API")
                .DeleteAsync($"api/Szoba/{id}");

            response.EnsureSuccessStatusCode();
        }
    }
}