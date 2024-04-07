using ApiAppBanSach.Models;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiAppBangHang.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public string UserId { get; set; }
        [JsonIgnore]
        public AppUser AppUser { get; set; }
        [JsonIgnore]
        public List<CartItem> CartItems { get; set;}
    }
}
