using System.Text.Json;
using costa_serena_grand_hotel_FRONTEND.Dtos;

namespace costa_serena_grand_hotel_FRONTEND.Services
{
    public class CartService
    {
        private const string CartKey = "webshop.cart";
        private readonly IHttpContextAccessor _http;

        public CartService(IHttpContextAccessor http)
        {
            _http = http;
        }

        public List<CartItemDto> GetCart()
        {
            var json = _http.HttpContext?.Session.GetString(CartKey);

            if (string.IsNullOrWhiteSpace(json))
                return new List<CartItemDto>();

            return JsonSerializer.Deserialize<List<CartItemDto>>(json) ?? new List<CartItemDto>();
        }

        public void SaveCart(List<CartItemDto> items)
        {
            var json = JsonSerializer.Serialize(items);
            _http.HttpContext?.Session.SetString(CartKey, json);
        }

        public void AddItem(TermekDto termek, int mennyiseg = 1)
        {
            var cart = GetCart();
            var existing = cart.FirstOrDefault(x => x.TermekId == termek.Id);

            if (existing == null)
            {
                cart.Add(new CartItemDto
                {
                    TermekId = termek.Id,
                    Nev = termek.Nev,
                    Ar = termek.Ar,
                    KepUrl = termek.KepUrl,
                    Mennyiseg = mennyiseg
                });
            }
            else
            {
                existing.Mennyiseg += mennyiseg;
            }

            SaveCart(cart);
        }

        public void RemoveItem(int termekId)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.TermekId == termekId);

            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
            }
        }

        public void ChangeQuantity(int termekId, int mennyiseg)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.TermekId == termekId);

            if (item == null)
                return;

            if (mennyiseg <= 0)
                cart.Remove(item);
            else
                item.Mennyiseg = mennyiseg;

            SaveCart(cart);
        }

        public void Clear()
        {
            _http.HttpContext?.Session.Remove(CartKey);
        }

        public int GetCount()
        {
            return GetCart().Sum(x => x.Mennyiseg);
        }

        public int GetTotal()
        {
            return GetCart().Sum(x => x.Osszeg);
        }
    }
}