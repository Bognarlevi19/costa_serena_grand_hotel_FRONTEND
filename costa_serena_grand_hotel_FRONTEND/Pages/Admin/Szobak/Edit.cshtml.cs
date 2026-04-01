using costa_serena_grand_hotel_FRONTEND.Dtos;
using costa_serena_grand_hotel_FRONTEND.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace costa_serena_grand_hotel_FRONTEND.Pages.Admin.Szobak
{
    public class EditModel : PageModel
    {
        private readonly SzobakApi _szobakApi;
        private readonly SzobaKategoriakApi _szobaKategoriakApi;
        private readonly AuthSession _authSession;
        private readonly IWebHostEnvironment _environment;

        public EditModel(
            SzobakApi szobakApi,
            SzobaKategoriakApi szobaKategoriakApi,
            AuthSession authSession,
            IWebHostEnvironment environment)
        {
            _szobakApi = szobakApi;
            _szobaKategoriakApi = szobaKategoriakApi;
            _authSession = authSession;
            _environment = environment;
        }

        [BindProperty]
        public SzobaDto Szoba { get; set; } = new();

        public List<SzobaKategoriaDto> Kategoriak { get; set; } = new();

        public List<string> Kepek { get; set; } = new();

        [TempData]
        public string? SuccessMessage { get; set; }

        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (!_authSession.IsInRole("Admin"))
                return RedirectToPage("/Errors/Forbidden");

            await LoadKategoriakAsync();

            var szoba = await _szobakApi.GetByIdAsync(id);
            if (szoba == null)
                return RedirectToPage("/Admin/Szobak/Index");

            Szoba = szoba;
            Kepek = GetKepek(Szoba.KepekJson);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(List<IFormFile>? UploadedImages)
        {
            if (!_authSession.IsInRole("Admin"))
                return RedirectToPage("/Errors/Forbidden");

            await LoadKategoriakAsync();

            if (Szoba.Id <= 0)
            {
                ErrorMessage = "Érvénytelen szobaazonosító.";
                Kepek = GetKepek(Szoba.KepekJson);
                return Page();
            }

            var eredeti = await _szobakApi.GetByIdAsync(Szoba.Id);
            if (eredeti == null)
            {
                ErrorMessage = "A szoba nem található.";
                Kepek = GetKepek(Szoba.KepekJson);
                return Page();
            }

            try
            {
                var aktualisKepek = GetKepek(eredeti.KepekJson);

                if (UploadedImages != null && UploadedImages.Any(f => f.Length > 0))
                {
                    var ujKepek = await SaveUploadedImagesAsync(UploadedImages, "kepek/Szobak");
                    aktualisKepek.AddRange(ujKepek);
                }

                Szoba.KepekJson = JsonSerializer.Serialize(aktualisKepek);

                await _szobakApi.UpdateAsync(Szoba.Id, Szoba);

                SuccessMessage = "A szoba adatai sikeresen frissültek.";
                return RedirectToPage("/Admin/Szobak/Edit", new { id = Szoba.Id });
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Mentési hiba: {ex.Message}";
                Kepek = GetKepek(Szoba.KepekJson);
                return Page();
            }
        }

        public List<string> GetKepek(string? kepekJson)
        {
            if (string.IsNullOrWhiteSpace(kepekJson))
                return new List<string>();

            try
            {
                return JsonSerializer.Deserialize<List<string>>(kepekJson) ?? new List<string>();
            }
            catch
            {
                return new List<string>();
            }
        }

        private async Task LoadKategoriakAsync()
        {
            Kategoriak = await _szobaKategoriakApi.GetAllAsync();
        }

        private async Task<List<string>> SaveUploadedImagesAsync(List<IFormFile> files, string relativeFolder)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var savedPaths = new List<string>();

            var physicalFolder = Path.Combine(_environment.WebRootPath, relativeFolder.Replace('/', Path.DirectorySeparatorChar));
            Directory.CreateDirectory(physicalFolder);

            foreach (var file in files)
            {
                if (file == null || file.Length == 0)
                    continue;

                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(extension))
                    throw new Exception($"Nem támogatott fájltípus: {file.FileName}");

                if (file.Length > 10 * 1024 * 1024)
                    throw new Exception($"A fájl túl nagy: {file.FileName}. Maximum 10 MB engedélyezett.");

                var safeFileName = $"{Guid.NewGuid():N}{extension}";
                var fullPath = Path.Combine(physicalFolder, safeFileName);

                await using var stream = new FileStream(fullPath, FileMode.Create);
                await file.CopyToAsync(stream);

                var virtualPath = "/" + relativeFolder.Trim('/').Replace("\\", "/") + "/" + safeFileName;
                savedPaths.Add(virtualPath);
            }

            return savedPaths;
        }
    }
}