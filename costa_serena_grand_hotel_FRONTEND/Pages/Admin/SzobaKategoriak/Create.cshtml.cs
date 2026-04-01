using System.Text.Json;
using costa_serena_grand_hotel_FRONTEND.Dtos;
using costa_serena_grand_hotel_FRONTEND.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace costa_serena_grand_hotel_FRONTEND.Pages.Admin.SzobaKategoriak
{
    public class CreateModel : PageModel
    {
        private readonly SzobaKategoriakApi _api;
        private readonly AuthSession _authSession;
        private readonly ImageStorageService _imageStorage;

        public CreateModel(SzobaKategoriakApi api, AuthSession authSession, ImageStorageService imageStorage)
        {
            _api = api;
            _authSession = authSession;
            _imageStorage = imageStorage;
        }

        [BindProperty]
        public SzobaKategoriaDto Kategoria { get; set; } = new();

        [BindProperty]
        public List<IFormFile> UploadedImages { get; set; } = new();

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

            try
            {
                var paths = await _imageStorage.SaveManyAsync(UploadedImages, "SzobaKategoriak");

                if (paths.Any())
                {
                    Kategoria.KepekJson = JsonSerializer.Serialize(paths);
                }

                await _api.CreateAsync(Kategoria);
                return RedirectToPage("/Admin/SzobaKategoriak/Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }
    }
}