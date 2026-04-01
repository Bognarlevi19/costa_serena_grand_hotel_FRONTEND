namespace costa_serena_grand_hotel_FRONTEND.Dtos
{
    public class RendelesAdminDto
    {
        public int Id { get; set; }
        public int VendegId { get; set; }
        public string Nev { get; set; } = string.Empty;
        public string SzemelyiIgazolvanySzam { get; set; } = string.Empty;
        public int IranyitoSzam { get; set; }
        public string Varos { get; set; } = string.Empty;
        public string Utca { get; set; } = string.Empty;
        public string Hazszam { get; set; } = string.Empty;
        public DateTime Letrehozva { get; set; }
        public int Vegosszeg { get; set; }
        public bool Fizetett { get; set; }
        public bool Elkuldve { get; set; }
        public int TetelDb { get; set; }
    }
}