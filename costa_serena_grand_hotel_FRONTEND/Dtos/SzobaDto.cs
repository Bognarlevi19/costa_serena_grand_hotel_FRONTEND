namespace costa_serena_grand_hotel_FRONTEND.Dtos
{
    public class SzobaDto
    {
        public int Id { get; set; }
        public string Szam { get; set; } = string.Empty;
        public int Emelet { get; set; }
        public double Alapterulet { get; set; }
        public int Ar { get; set; }
        public string Nev { get; set; } = string.Empty;
        public string? RovidLeiras { get; set; }
        public string? Leiras { get; set; }
        public int Ferohely { get; set; }
        public string? KepekJson { get; set; }
        public int SzobaKategoriaId { get; set; }
        public string? KategoriaNev { get; set; }
    }
}