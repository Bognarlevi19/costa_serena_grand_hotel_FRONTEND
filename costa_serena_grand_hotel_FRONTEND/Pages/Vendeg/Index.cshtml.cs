using costa_serena_grand_hotel_FRONTEND.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace costa_serena_grand_hotel_FRONTEND.Pages.Vendeg
{
    public class IndexModel : PageModel
    {
        private readonly VendegekApi _vendegekApi;
        private readonly AuthSession _authSession;

        public IndexModel(VendegekApi vendegekApi, AuthSession authSession)
        {
            _vendegekApi = vendegekApi;
            _authSession = authSession;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!_authSession.IsSignedIn)
            {
                return RedirectToPage("/Account/Login", new { returnUrl = "/Vendeg" });
            }

            var sajatVendeg = await _vendegekApi.GetCurrentAsync();

            if (sajatVendeg == null)
            {
                TempData["ErrorMessage"] = "A saját adataid nem találhatók.";
                return RedirectToPage("/Index");
            }

            return RedirectToPage("/Vendeg/Details", new { id = sajatVendeg.Id });
        }
    }
}