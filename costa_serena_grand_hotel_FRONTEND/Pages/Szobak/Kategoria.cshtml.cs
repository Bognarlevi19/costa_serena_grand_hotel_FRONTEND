using costa_serena_grand_hotel_FRONTEND.Dtos;
using costa_serena_grand_hotel_FRONTEND.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace costa_serena_grand_hotel_FRONTEND.Pages.Szobak
{
    public class KategoriaModel : PageModel
    {
        private readonly SzobaKategoriakApi _szobaKategoriakApi;
        private readonly SzobakApi _szobakApi;

        public KategoriaModel(SzobaKategoriakApi szobaKategoriakApi, SzobakApi szobakApi)
        {
            _szobaKategoriakApi = szobaKategoriakApi;
            _szobakApi = szobakApi;
        }

        public SzobaKategoriaDto? Kategoria { get; set; }
        public List<SzobaDto> Szobak { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Kategoria = await _szobaKategoriakApi.GetByIdAsync(id);

            if (Kategoria == null)
                return NotFound();

            Szobak = await _szobakApi.GetByKategoriaAsync(id);

            return Page();
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