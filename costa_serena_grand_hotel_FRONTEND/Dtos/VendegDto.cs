using System.ComponentModel.DataAnnotations;

namespace costa_serena_grand_hotel_FRONTEND.Dtos
{
    public class VendegDto
    {
        public int Id { get; set; }

        public string SzemelyiIgazolvanySzam { get; set; } = String.Empty;

        public string Nev { get; set; } = String.Empty;

        public int IranyitoSzam { get; set; }

        public string Varos { get; set; } = String.Empty;

        public string Utca { get; set; } = String.Empty;

        public string Hazszam { get; set; } = String.Empty;
    }
}
