using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopDemo.Models.UserModel
{
    public class Bill
    {
        public Client Client { get; set; } = new Client();
        public List<CartSelect> Select { get; set; }
        public double Price { get; set; }
        public double Giaship { get; set; }
        public List<Vanchuyen> danhsach { get; set; } = new List<Vanchuyen>();
        public Vanchuyen Vanchuyen { get; set; } = new Vanchuyen();
        public List<string> tinhthanh { get; set; }
      

    }
}
