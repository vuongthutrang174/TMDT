using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopDemo.Models.UserModel
{
    public class ProductViewModel
    {
        public List<proDisplay> ProductList { get; set; }


        public List<LoaiSP> LoaiHang { get; set; }
    }
}
