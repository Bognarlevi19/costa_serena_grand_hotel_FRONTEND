using costa_serena_grand_hotel_FRONTEND.Dtos;
using costa_serena_grand_hotel_FRONTEND.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace costa_serena_grand_hotel_FRONTEND.Pages.Admin.Foglalasok
{
    public class EditModel : PageModel
    {
        private readonly FoglalasokApi _api;
        private readonly AuthSession _authSession;

        public EditModel(FoglalasokApi api, AuthSession authSession)
        {
            _api = api;
            _authSession = authSession;
        }

        [BindProperty]
        public FoglalasDto Foglalas { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (!_authSession.IsInRole("Admin"))
                return RedirectToPage("/Errors/Forbidden");

            var item = await _api.GetByIdAsync(id);
            if (item == null)
                return RedirectToPage("/Admin/Foglalasok/Index");

            Foglalas = item;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!_authSession.IsInRole("Admin"))
                return RedirectToPage("/Errors/Forbidden");

            await _api.UpdateAsync(Foglalas.Id, Foglalas);
            return RedirectToPage("/Admin/Foglalasok/Index");
        }
    }
}