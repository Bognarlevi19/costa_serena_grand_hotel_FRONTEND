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
        private readonly AuthSession _authSession;

        public UjModel(SzobakApi szobakApi, VendegekApi vendegekApi, AuthSession authSession)
        {
            _szobakApi = szobakApi;
            _vendegekApi = vendegekApi;
            _authSession = authSession;
        }

        public SzobaDto? Szoba { get; set; }

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

            if (_authSession.IsSignedIn)
            {
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
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int szobaId)
        {
            Szoba = await _szobakApi.GetByIdAsync(szobaId);

            if (Szoba == null)
                return NotFound();

            return Page();
        }
    }
}