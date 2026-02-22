using System.ComponentModel.DataAnnotations;

namespace costa_serena_grand_hotel_FRONTEND.Dtos
{
    public class SzobaDto
    {
        public int Id { get; set; }

        public string Szam { get; set; } = String.Empty;

        public int Emelet { get; set; }

        public double Alapterulet { get; set; }

        public int Ar { get; set; }
    }
}
