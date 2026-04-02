using costa_serena_grand_hotel_FRONTEND.Dtos;
using costa_serena_grand_hotel_FRONTEND.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace costa_serena_grand_hotel_FRONTEND.Pages.Shop
{
    public class CartModel : PageModel
    {
        private readonly CartService _cartService;
        private readonly AuthSession _authSession;
        private readonly TermekekApi _termekekApi;

        public CartModel(CartService cartService, AuthSession authSession, TermekekApi termekekApi)
        {
            _cartService = cartService;
            _authSession = authSession;
            _termekekApi = termekekApi;
        }

        public List<CartItemDto> Items { get; set; } = new();

        public int Vegosszeg => Items.Sum(x => x.Osszeg);

        [TempData]
        public string? ErrorMessage { get; set; }

        public void OnGet()
        {
            Items = _cartService.GetCart();
        }

        public IActionResult OnPostRemove(int termekId)
        {
            _cartService.RemoveItem(termekId);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostPlusAsync(int termekId)
        {
            var item = _cartService.GetCart().FirstOrDefault(x => x.TermekId == termekId);
            if (item == null)
                return RedirectToPage();

            var termek = await _termekekApi.GetByIdAsync(termekId);
            if (termek == null)
            {
                ErrorMessage = "A termék már nem elérhetõ.";
                return RedirectToPage();
            }

            if (item.Mennyiseg >= termek.Darabszam)
            {
                ErrorMessage = "Nem növelhetõ tovább a mennyiség, mert nincs több készleten.";
                return RedirectToPage();
            }

            _cartService.ChangeQuantity(termekId, item.Mennyiseg + 1);
            return RedirectToPage();
        }

        public IActionResult OnPostMinus(int termekId)
        {
            var item = _cartService.GetCart().FirstOrDefault(x => x.TermekId == termekId);
            if (item != null)
                _cartService.ChangeQuantity(termekId, item.Mennyiseg - 1);

            return RedirectToPage();
        }

        public IActionResult OnPostCheckout()
        {
            if (!_authSession.IsSignedIn)
                return RedirectToPage("/Account/Login");

            return RedirectToPage("/Shop/Checkout");
        }
    }
}