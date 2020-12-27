using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineShopDemo.Models.UserModel;
using OnlineShopDemo.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Http;

namespace OnlineShopDemo.Controllers
{
    public class ProfileController : Controller
    {
        public IConfiguration Configuration { get; }

        private readonly IHostingEnvironment _appEnvironment;
        public ProfileController(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _appEnvironment = hostingEnvironment;
            Configuration = configuration;
        }

        [Route("Profile/Index")]
        public IActionResult Index()
        {
            Client myModel = HttpContext.Session.Get<Client>("ssClient");
            return View(myModel);
        }

        [HttpPost]
        [Route("Profile/Edit")]
        public IActionResult Edit(Client ID)
        {
            Client myModel = new Client();
            myModel = ID;
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                using (SqlCommand command = new SqlCommand("CapNhatCTTK", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    connection.Open();
                    command.Parameters.AddWithValue("@Ma", myModel.MaKH);
                    command.Parameters.AddWithValue("@Ten", myModel.TenKH);
                    command.Parameters.AddWithValue("@Email", myModel.Email);
                    command.Parameters.AddWithValue("@Diachi", myModel.DiaChi);
                    command.Parameters.AddWithValue("@Quan", myModel.Quan);
                    command.Parameters.AddWithValue("@SDT", myModel.sdt);

                    command.ExecuteNonQuery();

                }

            }
            HttpContext.Session.Set<Client>("ssClient", myModel);
            return RedirectToAction("Index", "Profile");
        }


        [HttpPost]
        [Route("Profile/ChangePass")]
        public IActionResult ChangePass(Client ID)
        {
                if (ID.Password == ID.OldPasswordVerify)
                {
                    Client myModel = new Client();
                    myModel = ID;
                    string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand("CapNhatMK", connection))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            command.Parameters.AddWithValue("@taikhoan", myModel.Taikhoan);
                            command.Parameters.AddWithValue("@matkhau", myModel.NewPassword);
                            command.ExecuteNonQuery();

                        }

                    }

                    return RedirectToAction("Index", "Profile");
                }
                else
                {
                    return View();
                }
        }

        [HttpGet]
        [Route("Profile/UpdatePassWord")]
        public IActionResult UpdatePassWord()
        {
            Client myModel = HttpContext.Session.Get<Client>("ssClient");
            myModel.NewPassword = "";
            myModel.NewPasswordVerify = "";
            myModel.OldPasswordVerify = "";
            return View(myModel);
        }
    }
}