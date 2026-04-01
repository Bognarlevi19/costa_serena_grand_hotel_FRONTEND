using costa_serena_grand_hotel_API.AdminModels;
using costa_serena_grand_hotel_FRONTEND.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace costa_serena_grand_hotel_FRONTEND.Pages.Admin
{
    public class IpStatsModel : PageModel
    {
        private readonly AdminApi _adminApi;
        private readonly AuthSession _authSession;

        public IpStatsModel(AdminApi adminApi, AuthSession authSession)
        {
            _adminApi = adminApi;
            _authSession = authSession;
        }

        [BindProperty(SupportsGet = true)]
        public int Days { get; set; } = 7;

        public List<IpStatsDto> Stats { get; set; } = new();
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!_authSession.IsInRole("Admin"))
                return RedirectToPage("/Errors/Forbidden");

            try
            {
                Stats = await _adminApi.GetIpStatsAsync(Days);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Hiba történt: {ex.Message}";
            }

            return Page();
        }
    }
}