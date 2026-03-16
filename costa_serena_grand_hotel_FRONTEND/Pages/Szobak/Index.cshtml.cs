using costa_serena_grand_hotel_FRONTEND.Dtos;
using costa_serena_grand_hotel_FRONTEND.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace costa_serena_grand_hotel_FRONTEND.Pages.Szobak
{
    public class IndexModel : PageModel
    {
        private readonly SzobaKategoriakApi _szobaKategoriakApi;

        public IndexModel(SzobaKategoriakApi szobaKategoriakApi)
        {
            _szobaKategoriakApi = szobaKategoriakApi;
        }

        public List<SzobaKategoriaDto> Kategoriak { get; set; } = new();

        public async Task OnGetAsync()
        {
            Kategoriak = await _szobaKategoriakApi.GetAllAsync();
        }

        public List<string> GetKepek(string? kepekJson)
        {
            if (string.IsNullOrWhiteSpace(kepekJson))
                return new List<string>();

            try
            {
                return JsonSerializer.Deserialize<List<string>>(kepekJson) ?? new List<string>();
            }
            catch
            {
                return new List<string>();
            }
        }

        public string? GetElsoKep(string? kepekJson)
        {
            var kepek = GetKepek(kepekJson);
            return kepek.FirstOrDefault();
        }
    }
}