using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopDemo.Models
{
    public class CustomerInfoModel
    {

        [Required]
        public string MaKH { get; set; }
        [Required]
        public string hinhanh { get; set; }
        [Required]
        public string TenKH { get; set; }
        [Required]
        public string TaiKhoan { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string sdt { get; set; }
        [Required]
        public string QuyenTruyCap { get; set; }
    }
}
