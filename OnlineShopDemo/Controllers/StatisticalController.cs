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
using OnlineShopDemo.Models.Statictis;
using OnlineShopDemo.Models.ViewModel;

namespace OnlineShopDemo.Controllers
{
    public class StatisticalController : Controller
    {
        public IConfiguration Configuration { get; }

        private readonly IHostingEnvironment _appEnvironment;
        public StatisticalController(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _appEnvironment = hostingEnvironment;
            Configuration = configuration;
        }

        private List<double> TinhDoanhThu(int year)
        {
            List<double> lst = new List<double>();
            
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("tinhDoanhThu", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    connection.Open();
                    for (int month = 1; month <= 12; month++)
                    {
                        command.Parameters.AddWithValue("@Thang",month);
                        command.Parameters.AddWithValue("@nam",year);
                        
                        object kq = command.ExecuteScalar();
                        
                        if (kq == DBNull.Value)
                            lst.Add((Convert.ToDouble(0)));
                        else
                        lst.Add((Convert.ToDouble(kq)));

                        command.Parameters.Clear();
                    }
                }
            }



            return lst;
        } //Tính doanh thu 12 tháng
        private int LaySoLuongDatHang(int year,int Month) //TỔNG SỐ ĐƠN HÀNG
        {
            int SoLuong = 0;
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("DemDonHang", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    connection.Open();
                    command.Parameters.AddWithValue("@Thang", Month);
                    command.Parameters.AddWithValue("@Nam", year);

                    object kq = command.ExecuteScalar();

                    if (kq == DBNull.Value)
                        SoLuong = 0;
                    else
                        SoLuong = Convert.ToInt32(kq);
                }
            }
            return SoLuong;
        }

        private int LayDoanhThu(int year, int Month)
        {
            int SoLuong = 0;
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("tinhDoanhThu", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    connection.Open();
                    command.Parameters.AddWithValue("@Thang", Month);
                    command.Parameters.AddWithValue("@Nam", year);
                    object kq = command.ExecuteScalar();
                    if (kq == DBNull.Value)
                        SoLuong = 0;
                    else
                        SoLuong = Convert.ToInt32(kq);
                }
            }
            return SoLuong;
        } //DONH THU CỦA THÁNG
        private Double TangTruong(int year, int Month) 
        {
            int ThangHienTai;
            int ThangTruoc;
            if (Month == 1)
            {
                 ThangHienTai = LayDoanhThu(year, Month);
                ThangTruoc = LayDoanhThu(year-1,12);
            }
            else
            {
                ThangHienTai = LayDoanhThu(year, Month);
                ThangTruoc = LayDoanhThu(year, Month - 1);
            }
            int kq = ThangHienTai - ThangHienTai;
            return kq;
        } //Tăng trưởng 
        private int LaySoLuongKhachHang(int year, int Month)//TONG SO KHACH HANG THEO THANG
        {
            int SoLuong = 0;
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("DemNguoiDung", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    connection.Open();
                    object kq = command.ExecuteScalar();

                    if (kq == DBNull.Value)
                        SoLuong = 0;
                    else
                        SoLuong = Convert.ToInt32(kq);
                }
            }
            return SoLuong;
        }
        private int KhuVucNgoaiThanh(int year, int Month)//TONG SO KHACH HANG THEO THANG
        {
            int SoLuong = 0;
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("DemDHNgoaiThanh", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    connection.Open();
                    command.Parameters.AddWithValue("@Thang", Month);
                    command.Parameters.AddWithValue("@Nam", year);

                    object kq = command.ExecuteScalar();

                    if (kq == DBNull.Value)
                        SoLuong = 0;
                    else
                        SoLuong = Convert.ToInt32(kq);
                }
            }
            return SoLuong;
        }
        private int KhuVucThanhPho(int year, int Month)//TONG SO KHACH HANG THEO THANG
        {
            int SoLuong = 0;
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("DemDHTP", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    connection.Open();
                    command.Parameters.AddWithValue("@Thang", Month);
                    command.Parameters.AddWithValue("@Nam", year);

                    object kq = command.ExecuteScalar();

                    if (kq == DBNull.Value)
                        SoLuong = 0;
                    else
                        SoLuong = Convert.ToInt32(kq);
                }
            }
            return SoLuong;
        }
        private List<HinhAnhSanPham> layTop5SanPham(int year, int Month)//TONG SO KHACH HANG THEO THANG
        {
            List<HinhAnhSanPham> DS = new List<HinhAnhSanPham>();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("LayHATop5SanPham", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    connection.Open();
                    command.Parameters.AddWithValue("@thang", Month);
                    command.Parameters.AddWithValue("@nam", year);
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        HinhAnhSanPham hinh = new HinhAnhSanPham(); 
                        hinh.TenSP = ds.Tables[0].Rows[i]["tenSP"].ToString();
                        hinh.Image = ds.Tables[0].Rows[i]["Hinhanh"].ToString();
                        DS.Add(hinh);
                    }
                }
            }
            return DS;
        }

        private List<OrderModel> getList( int year,int Month)
        {
            List<OrderModel> DsDH = new List<OrderModel>();

            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("LayDSDonHangTheoNgay", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    connection.Open();
                    command.Parameters.AddWithValue("@thang", Month);
                    command.Parameters.AddWithValue("@nam", year);
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


        [Route("Statistical/Index")]
        public IActionResult Index()
        {
            StatisticViewModel myModel = new StatisticViewModel();
            myModel.date = DateTime.Now;
            myModel.DSDH = getList(myModel.date.Year, myModel.date.Month);
            myModel.DTtheoThang = TinhDoanhThu(myModel.date.Year);
            myModel.SLDatHang = LaySoLuongDatHang(myModel.date.Year, myModel.date.Month);
            myModel.SLKhachHang = LaySoLuongKhachHang(myModel.date.Year, myModel.date.Month);
            myModel.SlDoanhThu = LayDoanhThu(myModel.date.Year, myModel.date.Month);
            myModel.SLDHthanhPho = KhuVucThanhPho(myModel.date.Year, myModel.date.Month);
            myModel.SLDHNoiThanh = KhuVucNgoaiThanh(myModel.date.Year, myModel.date.Month);
            myModel.DSTopSP = layTop5SanPham(myModel.date.Year, myModel.date.Month);
            return View(myModel);
        }

        [HttpPost]
        [Route("Statistical/Statistic")]
        public IActionResult Statistic(StatisticViewModel mod)
        {
            StatisticViewModel myModel = new StatisticViewModel();
            myModel.date = mod.date;
            myModel.DSDH = getList(myModel.date.Year, myModel.date.Month);
            myModel.DTtheoThang = TinhDoanhThu(myModel.date.Year);
            myModel.SLDatHang = LaySoLuongDatHang(myModel.date.Year, myModel.date.Month);
            myModel.SLKhachHang = LaySoLuongKhachHang(myModel.date.Year, myModel.date.Month);
            myModel.SlDoanhThu = LayDoanhThu(myModel.date.Year, myModel.date.Month);
            myModel.SLDHthanhPho = KhuVucThanhPho(myModel.date.Year, myModel.date.Month);
            myModel.SLDHNoiThanh = KhuVucNgoaiThanh(myModel.date.Year, myModel.date.Month);
            myModel.DSTopSP = layTop5SanPham(myModel.date.Year, myModel.date.Month);
            return View(myModel);
        }
    }
}