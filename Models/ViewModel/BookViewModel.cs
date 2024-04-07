namespace ApiAppBangHang.Models.ViewModel
{
    public class BookViewModel
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
        public int BookPrice { get; set; }
        public int Discount { get; set; }
        public string ImageUrl { get; set; }
        public int BookDiscountedPrice { get; set; }
    }
}
