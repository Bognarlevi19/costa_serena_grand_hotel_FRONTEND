using costa_serena_grand_hotel_FRONTEND.Dtos;
using costa_serena_grand_hotel_FRONTEND.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace costa_serena_grand_hotel_FRONTEND.Pages.Vendeg
{
    public class DetailsModel : PageModel
    {
        private readonly VendegekApi _vendegekApi;
        private readonly AuthSession _authSession;

        public DetailsModel(VendegekApi vendegekApi, AuthSession authSession)
        {
            _vendegekApi = vendegekApi;
            _authSession = authSession;
        }

        [BindProperty]
        public VendegDto Vendeg { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? SuccessMessage { get; set; }

        public string? ErrorMessage { get; set; }
        public string? BejelentkezettEmail { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (!_authSession.IsSignedIn)
            {
                return RedirectToPage("/Account/Login", new { returnUrl = $"/Vendeg/{id}" });
            }

            BejelentkezettEmail = _authSession.GetEmail();

            var sajatVendeg = await _vendegekApi.GetCurrentAsync();
            if (sajatVendeg == null)
            {
                ErrorMessage = "Nem sikerült betölteni a bejelentkezett felhasználó adatait.";
                return Page();
            }

            if (_authSession.IsInRole("Admin"))
            {
                var adminAltalMegnyitottVendeg = await _vendegekApi.GetByIdAsync(id);

                if (adminAltalMegnyitottVendeg == null)
                    return NotFound();

                Vendeg = adminAltalMegnyitottVendeg;
                return Page();
            }

            if (sajatVendeg.Id != id)
            {
                return Forbid();
            }

            Vendeg = sajatVendeg;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!_authSession.IsSignedIn)
            {
                return RedirectToPage("/Account/Login", new { returnUrl = $"/Vendeg/{id}" });
            }

            BejelentkezettEmail = _authSession.GetEmail();

            if (!ModelState.IsValid)
            {
                ErrorMessage = "Kérlek javítsd a hibás mezõket.";
                return Page();
            }

            try
            {
                var sajatVendeg = await _vendegekApi.GetCurrentAsync();
                if (sajatVendeg == null)
                {
                    ErrorMessage = "Nem sikerült betölteni a bejelentkezett felhasználó adatait.";
                    return Page();
                }

                if (_authSession.IsInRole("Admin"))
                {
                    Vendeg.Id = id;
                    await _vendegekApi.UpdateAsync(id, Vendeg);

                    return RedirectToPage("/Vendeg/Details", new
                    {
                        id,
                        successMessage = "A vendég adatai sikeresen frissítve lettek."
                    });
                }

                if (sajatVendeg.Id != id)
                {
                    return Forbid();
                }

                Vendeg.Id = sajatVendeg.Id;
                Vendeg.IdentityUserId = sajatVendeg.IdentityUserId;

                await _vendegekApi.UpdateOwnAsync(Vendeg);

                return RedirectToPage("/Vendeg/Details", new
                {
                    id = sajatVendeg.Id,
                    successMessage = "Az adatai sikeresen mentve lettek."
                });
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }
    }
}