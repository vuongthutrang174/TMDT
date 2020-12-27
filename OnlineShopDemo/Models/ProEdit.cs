using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopDemo.Models
{
    public class ProEdit
    {
        [Required]
        public int MaSP { get; set; }

        [Required(ErrorMessage = "Please enter the name")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Length of Username must longer than six")]
        public string productName { get; set; }

        [Required]
        public int productType{ get; set; }

        [Required]
        public string Color { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "Must have Description")]
        public string Description { get; set; }

    
        [Required]
        public string Stock { get; set; }

        [Required]
        public int Discount { get; set; }

        [Required]
    
        public double Price { get; set; }

        public string Image { get; set; }


        public String LastModify { get; set; }

    }
}
