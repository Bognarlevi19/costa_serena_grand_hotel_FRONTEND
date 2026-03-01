using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace costa_serena_grand_hotel_FRONTEND.Pages
{
    public class ErtekelesekModel : PageModel
    {
        private static readonly List<Review> _reviews = new()
        {
            new Review { Name = "Nagy Dóra", Rating = 5, Comment = "Kifogástalan kiszolgálás, gyönyörû szoba és nagyon nyugodt spa részleg.", CreatedAt = DateTime.Now.AddDays(-6) },
            new Review { Name = "Kiss Márk", Rating = 4, Comment = "A reggeli brutál jó, a személyzet kedves. Esténként a lobby hangulata extra.", CreatedAt = DateTime.Now.AddDays(-3) },
            new Review { Name = "Szabó Petra", Rating = 5, Comment = "Tengerpart, tisztaság, csend — pont ezt kerestük. Biztosan visszajövünk.", CreatedAt = DateTime.Now.AddDays(-1) },
        };

        public List<Review> Reviews => _reviews;

        public double AverageRating => _reviews.Count == 0 ? 0 : _reviews.Average(r => r.Rating);

        [BindProperty]
        public ReviewInput Input { get; set; } = new();

        public void OnGet()
        {
            // ha kell, itt lehetne elõtöltés / szûrés / rendezés
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            _reviews.Add(new Review
            {
                Name = Input.Name.Trim(),
                Rating = Input.Rating,
                Comment = Input.Comment.Trim(),
                CreatedAt = DateTime.Now
            });

            TempData["ReviewOk"] = "Köszönjük! Az értékelésed sikeresen mentésre került.";
            return RedirectToPage(); // PRG (ne duplázódjon frissítéskor)
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

        public class Review
        {
            public string Name { get; set; } = "";
            public int Rating { get; set; }
            public string Comment { get; set; } = "";
            public DateTime CreatedAt { get; set; }
        }

        public class ReviewInput
        {
            [Required(ErrorMessage = "A név megadása kötelezõ.")]
            [StringLength(60, ErrorMessage = "A név maximum 60 karakter lehet.")]
            public string Name { get; set; } = "";

            [Range(1, 5, ErrorMessage = "Válassz 1 és 5 csillag között.")]
            public int Rating { get; set; } = 5;

            [Required(ErrorMessage = "A vélemény megadása kötelezõ.")]
            [StringLength(600, ErrorMessage = "A vélemény maximum 600 karakter lehet.")]
            public string Comment { get; set; } = "";
        }
    }
}
