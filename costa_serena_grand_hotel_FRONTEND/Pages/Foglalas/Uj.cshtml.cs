using System.Text.Json;
using costa_serena_grand_hotel_FRONTEND.Dtos;
using costa_serena_grand_hotel_FRONTEND.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace costa_serena_grand_hotel_FRONTEND.Pages.Foglalas
{
    public class UjModel : PageModel
    {
        private readonly SzobakApi _szobakApi;
        private readonly VendegekApi _vendegekApi;
        private readonly FoglalasokApi _foglalasokApi;
        private readonly AuthSession _authSession;

        public UjModel(
            SzobakApi szobakApi,
            VendegekApi vendegekApi,
            FoglalasokApi foglalasokApi,
            AuthSession authSession)
        {
            _szobakApi = szobakApi;
            _vendegekApi = vendegekApi;
            _foglalasokApi = foglalasokApi;
            _authSession = authSession;
        }

        public SzobaDto? Szoba { get; set; }

        public string FoglaltIdoszakokJson { get; set; } = "[]";

        [TempData]
        public string? SuccessMessage { get; set; }

        [TempData]
        public string? PopupTitle { get; set; }

        [BindProperty]
        public string Nev { get; set; } = string.Empty;

        [BindProperty]
        public string SzemelyiIgazolvanySzam { get; set; } = string.Empty;

        [BindProperty]
        public int IranyitoSzam { get; set; }

        [BindProperty]
        public string Varos { get; set; } = string.Empty;

        [BindProperty]
        public string Utca { get; set; } = string.Empty;

        [BindProperty]
        public string Hazszam { get; set; } = string.Empty;

        [BindProperty]
        public DateTime Mettol { get; set; } = DateTime.Today;

        [BindProperty]
        public DateTime Meddig { get; set; } = DateTime.Today.AddDays(1);

        public async Task<IActionResult> OnGetAsync(int szobaId)
        {
            Szoba = await _szobakApi.GetByIdAsync(szobaId);

            if (Szoba == null)
                return NotFound();

            if (!_authSession.IsSignedIn)
                return RedirectToPage("/Account/Login");

            await LoadFoglaltIdoszakokAsync(szobaId);

            var vendeg = await _vendegekApi.GetCurrentAsync();

            if (vendeg != null)
            {
                Nev = vendeg.Nev;
                SzemelyiIgazolvanySzam = vendeg.SzemelyiIgazolvanySzam;
                IranyitoSzam = vendeg.IranyitoSzam;
                Varos = vendeg.Varos;
                Utca = vendeg.Utca;
                Hazszam = vendeg.Hazszam;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int szobaId)
        {
            Szoba = await _szobakApi.GetByIdAsync(szobaId);

            if (Szoba == null)
                return NotFound();

            if (!_authSession.IsSignedIn)
                return RedirectToPage("/Account/Login");

            await LoadFoglaltIdoszakokAsync(szobaId);

            if (!ModelState.IsValid)
                return Page();

            var vendeg = await _vendegekApi.GetCurrentAsync();
            if (vendeg == null)
            {
                ModelState.AddModelError(string.Empty, "Nem található a bejelentkezett felhasználóhoz tartozó vendégadat.");
                return Page();
            }

            vendeg.Nev = Nev;
            vendeg.SzemelyiIgazolvanySzam = SzemelyiIgazolvanySzam;
            vendeg.IranyitoSzam = IranyitoSzam;
            vendeg.Varos = Varos;
            vendeg.Utca = Utca;
            vendeg.Hazszam = Hazszam;

            try
            {
                await _vendegekApi.UpdateOwnAsync(vendeg);

                var result = await _foglalasokApi.CreateAsync(new FoglalasDto
                {
                    SzobaId = szobaId,
                    Mettol = Mettol,
                    Meddig = Meddig
                });

                PopupTitle = "Sikeres foglalás";
                SuccessMessage =
                    "Köszönjük a foglalását!\n\n" +
                    $"Fizetendõ összeg: {result.FizetendoOsszeg:N0} Ft\n" +
                    "Fizetés módja: személyesen a recepción\n" +
                    $"Foglalási azonosító: #{result.Id}";

                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

        private async Task LoadFoglaltIdoszakokAsync(int szobaId)
        {
            var ranges = await _foglalasokApi.GetBlockedRangesAsync(szobaId);
            FoglaltIdoszakokJson = JsonSerializer.Serialize(ranges);
        }
    }
}