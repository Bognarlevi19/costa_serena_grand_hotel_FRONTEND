namespace costa_serena_grand_hotel_FRONTEND.Dtos
{
    public class SajatFoglalasDto
    {
        public int Id { get; set; }
        public int SzobaId { get; set; }
        public int SzobaSzam { get; set; }
        public string SzobaNev { get; set; } = string.Empty;
        public string? KategoriaNev { get; set; }
        public DateTime Mettol { get; set; }
        public DateTime Meddig { get; set; }
        public bool Fizetett { get; set; }
    }
}