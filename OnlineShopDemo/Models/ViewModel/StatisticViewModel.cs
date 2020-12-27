using OnlineShopDemo.Models.Statictis;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopDemo.Models.ViewModel
{
    public class StatisticViewModel
    {
        [Required]
        [Display(Name = "date")]
        public DateTime date { get; set; }
        public int SLDatHang { get; set; }
        public int SLTangTruong { get; set; }
        public int SlDoanhThu { get; set; }
        public int SLKhachHang { get; set; }
        public int SLDHthanhPho { get; set; }
        public int SLDHNoiThanh { get; set; }
        public List<HinhAnhSanPham> DSTopSP { get; set; } 

        public List<Double> DTtheoThang { get; set; }

        public List<OrderModel> DSDH { get; set; } = new List<OrderModel>();

    }
}
