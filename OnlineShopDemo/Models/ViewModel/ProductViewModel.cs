using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopDemo.Models.ViewModel
{
    public class ProductViewModel
    {
        public List<proDisplay> ProductList { get; set; }
        public List<LoaiSanPham> CateList { get; set; }
        public Product product { get; set; }
        public ProEdit proEdit { get; set; }

        public warehouseModel Soluong = new warehouseModel();
    }
}
