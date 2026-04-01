using costa_serena_grand_hotel_FRONTEND.Dtos;
using costa_serena_grand_hotel_FRONTEND.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace costa_serena_grand_hotel_FRONTEND.Pages.Admin.Termekek
{
    public class IndexModel : PageModel
    {
        private readonly TermekekApi _api;
        private readonly AuthSession _authSession;

        public IndexModel(TermekekApi api, AuthSession authSession)
        {
            _api = api;
            _authSession = authSession;
        }

        public List<TermekDto> Termekek { get; set; } = new();
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!_authSession.IsInRole("Admin"))
                return RedirectToPage("/Errors/Forbidden");

            try
            {
                Termekek = (await _api.GetAllAsync(admin: true))
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
                TempData["AdminError"] = ex.Message;
            }

            return RedirectToPage();
        }
    }
}