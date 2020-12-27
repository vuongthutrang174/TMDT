using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopDemo.Models.ViewModel
{
    public class AccountViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "Verify Password is invalid")]
        public string NewPasswordVerify { get; set; }

        [Required]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Verify Password is invalid")]
        public string OldPasswordVerify { get; set; }


        public string Password { get; set; }

        public AccountModel Account { get; set; }

        public AccountInfoModel Info {get;set;}
    }
}
