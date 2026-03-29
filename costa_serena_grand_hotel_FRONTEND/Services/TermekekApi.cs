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

        public async Task<List<TermekDto>> GetAllAsync()
        {
            return await _f.CreateClient("costa_serena_grand_hotel_API")
                .GetFromJsonAsync<List<TermekDto>>("api/Termek")
                ?? new List<TermekDto>();
        }

        public async Task<TermekDto?> GetByIdAsync(int id)
        {
            return await _f.CreateClient("costa_serena_grand_hotel_API")
                .GetFromJsonAsync<TermekDto>($"api/Termek/{id}");
        }
    }
}