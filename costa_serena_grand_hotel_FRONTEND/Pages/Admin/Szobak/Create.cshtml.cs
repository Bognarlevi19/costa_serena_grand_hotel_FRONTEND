using System.Text.Json;
using costa_serena_grand_hotel_FRONTEND.Dtos;
using costa_serena_grand_hotel_FRONTEND.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace costa_serena_grand_hotel_FRONTEND.Pages.Admin.Szobak
{
    public class CreateModel : PageModel
    {
        private readonly SzobakApi _api;
        private readonly SzobaKategoriakApi _kategoriakApi;
        private readonly AuthSession _authSession;
        private readonly ImageStorageService _imageStorage;

        public CreateModel(
            SzobakApi api,
            SzobaKategoriakApi kategoriakApi,
            AuthSession authSession,
            ImageStorageService imageStorage)
        {
            _api = api;
            _kategoriakApi = kategoriakApi;
            _authSession = authSession;
            _imageStorage = imageStorage;
        }

        [BindProperty]
        public SzobaDto Szoba { get; set; } = new();

        [BindProperty]
        public List<IFormFile> UploadedImages { get; set; } = new();

        public List<SzobaKategoriaDto> Kategoriak { get; set; } = new();
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!_authSession.IsInRole("Admin"))
                return RedirectToPage("/Errors/Forbidden");

            Kategoriak = await _kategoriakApi.GetAllAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!_authSession.IsInRole("Admin"))
                return RedirectToPage("/Errors/Forbidden");

            try
            {
                var paths = await _imageStorage.SaveManyAsync(UploadedImages, "Szobak");

                if (paths.Any())
                    Szoba.KepekJson = JsonSerializer.Serialize(paths);

                await _api.CreateAsync(Szoba);
                return RedirectToPage("/Admin/Szobak/Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                Kategoriak = await _kategoriakApi.GetAllAsync();
                return Page();
            }
        }
    }
}