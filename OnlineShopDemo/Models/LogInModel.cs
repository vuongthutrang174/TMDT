using System;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopDemo.Models
{
    public class LogInModel
    {
        [Required]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Length of Username must longer than six")]
        public string UserName { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Length of Username must longer than six")]
        public string PassWord { get; set; }

        public bool RememberMe { get; set; }
    }
}
