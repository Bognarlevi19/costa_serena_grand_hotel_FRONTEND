using costa_serena_grand_hotel_FRONTEND.Dtos;
using costa_serena_grand_hotel_FRONTEND.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace costa_serena_grand_hotel_FRONTEND.Pages.Admin.Termekek
{
    public class CreateModel : PageModel
    {
        private readonly TermekekApi _api;
        private readonly AuthSession _authSession;
        private readonly ImageStorageService _imageStorage;

        public CreateModel(TermekekApi api, AuthSession authSession, ImageStorageService imageStorage)
        {
            _api = api;
            _authSession = authSession;
            _imageStorage = imageStorage;
        }

        [BindProperty]
        public TermekDto Termek { get; set; } = new() { Darabszam = 0 };

        [BindProperty]
        public IFormFile? UploadedImage { get; set; }

        public string? ErrorMessage { get; set; }

        public IActionResult OnGet()
        {
            if (!_authSession.IsInRole("Admin"))
                return RedirectToPage("/Errors/Forbidden");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!_authSession.IsInRole("Admin"))
                return RedirectToPage("/Errors/Forbidden");

            if (Termek.Darabszam < 0)
            {
                ErrorMessage = "A darabszám nem lehet negatív.";
                return Page();
            }

            try
            {
                var path = await _imageStorage.SaveSingleAsync(UploadedImage, "Termekek");
                if (!string.IsNullOrWhiteSpace(path))
                    Termek.KepUrl = path;

                await _api.CreateAsync(Termek);
                return RedirectToPage("/Admin/Termekek/Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }
    }
}