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
using OnlineShopDemo.Extensions;

namespace Amando2.Controllers
{
   
    public class HomeController : Controller
    {
        public IConfiguration Configuration { get; }

        private readonly IHostingEnvironment _appEnvironment;
        public HomeController(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _appEnvironment = hostingEnvironment;
            Configuration = configuration;
        }

        private List<proDisplay> getList()
        {
            List<proDisplay> productList = new List<proDisplay>();

            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("LayDSSanPham", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        proDisplay pro = new proDisplay();
                        pro.MaSP = Convert.ToInt32(ds.Tables[0].Rows[i]["MaSp"]);
                        pro.Image = ds.Tables[0].Rows[i]["Hinhanh"].ToString();
                        pro.productName = ds.Tables[0].Rows[i]["tenSP"].ToString();
                        pro.Price = Convert.ToDouble(ds.Tables[0].Rows[i]["Gia"]);
                        pro.Stock = ds.Tables[0].Rows[i]["TrangThai"].ToString();
                        productList.Add(pro);
                    }
                }
            }
            return productList;
        }
      
        [HttpGet]
        [Route("Home/Index")]
        public IActionResult Index()
        {
            ProductViewModel myModel = new ProductViewModel();
            myModel.ProductList = getList();
            return View(myModel);
        }
      
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
