namespace costa_serena_grand_hotel_FRONTEND.Dtos
{
    public class CartItemDto
    {
        public int TermekId { get; set; }
        public string Nev { get; set; } = string.Empty;
        public int Ar { get; set; }
        public string? KepUrl { get; set; }
        public int Mennyiseg { get; set; }

        public int Osszeg => Ar * Mennyiseg;
    }
}