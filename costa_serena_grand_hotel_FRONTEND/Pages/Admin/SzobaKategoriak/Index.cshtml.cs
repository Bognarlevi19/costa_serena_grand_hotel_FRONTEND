using costa_serena_grand_hotel_FRONTEND.Dtos;
using costa_serena_grand_hotel_FRONTEND.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace costa_serena_grand_hotel_FRONTEND.Pages.Admin.SzobaKategoriak
{
    public class IndexModel : PageModel
    {
        private readonly SzobaKategoriakApi _api;
        private readonly AuthSession _authSession;

        public IndexModel(SzobaKategoriakApi api, AuthSession authSession)
        {
            _api = api;
            _authSession = authSession;
        }

        public List<SzobaKategoriaDto> Kategoriak { get; set; } = new();
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!_authSession.IsInRole("Admin"))
                return RedirectToPage("/Errors/Forbidden");

            try
            {
                Kategoriak = (await _api.GetAllAsync())
                    .OrderBy(x => x.Id)
                    .ToList();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            if (!_authSession.IsInRole("Admin"))
                return RedirectToPage("/Errors/Forbidden");

            try
            {
                await _api.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                Kategoriak = (await _api.GetAllAsync())
                    .OrderBy(x => x.Id)
                    .ToList();
                return Page();
            }

            return RedirectToPage();
        }
    }
}