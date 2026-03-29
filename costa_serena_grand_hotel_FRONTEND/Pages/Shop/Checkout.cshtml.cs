using System.ComponentModel.DataAnnotations;
using costa_serena_grand_hotel_FRONTEND.Dtos;
using costa_serena_grand_hotel_FRONTEND.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace costa_serena_grand_hotel_FRONTEND.Pages.Shop
{
    public class CheckoutModel : PageModel
    {
        private readonly CartService _cartService;
        private readonly VendegekApi _vendegekApi;
        private readonly RendelesekApi _rendelesekApi;
        private readonly AuthSession _authSession;

        public CheckoutModel(
            CartService cartService,
            VendegekApi vendegekApi,
            RendelesekApi rendelesekApi,
            AuthSession authSession)
        {
            _cartService = cartService;
            _vendegekApi = vendegekApi;
            _rendelesekApi = rendelesekApi;
            _authSession = authSession;
        }

        public List<CartItemDto> Items { get; set; } = new();

        public int Vegosszeg => Items.Sum(x => x.Osszeg);

        [BindProperty]
        [Required(ErrorMessage = "A név megadása kötelezõ.")]
        public string Nev { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "A személyi igazolvány szám megadása kötelezõ.")]
        public string SzemelyiIgazolvanySzam { get; set; } = string.Empty;

        [BindProperty]
        [Range(1000, 9999, ErrorMessage = "Adj meg egy érvényes irányítószámot.")]
        public int IranyitoSzam { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "A város megadása kötelezõ.")]
        public string Varos { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Az utca megadása kötelezõ.")]
        public string Utca { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "A házszám megadása kötelezõ.")]
        public string Hazszam { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync()
        {
            if (!_authSession.IsSignedIn)
                return RedirectToPage("/Account/Login");

            Items = _cartService.GetCart();

            if (!Items.Any())
                return RedirectToPage("/Shop/Cart");

            var vendeg = await _vendegekApi.GetCurrentAsync();

            if (vendeg != null)
            {
                Nev = vendeg.Nev;
                SzemelyiIgazolvanySzam = vendeg.SzemelyiIgazolvanySzam;
                IranyitoSzam = vendeg.IranyitoSzam;
                Varos = vendeg.Varos;
                Utca = vendeg.Utca;
                Hazszam = vendeg.Hazszam;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!_authSession.IsSignedIn)
                return RedirectToPage("/Account/Login");

            Items = _cartService.GetCart();

            if (!Items.Any())
            {
                ModelState.AddModelError(string.Empty, "A kosár üres.");
                return Page();
            }

            if (!ModelState.IsValid)
                return Page();

            var dto = new CreateOrderDto
            {
                Nev = Nev.Trim(),
                SzemelyiIgazolvanySzam = SzemelyiIgazolvanySzam.Trim(),
                IranyitoSzam = IranyitoSzam,
                Varos = Varos.Trim(),
                Utca = Utca.Trim(),
                Hazszam = Hazszam.Trim(),
                Tetelek = Items.Select(i => new CreateOrderTetelDto
                {
                    TermekId = i.TermekId,
                    Mennyiseg = i.Mennyiseg
                }).ToList()
            };

            try
            {
                var result = await _rendelesekApi.CreateAsync(dto);

                _cartService.Clear();

                TempData["PopupTitle"] = "Sikeres rendelés";
                TempData["SuccessMessage"] =
                    $"Köszönjük a rendelést! A végösszeg: {result.Vegosszeg:N0} Ft. Rendelés azonosító: #{result.Id}.";

                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}