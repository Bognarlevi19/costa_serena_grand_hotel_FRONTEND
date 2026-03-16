using costa_serena_grand_hotel_FRONTEND.Dtos;
using costa_serena_grand_hotel_FRONTEND.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace costa_serena_grand_hotel_FRONTEND.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly AuthApi _auth;
        public RegisterModel(AuthApi auth)
        {
            _auth = auth;
        }
        [BindProperty] public string Email { get; set; } = "";
        [BindProperty] public string Password { get; set; } = "";

        [BindProperty] public string SzemelyiIgazolvanySzam { get; set; } = "";
        [BindProperty] public string Nev { get; set; } = "";
        [BindProperty] public int IranyitoSzam { get; set; }
        [BindProperty] public string Varos { get; set; } = "";
        [BindProperty] public string Utca { get; set; } = "";
        [BindProperty] public string Hazszam { get; set; } = "";

        public string? Error { get; set; }
        public void OnGet() { }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var dto = new RegisterDto
                {
                    Nev = Nev,
                    SzemelyiIgazolvanySzam = SzemelyiIgazolvanySzam,
                    IranyitoSzam = IranyitoSzam,
                    Varos = Varos,
                    Utca = Utca,
                    Hazszam = Hazszam,
                    Email = Email,
                    Password = Password
                };

                await _auth.RegisterAsync(dto);

                return RedirectToPage("/Account/Login");
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return Page();
            }
        }
    }
}
