using Microsoft.AspNetCore.Mvc.RazorPages;

namespace costa_serena_grand_hotel_FRONTEND.Pages
{
    public class IndexModel : PageModel
    {
        public record Etelelemek(string cim, string hely, string tipus, string kep);

        public List<Etelelemek> Elemek { get; private set; } = new();

        public void OnGet()
        {
            Elemek = new List<Etelelemek>
            {
                new("Aranypart Étterem", "Tengerre nézõ fine dining", "ÉTTEREM", "https://images.unsplash.com/photo-1414235077428-338989a2e8c0?auto=format&fit=crop&w=1400&q=70"),
                new("Panoráma Terasz Bár", "Panoráma & Koktélok", "BÁR", "https://images.unsplash.com/photo-1525268323446-0505b6fe7778?auto=format&fit=crop&w=1400&q=70"),
                new("Holdfény Bár", "Zene • Koktél • Hangulat", "BÁR", "https://images.unsplash.com/photo-1514933651103-005eec06c04b?auto=format&fit=crop&w=1400&q=70"),
                new("Ízmûhely", "Szezonális kóstolómenü", "ÉTTEREM", "https://images.unsplash.com/photo-1529692236671-f1f6cf9683ba?auto=format&fit=crop&w=1400&q=70"),
                new("Napfény Terasz", "Nyugodt reggelek a kertben", "ÉTTEREM", "https://images.unsplash.com/photo-1550966871-3ed3cdb5ed0c?auto=format&fit=crop&w=1400&q=70"),
                new("Kristály Bár", "Exklúzív Italok", "BÁR", "/kepek/Kristalybar.png"),
            };
        }
    }
}