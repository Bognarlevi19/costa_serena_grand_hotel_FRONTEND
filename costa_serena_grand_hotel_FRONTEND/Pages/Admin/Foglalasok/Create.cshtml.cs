using costa_serena_grand_hotel_FRONTEND.Dtos;
using costa_serena_grand_hotel_FRONTEND.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace costa_serena_grand_hotel_FRONTEND.Pages.Admin.Foglalasok
{
    public class CreateModel : PageModel
    {
        private readonly FoglalasokApi _api;
        private readonly SzobakApi _szobakApi;
        private readonly VendegekApi _vendegekApi;
        private readonly AuthSession _authSession;

        public CreateModel(
            FoglalasokApi api,
            SzobakApi szobakApi,
            VendegekApi vendegekApi,
            AuthSession authSession)
        {
            _api = api;
            _szobakApi = szobakApi;
            _vendegekApi = vendegekApi;
            _authSession = authSession;
        }

        [BindProperty]
        public FoglalasDto Foglalas { get; set; } = new();

        public List<SzobaDto> Szobak { get; set; } = new();
        public List<VendegDto> Vendegek { get; set; } = new();
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!_authSession.IsInRole("Admin"))
                return RedirectToPage("/Errors/Forbidden");

            Foglalas.Mettol = DateTime.Today;
            Foglalas.Meddig = DateTime.Today.AddDays(1);

            await LoadAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!_authSession.IsInRole("Admin"))
                return RedirectToPage("/Errors/Forbidden");

            try
            {
                await _api.CreateAdminAsync(Foglalas);
                return RedirectToPage("/Admin/Foglalasok/Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                await LoadAsync();
                return Page();
            }
        }

        private async Task LoadAsync()
        {
            Szobak = (await _szobakApi.GetAllAsync())
                .OrderBy(x => x.Id)
                .ToList();

            Vendegek = (await _vendegekApi.GetAllAsync())
                .OrderBy(x => x.Id)
                .ToList();
        }
    }
}