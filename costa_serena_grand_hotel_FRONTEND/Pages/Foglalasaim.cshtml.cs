using costa_serena_grand_hotel_FRONTEND.Dtos;
using costa_serena_grand_hotel_FRONTEND.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace costa_serena_grand_hotel_FRONTEND.Pages
{
    public class FoglalasaimModel : PageModel
    {
        private readonly FoglalasokApi _foglalasokApi;
        private readonly AuthSession _authSession;

        public FoglalasaimModel(FoglalasokApi foglalasokApi, AuthSession authSession)
        {
            _foglalasokApi = foglalasokApi;
            _authSession = authSession;
        }

        public List<SajatFoglalasDto> Foglalasok { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            if (!_authSession.IsSignedIn)
                return RedirectToPage("/Account/Login");

            Foglalasok = await _foglalasokApi.GetOwnAsync();
            return Page();
        }

        public string GetStatusText(SajatFoglalasDto foglalas)
        {
            var today = DateTime.Today;

            if (foglalas.Meddig.Date < today)
                return "Lejárt";

            if (foglalas.Mettol.Date <= today && foglalas.Meddig.Date >= today)
                return "Aktív";

            return "Közelgõ";
        }
    }
}