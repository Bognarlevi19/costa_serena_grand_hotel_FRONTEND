using costa_serena_grand_hotel_FRONTEND.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace costa_serena_grand_hotel_FRONTEND.Pages
{
    public class ContactModel : PageModel
    {
        private readonly AuthSession _authSession;
        private readonly VendegekApi _vendegekApi;

        public ContactModel(AuthSession authSession, VendegekApi vendegekApi)
        {
            _authSession = authSession;
            _vendegekApi = vendegekApi;
        }

        public string PrefillName { get; set; } = string.Empty;
        public string PrefillEmail { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            if (!_authSession.IsSignedIn)
                return;

            PrefillEmail = _authSession.GetEmail() ?? string.Empty;

            if (!_authSession.IsInRole("User"))
                return;

            try
            {
                var vendeg = await _vendegekApi.GetCurrentAsync();
                if (vendeg != null)
                {
                    PrefillName = vendeg.Nev ?? string.Empty;
                }
            }
            catch
            {
                PrefillName = string.Empty;
            }
        }
    }
}