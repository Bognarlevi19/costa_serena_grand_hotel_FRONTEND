using costa_serena_grand_hotel_API.AdminModels;
using costa_serena_grand_hotel_FRONTEND.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace costa_serena_grand_hotel_FRONTEND.Pages.Admin
{
    public class IndexModel : PageModel
    {
        private readonly AuthSession _authSession;
        private readonly AdminApi _adminApi;

        public IndexModel(AuthSession authSession, AdminApi adminApi)
        {
            _authSession = authSession;
            _adminApi = adminApi;
        }

        public AdminStatsDto? Stats { get; set; }
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!_authSession.IsInRole("Admin"))
                return RedirectToPage("/Errors/Forbidden");

            try
            {
                Stats = await _adminApi.GetStatsAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

            return Page();
        }
    }
}