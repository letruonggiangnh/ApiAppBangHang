using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiAppBangHang.Models
{
    public class ProductBook
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string Author { get; set; }
        public int Discount { get; set; }
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
        [System.Text.Json.Serialization.JsonIgnore]
        public BookCategoryChild BookCategoryChild { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public BookTag BookTag { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public List<BookDescription> BookDescriptions { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public CartItem CartItem { get; set; }
    }
}
