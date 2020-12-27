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
    public class OrderController : Controller
    {


        public IConfiguration Configuration { get; }

        private readonly IHostingEnvironment _appEnvironment;
        public OrderController(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _appEnvironment = hostingEnvironment;
            Configuration = configuration;
        }

        private List<OrderModel> getList()
        {
            List<OrderModel> DsDH = new List<OrderModel>();

            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("LayDSDonHang", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    connection.Open();
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        OrderModel Dh = new OrderModel();

                        Dh.MaDH = Convert.ToInt32(ds.Tables[0].Rows[i]["MaDH"]);
                        Dh.NgayDat = Convert.ToDateTime(ds.Tables[0].Rows[i]["NgayDat"]);
                        Dh.TenKH = ds.Tables[0].Rows[i]["tenKH"].ToString();
                        Dh.TongTien = Convert.ToDouble(ds.Tables[0].Rows[i]["TongTien"]);
                        Dh.sdt = ds.Tables[0].Rows[i]["sdt"].ToString();
                        DsDH.Add(Dh);
                    }
                }
            }
            return DsDH;
        }
        private OrderModel GetOrderInfo(int ID)
        {
            OrderModel ord = new OrderModel();

            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("LayChiTietDonHang", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    connection.Open();

                    command.Parameters.AddWithValue("@MaDH", ID);
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ord.MaDH = Convert.ToInt32(ds.Tables[0].Rows[i]["MaDH"]);
                        ord.TenKH = ds.Tables[0].Rows[i]["TenKH"].ToString();
                        ord.MaDvVanChuyen = Convert.ToInt32(ds.Tables[0].Rows[i]["MaDvVanChuyen"]);
                        ord.TongTien = Convert.ToDouble(ds.Tables[0].Rows[i]["TongTien"]);
                        ord.NgayDat = Convert.ToDateTime(ds.Tables[0].Rows[i]["NgayDat"]);
                        ord.DiaChi = ds.Tables[0].Rows[i]["DiaChi"].ToString();
                        ord.Quan = ds.Tables[0].Rows[i]["Quan"].ToString();
                        ord.ThanhPho = ds.Tables[0].Rows[i]["ThanhPho"].ToString();
                        ord.Email = ds.Tables[0].Rows[i]["Email"].ToString();
                        ord.sdt = ds.Tables[0].Rows[i]["sdt"].ToString();
                    }
                }
            }
            return ord;
        }

        private List<OrderDetailModel> GetOrderDetail(int ID)
        {
            List<OrderDetailModel> ListordDetail = new List<OrderDetailModel>();

            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("LayChiTietMuaHang", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    connection.Open();
                    command.Parameters.AddWithValue("@MaDH", ID);
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        OrderDetailModel ordDetail = new OrderDetailModel();
                        ordDetail.MaDH = Convert.ToInt32(ds.Tables[0].Rows[i]["MaDH"]);
                        ordDetail.TenSP = ds.Tables[0].Rows[i]["tenSP"].ToString();
                        ordDetail.MaSP = Convert.ToInt32(ds.Tables[0].Rows[i]["MaSP"]);
                        ordDetail.Soluong = Convert.ToInt32(ds.Tables[0].Rows[i]["SoLuong"]);
                        ordDetail.Gia = Convert.ToDouble(ds.Tables[0].Rows[i]["Gia"]);
                        ListordDetail.Add(ordDetail);
                    }
                }
            }
            return ListordDetail;
        }



        [Route("Order/Index")]
        public IActionResult Index()
        {
            OrderViewModel myModel = new OrderViewModel();
            myModel.DSDH = getList();
            return View(myModel);
        }

        [Route("Order/OrderDetail/{ID}")]
        public IActionResult OrderDetail(int ID)
        {
            OrderViewModel myModel  = new OrderViewModel();
            myModel.DH = GetOrderInfo(ID);
            myModel.ChiTietDH = GetOrderDetail(ID);
            return View(myModel);
        }

        [Route("Order/OrderVerify/{ID}")]
        public IActionResult OrderVerify(OrderViewModel model)
        {
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("XacNhanDonHang", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    connection.Open();
                    command.Parameters.AddWithValue("@MaDH", model.DH.MaDH);
                    command.ExecuteNonQuery();
                }
            }
            return RedirectToAction("Index","Order");
        }
    }
}