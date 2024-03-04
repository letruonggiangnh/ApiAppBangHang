namespace ApiAppBangHang.Models
{
    public class BookDescription
    {
        public int BookDescriptionId { get; set; }
        public int BookId { get; set; } 
        public string Description { get; set; }
        public ProductBook ProductBook { get; set; }
    }
}
