using costa_serena_grand_hotel_FRONTEND.Dtos;
using System.Net.Http.Json;

namespace costa_serena_grand_hotel_FRONTEND.Services
{
    public class TermekekApi
    {
        private readonly IHttpClientFactory _f;

        public TermekekApi(IHttpClientFactory f)
        {
            _f = f;
        }

        public async Task<List<TermekDto>> GetAllAsync(bool admin = false)
        {
            var url = admin ? "api/Termek/admin" : "api/Termek";

            return await _f.CreateClient("costa_serena_grand_hotel_API")
                .GetFromJsonAsync<List<TermekDto>>(url)
                ?? new List<TermekDto>();
        }

        public async Task<TermekDto?> GetByIdAsync(int id)
        {
            return await _f.CreateClient("costa_serena_grand_hotel_API")
                .GetFromJsonAsync<TermekDto>($"api/Termek/{id}");
        }

        public async Task CreateAsync(TermekDto dto)
        {
            var response = await _f.CreateClient("costa_serena_grand_hotel_API")
                .PostAsJsonAsync("api/Termek", dto);

            await EnsureSuccess(response);
        }

        public async Task UpdateAsync(int id, TermekDto dto)
        {
            var response = await _f.CreateClient("costa_serena_grand_hotel_API")
                .PutAsJsonAsync($"api/Termek/{id}", dto);

            await EnsureSuccess(response);
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var response = await _f.CreateClient("costa_serena_grand_hotel_API")
                    .DeleteAsync($"api/Termek/{id}");

                await EnsureSuccess(response);
            }
            catch (HttpRequestException)
            {
                throw new Exception("A termék törlése közben szerverhiba történt.");
            }
        }

        private static async Task EnsureSuccess(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return;

            var error = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(error))
                throw new Exception("A művelet nem sikerült.");

            throw new Exception(error);
        }
    }
}