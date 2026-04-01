using costa_serena_grand_hotel_FRONTEND.Dtos;
using costa_serena_grand_hotel_FRONTEND.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace costa_serena_grand_hotel_FRONTEND.Pages.Admin.Rendelesek
{
    public class CreateModel : PageModel
    {
        private readonly RendelesekApi _api;
        private readonly VendegekApi _vendegekApi;
        private readonly AuthSession _authSession;

        public CreateModel(RendelesekApi api, VendegekApi vendegekApi, AuthSession authSession)
        {
            _api = api;
            _vendegekApi = vendegekApi;
            _authSession = authSession;
        }

        [BindProperty]
        public RendelesAdminDto Rendeles { get; set; } = new();

        public List<VendegDto> Vendegek { get; set; } = new();
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!_authSession.IsInRole("Admin"))
                return RedirectToPage("/Errors/Forbidden");

            Vendegek = (await _vendegekApi.GetAllAsync())
                .OrderBy(x => x.Id)
                .ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!_authSession.IsInRole("Admin"))
                return RedirectToPage("/Errors/Forbidden");

            try
            {
                await _api.CreateAdminAsync(Rendeles);
                return RedirectToPage("/Admin/Rendelesek/Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                Vendegek = (await _vendegekApi.GetAllAsync())
                    .OrderBy(x => x.Id)
                    .ToList();
                return Page();
            }
        }
    }
}