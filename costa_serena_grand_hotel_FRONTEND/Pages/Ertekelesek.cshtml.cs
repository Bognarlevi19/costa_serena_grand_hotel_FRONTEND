using costa_serena_grand_hotel_FRONTEND.Dtos;
using costa_serena_grand_hotel_FRONTEND.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace costa_serena_grand_hotel_FRONTEND.Pages
{
    public class ErtekelesekModel : PageModel
    {
        private readonly ErtekelesekApi _ertekelesekApi;
        private readonly AuthSession _authSession;

        public ErtekelesekModel(ErtekelesekApi ertekelesekApi, AuthSession authSession)
        {
            _ertekelesekApi = ertekelesekApi;
            _authSession = authSession;
        }

        public List<ErtekelesDto> Reviews { get; set; } = new();

        public double AverageRating => Reviews.Count == 0 ? 0 : Reviews.Average(r => r.Rating);

        [BindProperty]
        public ReviewInput Input { get; set; } = new();

        public async Task OnGetAsync()
        {
            Reviews = await _ertekelesekApi.GetAllAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!_authSession.IsSignedIn)
            {
                TempData["ReviewError"] = "Értékelést csak bejelentkezett felhasználó írhat.";
                return RedirectToPage("/Account/Login");
            }

            if (!ModelState.IsValid)
            {
                Reviews = await _ertekelesekApi.GetAllAsync();
                return Page();
            }

            await _ertekelesekApi.CreateAsync(
                Input.Rating,
                Input.Comment
            );

            TempData["ReviewOk"] = "Köszönjük! Az értékelése sikeresen mentésre került.";
            return RedirectToPage();
        }

        public string GetInitials(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "?";

            var parts = name
                .Trim()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 1)
                return parts[0].Substring(0, 1).ToUpperInvariant();

            var first = parts[0].Substring(0, 1).ToUpperInvariant();
            var last = parts[^1].Substring(0, 1).ToUpperInvariant();
            return first + last;
        }

        public class ReviewInput
        {
            [Range(1, 5, ErrorMessage = "Válasszon 1 és 5 csillag között.")]
            public int Rating { get; set; } = 5;

            [Required(ErrorMessage = "A vélemény megadása kötelezõ.")]
            [StringLength(600, ErrorMessage = "A vélemény maximum 600 karakter lehet.")]
            public string Comment { get; set; } = "";
        }
    }
}