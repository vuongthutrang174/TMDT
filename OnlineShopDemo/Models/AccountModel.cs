using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopDemo.Models
{
    public class AccountModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Length of Username must longger than six")]
        public string Username { get; set; }

        public string Password { get; set; }

        [Required]
        public string QuyenTruyCap { get; set; }
    }
}
