using costa_serena_grand_hotel_FRONTEND.Dtos;
using costa_serena_grand_hotel_FRONTEND.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace costa_serena_grand_hotel_FRONTEND.Pages.Admin.Vendegek
{
    public class EditModel : PageModel
    {
        private readonly VendegekApi _api;
        private readonly AuthSession _authSession;

        public EditModel(VendegekApi api, AuthSession authSession)
        {
            _api = api;
            _authSession = authSession;
        }

        [BindProperty]
        public VendegDto Vendeg { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (!_authSession.IsInRole("Admin"))
                return RedirectToPage("/Errors/Forbidden");

            try
            {
                var item = await _api.GetByIdAsync(id);
                if (item == null)
                    return RedirectToPage("/Admin/Vendegek/Index");

                Vendeg = item;
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!_authSession.IsInRole("Admin"))
                return RedirectToPage("/Errors/Forbidden");

            try
            {
                await _api.UpdateAsync(Vendeg.Id, Vendeg);
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