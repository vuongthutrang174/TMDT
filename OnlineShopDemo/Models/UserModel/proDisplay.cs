using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopDemo.Models.UserModel
{
    public class proDisplay
    {
        [Required]
        public int MaSP { get; set; }

        [Required]
        public string productName { get; set; }

        [Required]
        public int productType { get; set; }

        [Required]
        public string Color { get; set; }

        [Required]
       
        public string Description { get; set; }


        [Required]
        public string Stock { get; set; }

        [Required]
        public int Discount { get; set; }

        [Required]

        public double Price { get; set; }

        public string Image { get; set; }


        public String LastModify { get; set; }

        public int Conlai { get; set; }

    }
}
