using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineShopDemo.Models.UserModel;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.Data.SqlClient;
using System.Data;
using Amando2.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OnlineShopDemo.Extensions;

namespace Amando2.Controllers
{

    
    public class CartController : Controller
    {
        
        private proDisplay GetProduct(int ID)
        {
            proDisplay pro = new proDisplay();

            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("LaySanPham", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    command.Parameters.AddWithValue("@ID", ID);
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        pro.MaSP = Convert.ToInt32(ds.Tables[0].Rows[i]["MaSp"]);
                        pro.productName = ds.Tables[0].Rows[i]["tenSP"].ToString();
                        pro.productType = Convert.ToInt32(ds.Tables[0].Rows[i]["MaLoai"]);
                        pro.Color = ds.Tables[0].Rows[i]["mausac"].ToString();
                        pro.Description = ds.Tables[0].Rows[i]["Mota"].ToString();
                        pro.Image = ds.Tables[0].Rows[i]["Hinhanh"].ToString();
                        pro.Discount = Convert.ToInt32(ds.Tables[0].Rows[i]["Giamgia"]);
                        pro.Price = Convert.ToDouble(ds.Tables[0].Rows[i]["Gia"]);
                        pro.Stock = ds.Tables[0].Rows[i]["TrangThai"].ToString();
                        pro.LastModify = ds.Tables[0].Rows[i]["LastModify"].ToString();
                    }
                }
            }
            return pro;
        }
        public IConfiguration Configuration { get; }

        private readonly IHostingEnvironment _appEnvironment;
        public CartController(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _appEnvironment = hostingEnvironment;
            Configuration = configuration;
        }
        public Cart CartView { get; set; }



        [Route("Cart/NotFound")]
        public IActionResult NoSelect()
        {
            return View();
        }


        [Route("Cart/Index")]
        public IActionResult Index()
        {

            List<CartSelect> lstShoppingCart = HttpContext.Session.Get<List<CartSelect>>("ssShoppingCart");
            Bill model =  new Bill();
            model.Select = lstShoppingCart;
            if(lstShoppingCart == null)
            {
                return RedirectToAction("NoSelect", "Cart");
            }
           else
            {
                return View(model);
            }
          
        }
      
    }
}