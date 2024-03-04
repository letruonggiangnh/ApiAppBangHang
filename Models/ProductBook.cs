using System.Collections.Generic;

namespace ApiAppBangHang.Models
{
    public class ProductBook
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string Author { get; set; }
        public double Discount { get; set; }
        public string PackagingSize { get; set; }
        public string Weight { get; set; }
        public string PublishingCompany { get; set; }
        public string Supplier { get; set; }
        public uint NumberOfPages { get; set; }
        public string Language { get; set; }
        public string ImageUrl { get; set; }
        public int BookPrice { get; set; }
        public int TagId { get; set; }
        public int SoldQuantity { get; set; }
        public int CategoryChildId { get; set; }
        public BookCategoryChild BookCategoryChild { get; set; }
        public BookTag BookTag { get; set; }
        public List<BookDescription> BookDescriptions { get; set; }
    }
}
