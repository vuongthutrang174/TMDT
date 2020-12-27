using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopDemo.Models.ViewModel
{
    public class AuthorizeViewModel
    {
        public CustomerInfoModel thongtin = new CustomerInfoModel();
         public List<CustomerInfoModel> DSThongTin { get; set; } = new List<CustomerInfoModel>();
    }
}
