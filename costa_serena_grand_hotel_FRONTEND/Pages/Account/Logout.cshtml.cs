using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using costa_serena_grand_hotel_FRONTEND.Services;

namespace costa_serena_grand_hotel_FRONTEND.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly AuthSession _auth;
        public LogoutModel(AuthSession auth) => _auth = auth;
        public IActionResult OnPost()
        {
            _auth.Clear();
            return RedirectToPage("/Index");
        }
    }
}
