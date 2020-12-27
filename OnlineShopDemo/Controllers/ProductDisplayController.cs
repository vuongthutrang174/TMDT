using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OnlineShopDemo.Models.UserModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OnlineShopDemo.Extensions;

namespace OnlineShopDemo.Controllers
{
    public class ProductDisPlayController : Controller
    {
        public IConfiguration Configuration { get; }

        private readonly IHostingEnvironment _appEnvironment;
        public ProductDisPlayController(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _appEnvironment = hostingEnvironment;
            Configuration = configuration;
        }


        private int GetKho(int ID)
        {
            int sl = 0;
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("LaySoLuong", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    connection.Open();
                    command.Parameters.AddWithValue("@ID", ID);
                    sl = Convert.ToInt32(command.ExecuteScalar());
                }

            }
            return sl;
        }
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
                    pro.Conlai = GetKho(pro.MaSP);
                }
            }
            return pro;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Route("ProductDisplay/ProductDetail/{ID}")]
        public IActionResult ProductDetail(int ID)
        {
           CartSelect myModel = new CartSelect();
            myModel.proDisplay = GetProduct(ID);
            return View(myModel);
        }
     
        [Route("ProductDisplay/AddCart")]
        [HttpPost,ActionName("AddCart")]
        [ValidateAntiForgeryToken]
        public IActionResult AddCart(CartSelect ID)
        {
            CartSelect pro = new CartSelect();
            pro.proDisplay = GetProduct(ID.proDisplay.MaSP);
            pro.Soluong = ID.Soluong;
            List<CartSelect> lstShoppingCart = HttpContext.Session.Get<List<CartSelect>>("ssShoppingCart");
            if (lstShoppingCart == null)
            {
                lstShoppingCart = new List<CartSelect>();
                lstShoppingCart.Add(pro);
            }
            else
            {
                for(int i= 0;i <lstShoppingCart.Count;i++)
                {
                    if (lstShoppingCart[i].proDisplay.MaSP == pro.proDisplay.MaSP)
                    {
                        lstShoppingCart[i].Soluong = lstShoppingCart[i].Soluong + pro.Soluong;
                        break;
                    }
                    else
                    if (i == lstShoppingCart.Count - 1)
                    {
                        lstShoppingCart.Add(pro);
                        break;
                    }

                }
         
            }

            HttpContext.Session.Set("ssShoppingCart", lstShoppingCart);
            return RedirectToAction("ProductDetail", "ProductDisplay", new { ID = pro.proDisplay.MaSP });
        }
    }
}