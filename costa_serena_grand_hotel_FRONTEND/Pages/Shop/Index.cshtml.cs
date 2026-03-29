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

            _cartService.AddItem(termek, 1);
            SuccessMessage = "A termék bekerült a kosárba.";

            return RedirectToPage();
        }
    }
}