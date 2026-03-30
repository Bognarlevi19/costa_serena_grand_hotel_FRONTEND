using System.ComponentModel.DataAnnotations;

namespace costa_serena_grand_hotel_FRONTEND.Dtos
{
    public class VendegDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A személyi igazolvány szám megadása kötelező.")]
        [StringLength(20, ErrorMessage = "A személyi igazolvány szám legfeljebb 20 karakter lehet.")]
        public string SzemelyiIgazolvanySzam { get; set; } = string.Empty;

        [Required(ErrorMessage = "A név megadása kötelező.")]
        [StringLength(100, ErrorMessage = "A név legfeljebb 100 karakter lehet.")]
        public string Nev { get; set; } = string.Empty;

        [Required(ErrorMessage = "Az irányítószám megadása kötelező.")]
        [Range(1000, 9999, ErrorMessage = "Az irányítószámnak 4 jegyű számnak kell lennie.")]
        public int IranyitoSzam { get; set; }

        [Required(ErrorMessage = "A város megadása kötelező.")]
        [StringLength(30, ErrorMessage = "A város neve legfeljebb 30 karakter lehet.")]
        public string Varos { get; set; } = string.Empty;

        [Required(ErrorMessage = "Az utca megadása kötelező.")]
        [StringLength(50, ErrorMessage = "Az utca neve legfeljebb 50 karakter lehet.")]
        public string Utca { get; set; } = string.Empty;

        [Required(ErrorMessage = "A házszám megadása kötelező.")]
        [StringLength(10, ErrorMessage = "A házszám legfeljebb 10 karakter lehet.")]
        public string Hazszam { get; set; } = string.Empty;

        public string? IdentityUserId { get; set; }
    }
}