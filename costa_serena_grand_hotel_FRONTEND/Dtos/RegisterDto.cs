namespace costa_serena_grand_hotel_FRONTEND.Dtos
{
    public class RegisterDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public string Nev { get; set; } = string.Empty;
        public string SzemelyiIgazolvanySzam { get; set; } = string.Empty;
        public int IranyitoSzam { get; set; }
        public string Varos { get; set; } = string.Empty;
        public string Utca { get; set; } = string.Empty;
        public string Hazszam { get; set; } = string.Empty;
    }
}
