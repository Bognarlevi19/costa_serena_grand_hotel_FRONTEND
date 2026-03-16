namespace costa_serena_grand_hotel_FRONTEND.Dtos
{
    public class ErtekelesDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Nev { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string? IdentityUserId { get; set; }
    }
}
