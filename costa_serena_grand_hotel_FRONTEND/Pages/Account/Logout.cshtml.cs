using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using costa_serena_grand_hotel_FRONTEND.Services;

namespace costa_serena_grand_hotel_FRONTEND.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly AuthSession _auth;
        private readonly CartService _cartService;

        public LogoutModel(AuthSession auth, CartService cartService)
        {
            _auth = auth;
            _cartService = cartService;
        }

        public IActionResult OnPost()
        {
            _cartService.Clear();
            _auth.Clear();

            return RedirectToPage("/Index");
        }
    }
}