using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OnlineShopDemo.Models;
using OnlineShopDemo.Models.ViewModel;

namespace OnlineShopDemo.Controllers
{
    public class AuthorizeController : Controller
    {
        public IConfiguration Configuration { get; }

        private readonly IHostingEnvironment _appEnvironment;
        public AuthorizeController(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _appEnvironment = hostingEnvironment;
            Configuration = configuration;
        }

        private List<CustomerInfoModel> GetInfo()
        {
            List<CustomerInfoModel> DS = new List<CustomerInfoModel>();

            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("LayDSTaiKhoan", connection))
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
                        CustomerInfoModel CusInfo = new CustomerInfoModel();
                        CusInfo.MaKH = ds.Tables[0].Rows[i]["MaKH"].ToString();
                        CusInfo.hinhanh = ds.Tables[0].Rows[i]["hinhanh"].ToString();
                        CusInfo.TenKH = ds.Tables[0].Rows[i]["TenKH"].ToString();
                        CusInfo.sdt = ds.Tables[0].Rows[i]["sdt"].ToString();
                        CusInfo.TaiKhoan = ds.Tables[0].Rows[i]["TaiKhoan"].ToString();
                        CusInfo.QuyenTruyCap = ds.Tables[0].Rows[i]["QuyenTruyCap"].ToString();
                        CusInfo.Email = ds.Tables[0].Rows[i]["Email"].ToString();
                        DS.Add(CusInfo);
                    }
                }
            }
            return DS;
        }


        private List<CustomerInfoModel> GetUser()
        {
            List<CustomerInfoModel> DS = new List<CustomerInfoModel>();

            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("ThongtinKH", connection))
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
                        CustomerInfoModel CusInfo = new CustomerInfoModel();
                        CusInfo.MaKH = ds.Tables[0].Rows[i]["MaKH"].ToString();
                        CusInfo.hinhanh = ds.Tables[0].Rows[i]["hinhanh"].ToString();
                        CusInfo.TenKH = ds.Tables[0].Rows[i]["TenKH"].ToString();
                        CusInfo.sdt = ds.Tables[0].Rows[i]["sdt"].ToString();
                        CusInfo.TaiKhoan = ds.Tables[0].Rows[i]["TaiKhoan"].ToString();
                        CusInfo.QuyenTruyCap = ds.Tables[0].Rows[i]["QuyenTruyCap"].ToString();
                        CusInfo.Email = ds.Tables[0].Rows[i]["Email"].ToString();
                        DS.Add(CusInfo);
                    }
                }
            }
            return DS;
        }

        [Route("Authorize/Index")]
        public IActionResult Index()
        {
            AuthorizeViewModel myModel = new AuthorizeViewModel();
            myModel.DSThongTin = GetInfo();
            return View(myModel);
        }


        [Route("Authorize/ViewUser")]
        public IActionResult ViewUser()
        {
            AuthorizeViewModel myModel = new AuthorizeViewModel();
            myModel.DSThongTin = GetUser();
            return View(myModel);
        }



        [Route("Authorize/Set/{UserName}")]
        public IActionResult Set (string Username)
        {
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("CapQuyenQT", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    connection.Open();
                    command.Parameters.AddWithValue("@username", Username);
                    command.ExecuteNonQuery();
                }
                return RedirectToAction("Index","Authorize");
            }
        }


        [Route("Authorize/Reset/{UserName}")]
        public IActionResult Reset(string Username)
        {
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("BoQuyenQT", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    connection.Open();
                    command.Parameters.AddWithValue("@username", Username);
                    command.ExecuteNonQuery();
                }
                return RedirectToAction("ViewUser", "Authorize");
            }
        }


    }
}