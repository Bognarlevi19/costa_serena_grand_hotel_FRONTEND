using costa_serena_grand_hotel_FRONTEND.Dtos;
using costa_serena_grand_hotel_FRONTEND.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace costa_serena_grand_hotel_FRONTEND.Pages.Admin.Termekek
{
    public class EditModel : PageModel
    {
        private readonly TermekekApi _api;
        private readonly AuthSession _authSession;
        private readonly ImageStorageService _imageStorage;

        public EditModel(TermekekApi api, AuthSession authSession, ImageStorageService imageStorage)
        {
            _api = api;
            _authSession = authSession;
            _imageStorage = imageStorage;
        }

        [BindProperty]
        public TermekDto Termek { get; set; } = new();

        [BindProperty]
        public IFormFile? UploadedImage { get; set; }

        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (!_authSession.IsInRole("Admin"))
                return RedirectToPage("/Errors/Forbidden");

            var item = await _api.GetByIdAsync(id);
            if (item == null)
                return RedirectToPage("/Admin/Termekek/Index");

            Termek = item;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!_authSession.IsInRole("Admin"))
                return RedirectToPage("/Errors/Forbidden");

            try
            {
                var path = await _imageStorage.SaveSingleAsync(UploadedImage, "Termekek");
                if (!string.IsNullOrWhiteSpace(path))
                    Termek.KepUrl = path;

                await _api.UpdateAsync(Termek.Id, Termek);
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