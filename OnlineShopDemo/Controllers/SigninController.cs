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

    public class SigninController : Controller
    {
        public IConfiguration Configuration { get; }

        private readonly IHostingEnvironment _appEnvironment;
        public SigninController(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _appEnvironment = hostingEnvironment;
            Configuration = configuration;
        }

        [Route("Signin/Index")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("Signin/Signin")]
        public IActionResult Signin(Account acc)
        {
            string QTC = LayQTC(acc.username, acc.pass, acc.QTC);
            if (QTC == "Ur")
            {
                Client li = new Client();
                li = LayThongTin(acc.username);
                li.Password = acc.pass;
                Client client = HttpContext.Session.Get<Client>("ssClient");
                if (client == null)
                {
                    client = li;
                }
                HttpContext.Session.Set("ssClient", client);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        public IActionResult Signup(Account acc)
        {
            return View();
        }
        private string LayQTC(string username, string pass, string QTC)
        {
            string result = "";
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("Login", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    connection.Open();

                    command.Parameters.AddWithValue("@TK", username);
                    command.Parameters.AddWithValue("@MK", pass);

                    result = command.ExecuteScalar().ToString();
                    
                }
            }
            return result;
        }
        private Client LayThongTin(string username)
        {
            Client pro = new Client();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("LayThongTin", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    command.Parameters.AddWithValue("@ID", username);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        pro.MaKH = Convert.ToInt32(ds.Tables[0].Rows[i]["MaKH"]);
                        pro.TenKH = ds.Tables[0].Rows[i]["TenKH"].ToString();
                        pro.DiaChi = ds.Tables[0].Rows[i]["DiaChi"].ToString();
                        pro.Quan = ds.Tables[0].Rows[i]["Quan"].ToString();
                        pro.ThanhPho = ds.Tables[0].Rows[i]["ThanhPho"].ToString();
                        pro.Email = ds.Tables[0].Rows[i]["Email"].ToString();
                        pro.sdt = ds.Tables[0].Rows[i]["sdt"].ToString();
                        pro.Taikhoan = username;
                    }
                }
            }
            return pro;
        }
    }
    
}