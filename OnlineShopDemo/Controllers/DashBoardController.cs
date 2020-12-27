using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShopDemo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Dynamic;

namespace OnlineShopDemo.Controllers
{
    public class ViewProductModel
    {
        public List<proDisplay> ProductList { get; set; }
        public List<LoaiSanPham> CateList { get; set; }

        public Product product { get; set; }
        
        public ProEdit proEdit { get; set; }

        public warehouseModel Soluong = new warehouseModel();

    }
    public class DashBoardController : Controller
    {

        [HttpGet]
        [Route("DashBoard/Home")]
        public IActionResult Home()
        {
            return View();
        }

    }
}