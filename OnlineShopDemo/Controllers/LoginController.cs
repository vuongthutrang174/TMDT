using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineShopDemo.Models;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using OnlineShopDemo.Extensions;

namespace OnlineShopDemo.Controllers
{
    public class LoginController : Controller
    {
        public IConfiguration Configuration { get; }
        public LoginController(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public AccountInfoModel GetInfo(string username)
        {
            AccountInfoModel acc = new AccountInfoModel();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("layThongTinTaiKhoan", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }

                    command.Parameters.AddWithValue("@UserName", username);
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    acc.TenKH = ds.Tables[0].Rows[0]["TenKH"].ToString();
                    acc.NamSinh = Convert.ToDateTime(ds.Tables[0].Rows[0]["NamSinh"].ToString());
                    acc.DiaChi = ds.Tables[0].Rows[0]["DiaChi"].ToString();
                    acc.Quan = ds.Tables[0].Rows[0]["Quan"].ToString();
                    acc.ThanhPho = ds.Tables[0].Rows[0]["ThanhPho"].ToString();
                    acc.sdt = ds.Tables[0].Rows[0]["sdt"].ToString();
                    acc.image = ds.Tables[0].Rows[0]["hinhanh"].ToString();
                }
            }
            return acc;
        }

        [HttpPost]
        public IActionResult Login(LogInModel model)
        {
            if (ModelState.IsValid)
            {
               
                string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string kq;
                    using (SqlCommand command = new SqlCommand("DangNhap", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        
                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }

                        connection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@TK", model.UserName);
                        command.Parameters.AddWithValue("@MK", model.PassWord);
                        SqlParameter ret = command.Parameters.Add("@QTC", SqlDbType.NVarChar, 10);
                        ret.Direction = ParameterDirection.Output;
                        command.ExecuteNonQuery();
                        kq = command.Parameters["@QTC"].Value is DBNull ? null : command.Parameters["@QTC"].Value.ToString();
                    }

                    if (kq == "Ad")
                    {
                        AccountInfoModel Cus = GetInfo(model.UserName);
                        AccountInfoModel Admin = HttpContext.Session.Get<AccountInfoModel>("AdminName");
                        if(Admin == null)
                        {
                            Admin = Cus;
                        }
                        HttpContext.Session.Set("AdminName",Admin);
                        TempData["Username"] = model.UserName;
                        return RedirectToAction("Home", "DashBoard");
                    }
                    else if (kq == "Ur")
                    {
                        TempData["Username"] = model.UserName;
                        return RedirectToAction("Home", "UserDashBoard");
                    }
                    else if (kq == "Ban")
                    {
                        ModelState.AddModelError("Password", "YOUR ACCOUNT IS BANNED");
                        return View(model);
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "INCORRECT PASSWORD OR USERNAME ");
                        return View(model);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("Password", "INCORRECT PASSWORD OR USERNAME ");
                return View(model);
            }
        }
        //the framework handles this


        [Route("Login/Login")]
        [HttpGet]
        public IActionResult Login()
        {
            HttpContext.Session.Clear();
            return View();
        }


        public IActionResult Index()
        {
            return View();
        }

        [Route("Login/Logout")]
        public IActionResult Logout()
        {
            return RedirectToAction("Login", "Login");
        }


        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(SignUpModel model)
        {
            if (ModelState.IsValid)
            {

            }
            else
            {
                
            }
            return View(model);
        }

    }
}