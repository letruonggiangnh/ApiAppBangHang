using System.Collections.Generic;

namespace ApiAppBangHang.Models
{
    public class BookTag
    {
        public int BookTagId { get; set; }
        public string TagName { get; set; }
        public List<ProductBook> ProductBooks { get; set; }
    }
}
