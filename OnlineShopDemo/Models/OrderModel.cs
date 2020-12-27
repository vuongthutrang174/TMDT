using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopDemo.Models
{
    public class OrderModel
    {
        public int MaDH { get; set; }

        public string TenKH { get; set; }

        public string DiaChi { get; set; }
        public string Quan { get; set; }
        public string ThanhPho { get; set; }
        public string Email { get; set; }
        public int MaDvVanChuyen { get; set; }
        public double TongTien { get; set; }
        public string sdt { get; set; }
        public DateTime NgayDat { get; set; }

    }
}
