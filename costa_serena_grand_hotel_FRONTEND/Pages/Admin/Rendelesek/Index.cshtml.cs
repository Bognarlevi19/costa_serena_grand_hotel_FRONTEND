using costa_serena_grand_hotel_FRONTEND.Dtos;
using costa_serena_grand_hotel_FRONTEND.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace costa_serena_grand_hotel_FRONTEND.Pages.Admin.Rendelesek
{
    public class IndexModel : PageModel
    {
        private readonly RendelesekApi _api;
        private readonly AuthSession _authSession;

        public IndexModel(RendelesekApi api, AuthSession authSession)
        {
            _api = api;
            _authSession = authSession;
        }

        public List<RendelesAdminDto> Rendelesek { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            if (!_authSession.IsInRole("Admin"))
                return RedirectToPage("/Errors/Forbidden");

            Rendelesek = (await _api.GetAllAsync())
                .OrderBy(x => x.Id)
                .ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _api.DeleteAsync(id);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostToggleElkuldveAsync(int id, bool elkuldve)
        {
            await _api.SetElkuldveAsync(id, elkuldve);
            return RedirectToPage();
        }
    }
}