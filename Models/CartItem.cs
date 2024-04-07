using System.Text.Json.Serialization;

namespace ApiAppBangHang.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public int CartId { get; set; }
        [JsonIgnore]
        public Cart Cart { get; set; }
        public int BookId { get; set; }
        [JsonIgnore]
        public ProductBook ProductBook { get; set; }
        public int Quantity { get; set; }
    }
}
