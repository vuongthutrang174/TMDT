using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopDemo.Models.UserModel
{
    public class CartSelect
    {

        [Required]
        [Display]
        public proDisplay proDisplay { get; set; } = new proDisplay();
        public int Soluong { get; set; }
    }
}
