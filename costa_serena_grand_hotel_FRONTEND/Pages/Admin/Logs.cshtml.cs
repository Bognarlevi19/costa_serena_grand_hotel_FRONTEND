using costa_serena_grand_hotel_API.AdminModels;
using costa_serena_grand_hotel_FRONTEND.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace costa_serena_grand_hotel_FRONTEND.Pages.Admin
{
    public class LogsModel : PageModel
    {
        private readonly AdminApi _adminApi;
        private readonly AuthSession _authSession;

        public LogsModel(AdminApi adminApi, AuthSession authSession)
        {
            _adminApi = adminApi;
            _authSession = authSession;
        }

        [BindProperty(SupportsGet = true)]
        public string? UserEmail { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? EntityType { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool? IsAuthFailure { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        public LogsPagedDto? Result { get; set; }
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!_authSession.IsInRole("Admin"))
                return RedirectToPage("/Errors/Forbidden");

            try
            {
                Result = await _adminApi.GetLogsAsync(UserEmail, EntityType, IsAuthFailure, PageNumber, 50);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Hiba történt: {ex.Message}";
            }

            return Page();
        }
    }
}