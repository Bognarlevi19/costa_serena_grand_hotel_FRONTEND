namespace costa_serena_grand_hotel_FRONTEND.Dtos
{
    public class SzobaKategoriaDto
    {
        public int Id { get; set; }
        public string Nev { get; set; } = string.Empty;
        public string? Leiras { get; set; }
        public string? KepekJson { get; set; }
        public int Darab { get; set; }
    }
}