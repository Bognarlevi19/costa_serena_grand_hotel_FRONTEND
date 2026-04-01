using costa_serena_grand_hotel_FRONTEND.Dtos;
using costa_serena_grand_hotel_FRONTEND.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace costa_serena_grand_hotel_FRONTEND.Pages.Admin.Rendelesek
{
    public class EditModel : PageModel
    {
        private readonly RendelesekApi _api;
        private readonly AuthSession _authSession;

        public EditModel(RendelesekApi api, AuthSession authSession)
        {
            _api = api;
            _authSession = authSession;
        }

        [BindProperty]
        public RendelesAdminDto Rendeles { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (!_authSession.IsInRole("Admin"))
                return RedirectToPage("/Errors/Forbidden");

            try
            {
                var item = await _api.GetByIdAsync(id);
                if (item == null)
                    return RedirectToPage("/Admin/Rendelesek/Index");

                Rendeles = item;
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
                await _api.UpdateAsync(Rendeles.Id, Rendeles);
                return RedirectToPage("/Admin/Rendelesek/Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }
    }
}