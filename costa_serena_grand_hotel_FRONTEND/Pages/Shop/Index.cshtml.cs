using costa_serena_grand_hotel_FRONTEND.Dtos;
using costa_serena_grand_hotel_FRONTEND.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace costa_serena_grand_hotel_FRONTEND.Pages.Shop
{
    public class IndexModel : PageModel
    {
        private readonly TermekekApi _termekekApi;
        private readonly CartService _cartService;

        public IndexModel(TermekekApi termekekApi, CartService cartService)
        {
            _termekekApi = termekekApi;
            _cartService = cartService;
        }

        public List<TermekDto> Termekek { get; set; } = new();
        public int CartCount { get; set; }

        [TempData]
        public string? SuccessMessage { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }

        public async Task OnGetAsync()
        {
            Termekek = await _termekekApi.GetAllAsync();
            CartCount = _cartService.GetCount();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(int termekId)
        {
            var termek = await _termekekApi.GetByIdAsync(termekId);

            if (termek == null)
                return NotFound();

            if (termek.Darabszam <= 0)
            {
                ErrorMessage = "A termék jelenleg nincs készleten.";
                return RedirectToPage();
            }

            var cartItem = _cartService.GetCart().FirstOrDefault(x => x.TermekId == termekId);
            var jelenlegiMennyiseg = cartItem?.Mennyiseg ?? 0;

            if (jelenlegiMennyiseg >= termek.Darabszam)
            {
                ErrorMessage = "Nem tehetsz többet a kosárba, mint amennyi készleten van.";
                return RedirectToPage();
            }

            _cartService.AddItem(termek, 1);
            SuccessMessage = "A termék bekerült a kosárba.";

            return RedirectToPage();
        }
    }
}