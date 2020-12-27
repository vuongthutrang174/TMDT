using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopDemo.Models.ViewModel
{
    public class OrderViewModel
    {
        public List<OrderModel> DSDH { get; set; } = new List<OrderModel>();

        public OrderModel DH { get; set; }

        public List<OrderDetailModel> ChiTietDH { get; set; } = new List<OrderDetailModel>();
    }
}
