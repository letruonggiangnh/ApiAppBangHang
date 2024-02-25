namespace ApiAppBangHang.Models
{
    public class AddressDetail
    {
        public int AdressDetailId { get; set; }
        public int UserId { get; set; }
        public UserAddress UserAddress { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public string SubDistrict { get; set; }
        public string Address { get; set; }
    }
}
