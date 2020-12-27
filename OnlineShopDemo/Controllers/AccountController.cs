using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OnlineShopDemo.Models;
using OnlineShopDemo.Models.ViewModel;

namespace OnlineShopDemo.Controllers
{
    public class AccountController : Controller
    {
        public IConfiguration Configuration { get; }

        private readonly IHostingEnvironment _appEnvironment;

        public AccountController(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _appEnvironment = hostingEnvironment;
            Configuration = configuration;
        }

      

        //Lấy một loại SP
        public AccountModel GetAccount(string username)
        {
            AccountModel acc = new AccountModel();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("LayTaiKhoan", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }

                    command.Parameters.AddWithValue("@Username", username);
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    acc.Username = ds.Tables[0].Rows[0]["TaiKhoan"].ToString();
                    acc.Password = ds.Tables[0].Rows[0]["MatKhau"].ToString();
                    acc.Email = ds.Tables[0].Rows[0]["Email"].ToString();
                    acc.QuyenTruyCap = ds.Tables[0].Rows[0]["QuyenTruyCap"].ToString();

                }
            }
            return acc;
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

        [Route("Account/Index")]
        public IActionResult Index()
        {
            AccountViewModel myModel = new AccountViewModel();
            myModel.Account = GetAccount(TempData["Username"].ToString());
            myModel.Info = GetInfo(TempData["Username"].ToString());
            myModel.Password = myModel.Account.Username;
            return View(myModel);
        }


        //[Route("Account/AccountDetail/{username}")]
        //public IActionResult AccountDetail(string username)
        //{
        //    AccountViewModel myModel = new AccountViewModel();
        //    myModel.Account = GetAccount(username);
        //    return View(myModel);
        //}


        [Route("Account/AccountEdit/{username}")]
        public IActionResult AccountEdit(string username)
        {
            AccountViewModel myModel = new AccountViewModel();
            myModel.Account = GetAccount(username);
            myModel.Info = GetInfo(username);
            myModel.Password = myModel.Account.Password;
            return View(myModel);
        }

        [HttpPost]
        [Route("Account/EditAsync")]
        public async Task<IActionResult> EditAsync(AccountViewModel myModel, IFormFile pic)
        {
            if (ModelState.IsValid)
            {
                if (myModel.Password == myModel.OldPasswordVerify)
                {
                    if (pic != null)
                    {
                        var file = pic;
                        var uploads = Path.Combine(_appEnvironment.WebRootPath, "Images");
                        var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                        using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                            string filePath = "Images\\" + fileName;
                            myModel.Info.image = filePath;
                        }
                        string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            using (SqlCommand command = new SqlCommand("CapNhatTK", connection))
                            {
                                command.CommandType = System.Data.CommandType.StoredProcedure;

                                if (connection.State == ConnectionState.Open)
                                {
                                    connection.Close();
                                }
                                connection.Open();
                                command.Parameters.AddWithValue("@taikhoan", myModel.Account.Username);
                                command.Parameters.AddWithValue("@Email", myModel.Account.Email);
                                command.Parameters.AddWithValue("@matkhau", myModel.NewPassword);
                                command.Parameters.AddWithValue("@TenKH", myModel.Info.TenKH);
                                command.Parameters.AddWithValue("@NamSinh", myModel.Info.NamSinh);
                                command.Parameters.AddWithValue("@DiaChi", myModel.Info.DiaChi);
                                command.Parameters.AddWithValue("@Quan", myModel.Info.Quan);
                                command.Parameters.AddWithValue("@ThanhPho", myModel.Info.ThanhPho);
                                command.Parameters.AddWithValue("@sdt", myModel.Info.sdt);
                                command.Parameters.AddWithValue("@hinhanh", myModel.Info.image);
                                command.ExecuteNonQuery();
                            }
                        }
                        return RedirectToAction("Index", "Account", new { username = myModel.Account.Username });
                    }
                    else
                    {
                        string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            using (SqlCommand command = new SqlCommand("CapNhatTK", connection))
                            {
                                command.CommandType = System.Data.CommandType.StoredProcedure;

                                if (connection.State == ConnectionState.Open)
                                {
                                    connection.Close();
                                }
                                connection.Open();
                                command.Parameters.AddWithValue("@taikhoan", myModel.Account.Username);
                                command.Parameters.AddWithValue("@Email", myModel.Account.Email);
                                command.Parameters.AddWithValue("@matkhau", myModel.NewPassword);
                                command.Parameters.AddWithValue("@TenKH", myModel.Info.TenKH);
                                command.Parameters.AddWithValue("@NamSinh", myModel.Info.NamSinh);
                                command.Parameters.AddWithValue("@DiaChi", myModel.Info.DiaChi);
                                command.Parameters.AddWithValue("@Quan", myModel.Info.Quan);
                                command.Parameters.AddWithValue("@ThanhPho", myModel.Info.ThanhPho);
                                command.Parameters.AddWithValue("@sdt", myModel.Info.sdt);
                                command.Parameters.AddWithValue("@hinhanh", myModel.Info.image);
                                command.ExecuteNonQuery();
                            }
                        }
                        return RedirectToAction("Index", "Account", new { username = myModel.Account.Username });
                    }
                }
                else
                {
                    ModelState.AddModelError("OldPasswordVerify","Old password is incorrect");
                    return RedirectToAction("AccountEdit", "Account",new { username = myModel.Account.Username });
                }
            }
            else
            {
                ModelState.AddModelError("OldPasswordVerify", "Old password is incorrect");
                return RedirectToAction("AccountEdit", "Account", new { username = myModel.Account.Username });
            }
        }


        [Route("Account/AccountDelete/{username}")]
        public IActionResult AccountDelete(string username)
        {
            AccountViewModel myModel = new AccountViewModel();
            myModel.Account = GetAccount(username);
            myModel.Password = myModel.Account.Password;
            myModel.Info = GetInfo(TempData["Username"].ToString());
            return View(myModel);
        }


        [HttpPost]
        [Route("Account/Delete")]
        public IActionResult Delete(AccountViewModel myModel)
        {
                if (myModel.Password == myModel.OldPasswordVerify)
                {
                    string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand("XoaTK", connection))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;

                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }

                            connection.Open();
                            command.Parameters.AddWithValue("@TaiKhoan", myModel.Account.Username);
                            command.ExecuteNonQuery();
                        }
                    }
                    return RedirectToAction("Login", "Login");
                }
                else
                {
                    ModelState.AddModelError("OldPasswordVerify", "Old password is incorrect");
                    return RedirectToAction("AccountDelete", "Account", new { username = myModel.Account.Username });
                }
        }


        [Route("Account/AccountCreate")]
        public IActionResult CategoryCreate()
        {
            return View();
        }


        [HttpPost]
        [Route("Account/Create")]
        public IActionResult Create(CategoryViewModel myModel)
        {
            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("ThemLoaiSP", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }
                        connection.Open();

                        command.Parameters.AddWithValue("@TenLoai", myModel.CategoryName);
                        command.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("Index", "Account");
            }
            else
            {
                return RedirectToAction("AccountCreate", "Account");
            }
        }


    }
}