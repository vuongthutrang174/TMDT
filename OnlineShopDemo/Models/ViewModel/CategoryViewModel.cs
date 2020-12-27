using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopDemo.Models.ViewModel
{
    public class CategoryViewModel
    {
        public string  CategoryName {get; set; }
        public LoaiSanPham Category { get; set; }
        public List<LoaiSanPham> CategoryList { get; set; }

    }
}
