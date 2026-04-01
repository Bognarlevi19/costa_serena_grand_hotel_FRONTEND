using costa_serena_grand_hotel_FRONTEND.Dtos;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

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

        public async Task CreateAdminAsync(RendelesAdminDto dto)
        {
            var payload = new
            {
                dto.VendegId,
                dto.Nev,
                dto.SzemelyiIgazolvanySzam,
                dto.IranyitoSzam,
                dto.Varos,
                dto.Utca,
                dto.Hazszam,
                dto.Vegosszeg,
                dto.Fizetett,
                dto.Elkuldve
            };

            var response = await _f.CreateClient("costa_serena_grand_hotel_API")
                .PostAsJsonAsync("api/Rendeles/admin", payload);

            await EnsureSuccess(response);
        }

        public async Task<List<RendelesAdminDto>> GetAllAsync()
        {
            return await _f.CreateClient("costa_serena_grand_hotel_API")
                .GetFromJsonAsync<List<RendelesAdminDto>>("api/Rendeles")
                ?? new List<RendelesAdminDto>();
        }

        public async Task<RendelesAdminDto?> GetByIdAsync(int id)
        {
            return await _f.CreateClient("costa_serena_grand_hotel_API")
                .GetFromJsonAsync<RendelesAdminDto>($"api/Rendeles/{id}");
        }

        public async Task UpdateAsync(int id, RendelesAdminDto dto)
        {
            var response = await _f.CreateClient("costa_serena_grand_hotel_API")
                .PutAsJsonAsync($"api/Rendeles/{id}", dto);

            await EnsureSuccess(response);
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _f.CreateClient("costa_serena_grand_hotel_API")
                .DeleteAsync($"api/Rendeles/{id}");

            await EnsureSuccess(response);
        }

        public async Task SetElkuldveAsync(int id, bool elkuldve)
        {
            var client = _f.CreateClient("costa_serena_grand_hotel_API");

            var json = JsonSerializer.Serialize(elkuldve);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PatchAsync($"api/Rendeles/{id}/elkuldve", content);
            await EnsureSuccess(response);
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