using System.Collections.Generic;

namespace ApiAppBangHang.Models
{
    public class BookCategoryParent
    {
        public int CategoryParentId { get; set; }
        public string CategoryParentName { get; set; }
        public List<BookCategoryChild> BookCategoryChilds { get; set; }
    }
}
