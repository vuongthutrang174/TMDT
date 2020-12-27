using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopDemo.Models
{
    public class Product
    {
        [Required]
        public string ProductType { get; set; }

        [Required(ErrorMessage = "Please enter the name")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Length of Username must longer than six")]
        public string productName { get; set; }

        [Required]
        public string color { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Please enter the price")]
        public string Price { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "Must have Description")]
        public string Description { get; set; }

        public string Image { get; set; }

    }
}
