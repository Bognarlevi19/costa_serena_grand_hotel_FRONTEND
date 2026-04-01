using System.Text.Json;
using costa_serena_grand_hotel_FRONTEND.Dtos;
using costa_serena_grand_hotel_FRONTEND.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace costa_serena_grand_hotel_FRONTEND.Pages.Admin.SzobaKategoriak
{
    public class EditModel : PageModel
    {
        private readonly SzobaKategoriakApi _api;
        private readonly AuthSession _authSession;
        private readonly ImageStorageService _imageStorage;

        public EditModel(SzobaKategoriakApi api, AuthSession authSession, ImageStorageService imageStorage)
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
        public List<string> CurrentImages { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (!_authSession.IsInRole("Admin"))
                return RedirectToPage("/Errors/Forbidden");

            var item = await _api.GetByIdAsync(id);
            if (item == null)
                return RedirectToPage("/Admin/SzobaKategoriak/Index");

            Kategoria = item;

            if (!string.IsNullOrWhiteSpace(Kategoria.KepekJson))
            {
                try
                {
                    CurrentImages = JsonSerializer.Deserialize<List<string>>(Kategoria.KepekJson) ?? new();
                }
                catch
                {
                    CurrentImages = new();
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!_authSession.IsInRole("Admin"))
                return RedirectToPage("/Errors/Forbidden");

            try
            {
                var currentImages = new List<string>();

                if (!string.IsNullOrWhiteSpace(Kategoria.KepekJson))
                {
                    try
                    {
                        currentImages = JsonSerializer.Deserialize<List<string>>(Kategoria.KepekJson) ?? new();
                    }
                    catch
                    {
                        currentImages = new();
                    }
                }

                var paths = await _imageStorage.SaveManyAsync(UploadedImages, "SzobaKategoriak");

                if (paths.Any())
                {
                    currentImages.AddRange(paths);
                    Kategoria.KepekJson = JsonSerializer.Serialize(currentImages);
                }

                await _api.UpdateAsync(Kategoria.Id, Kategoria);
                return RedirectToPage("/Admin/SzobaKategoriak/Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;

                if (!string.IsNullOrWhiteSpace(Kategoria.KepekJson))
                {
                    try
                    {
                        CurrentImages = JsonSerializer.Deserialize<List<string>>(Kategoria.KepekJson) ?? new();
                    }
                    catch
                    {
                        CurrentImages = new();
                    }
                }

                return Page();
            }
        }
    }
}