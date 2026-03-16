using costa_serena_grand_hotel_FRONTEND.Dtos;
using costa_serena_grand_hotel_FRONTEND.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace costa_serena_grand_hotel_FRONTEND.Pages.Szobak
{
    public class DetailsModel : PageModel
    {
        private readonly SzobakApi _szobakApi;

        public DetailsModel(SzobakApi szobakApi)
        {
            _szobakApi = szobakApi;
        }

        public SzobaDto? Szoba { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Szoba = await _szobakApi.GetByIdAsync(id);

            if (Szoba == null)
                return NotFound();

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
    }
}