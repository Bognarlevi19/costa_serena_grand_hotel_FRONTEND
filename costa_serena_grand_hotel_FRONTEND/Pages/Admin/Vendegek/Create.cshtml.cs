using costa_serena_grand_hotel_FRONTEND.Dtos;
using costa_serena_grand_hotel_FRONTEND.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace costa_serena_grand_hotel_FRONTEND.Pages.Admin.Vendegek
{
    public class CreateModel : PageModel
    {
        private readonly VendegekApi _api;
        private readonly AuthSession _authSession;

        public CreateModel(VendegekApi api, AuthSession authSession)
        {
            _api = api;
            _authSession = authSession;
        }

        [BindProperty]
        public VendegDto Vendeg { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public IActionResult OnGet()
        {
            if (!_authSession.IsInRole("Admin"))
                return RedirectToPage("/Errors/Forbidden");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!_authSession.IsInRole("Admin"))
                return RedirectToPage("/Errors/Forbidden");

            try
            {
                await _api.CreateAsync(Vendeg);
                return RedirectToPage("/Admin/Vendegek/Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }
    }
}