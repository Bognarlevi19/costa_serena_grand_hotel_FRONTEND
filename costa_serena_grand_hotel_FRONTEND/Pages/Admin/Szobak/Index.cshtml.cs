using costa_serena_grand_hotel_FRONTEND.Dtos;
using costa_serena_grand_hotel_FRONTEND.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace costa_serena_grand_hotel_FRONTEND.Pages.Admin.Szobak
{
    public class IndexModel : PageModel
    {
        private readonly SzobakApi _api;
        private readonly AuthSession _authSession;

        public IndexModel(SzobakApi api, AuthSession authSession)
        {
            _api = api;
            _authSession = authSession;
        }

        public List<SzobaDto> Szobak { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            if (!_authSession.IsInRole("Admin"))
                return RedirectToPage("/Errors/Forbidden");

            Szobak = (await _api.GetAllAsync())
            .OrderBy(x => x.Id)
            .ToList();      
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _api.DeleteAsync(id);
            return RedirectToPage();
        }
    }
}