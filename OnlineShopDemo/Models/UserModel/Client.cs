using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopDemo.Models.UserModel
{
    public class Client
    {
        [Required]
        public string Password { get; set; }
        public string QTC { get; set; }
        public int? MaKH { get; set; }
        public string TenKH { get; set; }
        public string DiaChi { get; set; }
        public string Quan { get; set; }
        public string ThanhPho { get; set; }
        public string Email { get; set; }
        public string sdt { get; set; }
        public string Taikhoan { get; set; }
        public string Hinhanh { get; set; }
        [Required]
        [DataType(DataType.Password)]

        public string NewPassword { get; set; }

        [Required]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "Verify Password is invalid")]
        public string NewPasswordVerify { get; set; }

        [Required]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Verify Password is invalid")]
        public string OldPasswordVerify { get; set; }

    }
}
