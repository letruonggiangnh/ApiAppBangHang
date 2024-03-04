using System.Collections.Generic;

namespace ApiAppBangHang.Models
{
    public class BookCategoryChild
    {
        public int CategoryChildId { get; set; }
        public int CategoryParentId { get; set; }
        public string CategoryChildName { get; set; }  
        public List<ProductBook> ProductBooks { get; set; }
        public BookCategoryParent CategoryParent { get; set; } 
    }
}
