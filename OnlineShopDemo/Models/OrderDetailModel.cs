using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopDemo.Models
{
    public class OrderDetailModel
    {
        public int MaDH {get; set;}
        public string TenSP { get; set; }
        public int MaSP { get; set; }
        public int Soluong { get; set; }
        public double Gia { get; set; }
    }
}
