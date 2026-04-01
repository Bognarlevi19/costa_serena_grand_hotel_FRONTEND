using costa_serena_grand_hotel_API.AdminModels;
using costa_serena_grand_hotel_FRONTEND.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace costa_serena_grand_hotel_FRONTEND.Pages.Admin
{
    public class UserDetailsModel : PageModel
    {
        private readonly AdminApi _adminApi;
        private readonly AuthSession _authSession;

        public UserDetailsModel(AdminApi adminApi, AuthSession authSession)
        {
            _adminApi = adminApi;
            _authSession = authSession;
        }

        public UserDetailsDto? UserDetails { get; set; }
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (!_authSession.IsInRole("Admin"))
                return RedirectToPage("/Errors/Forbidden");

            if (string.IsNullOrWhiteSpace(id))
                return RedirectToPage("/Admin/Users");

            try
            {
                UserDetails = await _adminApi.GetUserDetailsAsync(id);

                if (UserDetails == null)
                    ErrorMessage = "A felhasználó nem található.";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Hiba történt: {ex.Message}";
            }

            return Page();
        }
    }
}