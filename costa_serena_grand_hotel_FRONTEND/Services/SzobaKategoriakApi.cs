using costa_serena_grand_hotel_FRONTEND.Dtos;
using System.Net.Http.Json;

namespace costa_serena_grand_hotel_FRONTEND.Services
{
    public class SzobaKategoriakApi
    {
        private readonly IHttpClientFactory _f;

        public SzobaKategoriakApi(IHttpClientFactory f)
        {
            _f = f;
        }

        public async Task<List<SzobaKategoriaDto>> GetAllAsync()
        {
            return await _f.CreateClient("costa_serena_grand_hotel_API")
                .GetFromJsonAsync<List<SzobaKategoriaDto>>("api/SzobaKategoria")
                ?? new List<SzobaKategoriaDto>();
        }

        public async Task<SzobaKategoriaDto?> GetByIdAsync(int id)
        {
            return await _f.CreateClient("costa_serena_grand_hotel_API")
                .GetFromJsonAsync<SzobaKategoriaDto>($"api/SzobaKategoria/{id}");
        }
    }
}