using costa_serena_grand_hotel_FRONTEND.Dtos;
using costa_serena_grand_hotel_FRONTEND.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace costa_serena_grand_hotel_FRONTEND.Pages.Admin.Foglalasok
{
    public class IndexModel : PageModel
    {
        private readonly FoglalasokApi _api;
        private readonly AuthSession _authSession;

        public IndexModel(FoglalasokApi api, AuthSession authSession)
        {
            _api = api;
            _authSession = authSession;
        }

        public List<FoglalasDto> Foglalasok { get; set; } = new();
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!_authSession.IsInRole("Admin"))
                return RedirectToPage("/Errors/Forbidden");

            try
            {
                Foglalasok = (await _api.GetAllAsync())
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

            await _api.DeleteAsync(id);
            return RedirectToPage();
        }
    }
}