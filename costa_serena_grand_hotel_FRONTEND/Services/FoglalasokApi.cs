using costa_serena_grand_hotel_FRONTEND.Dtos;
using System.Net.Http.Json;

namespace costa_serena_grand_hotel_FRONTEND.Services
{
    public class FoglalasokApi
    {
        private readonly IHttpClientFactory _f;

        public FoglalasokApi(IHttpClientFactory f)
        {
            _f = f;
        }

        public async Task<List<FoglalasDto>> GetAllAsync()
        {
            return await _f.CreateClient("costa_serena_grand_hotel_API")
                .GetFromJsonAsync<List<FoglalasDto>>("api/Foglalas")
                ?? new List<FoglalasDto>();
        }

        public async Task<FoglalasDto?> GetByIdAsync(int id)
        {
            return await _f.CreateClient("costa_serena_grand_hotel_API")
                .GetFromJsonAsync<FoglalasDto>($"api/Foglalas/{id}");
        }

        public async Task<FoglalasEredmenyDto> CreateAsync(FoglalasDto dto)
        {
            var client = _f.CreateClient("costa_serena_grand_hotel_API");

            var response = await client.PostAsJsonAsync("api/Foglalas", dto);

            if (!response.IsSuccessStatusCode)
            {
                var hiba = await response.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(hiba) ? "A foglalás mentése nem sikerült." : hiba);
            }

            var result = await response.Content.ReadFromJsonAsync<FoglalasEredmenyDto>();

            if (result == null)
                throw new Exception("A foglalás mentése sikerült, de az API nem adott vissza érvényes választ.");

            return result;
        }

        public async Task CreateAdminAsync(FoglalasDto dto)
        {
            var payload = new
            {
                dto.SzobaId,
                dto.VendegId,
                dto.Mettol,
                dto.Meddig,
                dto.Fizetett
            };

            var response = await _f.CreateClient("costa_serena_grand_hotel_API")
                .PostAsJsonAsync("api/Foglalas/admin", payload);

            await EnsureSuccess(response);
        }

        public async Task UpdateAsync(int id, FoglalasDto dto)
        {
            var response = await _f.CreateClient("costa_serena_grand_hotel_API")
                .PutAsJsonAsync($"api/Foglalas/{id}", dto);

            await EnsureSuccess(response);
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _f.CreateClient("costa_serena_grand_hotel_API")
                .DeleteAsync($"api/Foglalas/{id}");

            await EnsureSuccess(response);
        }

        public async Task<List<SajatFoglalasDto>> GetOwnAsync()
        {
            return await _f.CreateClient("costa_serena_grand_hotel_API")
                .GetFromJsonAsync<List<SajatFoglalasDto>>("api/Foglalas/sajat")
                ?? new List<SajatFoglalasDto>();
        }
        public async Task<List<FoglaltIdoszakDto>> GetBlockedRangesAsync(int szobaId)
        {
            return await _f.CreateClient("costa_serena_grand_hotel_API")
                .GetFromJsonAsync<List<FoglaltIdoszakDto>>($"api/Foglalas/szoba/{szobaId}/foglalt-idoszakok")
                ?? new List<FoglaltIdoszakDto>();
        }

        private static async Task EnsureSuccess(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return;

            var error = await response.Content.ReadAsStringAsync();
            throw new Exception(string.IsNullOrWhiteSpace(error) ? "A művelet nem sikerült." : error);
        }
    }
}