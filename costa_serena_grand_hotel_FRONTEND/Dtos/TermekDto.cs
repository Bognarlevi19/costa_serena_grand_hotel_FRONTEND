namespace costa_serena_grand_hotel_FRONTEND.Dtos
{
    public class TermekDto
    {
        public int Id { get; set; }
        public string Nev { get; set; } = string.Empty;
        public string? Leiras { get; set; }
        public int Ar { get; set; }
        public string? KepUrl { get; set; }
        public string? Kategoria { get; set; }
        public int Darabszam { get; set; }
    }
}