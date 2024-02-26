using ApiAppBanSach.Models;

namespace ApiAppBangHang.Models
{
    public class UserAddress
    {
        public int UserAddressId { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }

        public AddressDetail AddressDetail { get; set; }
        public bool IsDefault { get; set; }
    }
}
