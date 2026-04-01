using costa_serena_grand_hotel_API.AdminModels;
using costa_serena_grand_hotel_FRONTEND.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace costa_serena_grand_hotel_FRONTEND.Pages.Admin
{
    public class UsersModel : PageModel
    {
        private readonly AdminApi _adminApi;
        private readonly AuthSession _authSession;

        public UsersModel(AdminApi adminApi, AuthSession authSession)
        {
            _adminApi = adminApi;
            _authSession = authSession;
        }

        public List<UserActivityDto> Users { get; set; } = new();
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!_authSession.IsInRole("Admin"))
                return RedirectToPage("/Errors/Forbidden");

            try
            {
                Users = await _adminApi.GetUsersAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Hiba történt: {ex.Message}";
            }

            return Page();
        }
    }
}