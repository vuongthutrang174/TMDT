using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopDemo.Models
{
    public class proDisplay
    {
        public int MaSP { get; set; }

        [Required]
        public string Image { get; set; }

        [Required]
        public string productName { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public string Stock{ get; set; }
    }
}
