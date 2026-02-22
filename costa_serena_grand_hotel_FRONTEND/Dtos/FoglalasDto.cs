namespace costa_serena_grand_hotel_FRONTEND.Dtos
{
    public class FoglalasDto
    {
        public int Id { get; set; }

        public int SzobaId { get; set; }
        public int VendegId { get; set; }


        public DateTime Mettol { get; set; }
        public DateTime Meddig { get; set; }
        public bool Fizetett { get; set; }
    }
}
