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
using Microsoft.AspNetCore.Http;

namespace Amando2.Controllers
{
    public class CheckoutController : Controller
    {
        public IConfiguration Configuration { get; }

        private readonly IHostingEnvironment _appEnvironment;
        public CheckoutController(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _appEnvironment = hostingEnvironment;
            Configuration = configuration;
        }

        private List<Vanchuyen> getListVC()
        {
            List<Vanchuyen> productList = new List<Vanchuyen>();

            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("LayDsDonViVanChuyen", connection))
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
                        Vanchuyen pro = new Vanchuyen();
                        pro.MaDVVanChuyen = Convert.ToInt32(ds.Tables[0].Rows[i]["MaDvVanChuyen"]);
                        pro.TenDv = ds.Tables[0].Rows[i]["TenDv"].ToString();

                        pro.GiaNgoaiThanh = Convert.ToDouble(ds.Tables[0].Rows[i]["GiaNgoaiThanh"]);
                        pro.GiaNoiThanh = Convert.ToDouble(ds.Tables[0].Rows[i]["GiaNoiThanh"]);
                        productList.Add(pro);
                    }
                }
            }
            return productList;
        }
        private double getGiaVC(string TP, int MaDV)
        {
            double price = 0;
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("LayGiaVanChuyen", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    connection.Open();
                    command.Parameters.AddWithValue("@ViTri", TP);
                    command.Parameters.AddWithValue("@MaDV", MaDV);

                    price = Convert.ToDouble(command.ExecuteScalar());
                }
            }
            return price;

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
                }
            }
            return pro;
        }
        private List<string> getListTinhThanh()
        {
            List<string> dsach = new List<string>();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string tp = "";
                using (SqlCommand command = new SqlCommand("Laydstp", connection))
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
                        tp = Convert.ToString(ds.Tables[0].Rows[i]["TenTP"]);
                        dsach.Add(tp);
                    }
                }
                return dsach;
            }
        }


        [Route("Checkout/Index")]
        public IActionResult Index()
        {
            List<CartSelect> lstShoppingCart = HttpContext.Session.Get<List<CartSelect>>("ssShoppingCart");
            if (lstShoppingCart == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Checkout", "Checkout");
            }
        }
        [HttpPost]
        [HttpGet]
        [Route("Checkout/Checkout")]
        public IActionResult Checkout(Bill bill)
        {

            HttpContext.Session.Set<List<CartSelect>>("ssShoppingCart", null);
            List<CartSelect> lstShoppingCart = HttpContext.Session.Get<List<CartSelect>>("ssShoppingCart");

            foreach (var pro in bill.Select)
            {
                CartSelect cs = new CartSelect();
                cs.proDisplay = GetProduct(pro.proDisplay.MaSP);
                cs.Soluong = pro.Soluong;

                if (lstShoppingCart == null)
                {
                    lstShoppingCart = new List<CartSelect>();
                }

                lstShoppingCart.Add(cs);
                HttpContext.Session.Set("ssShoppingCart", lstShoppingCart);
            }
            List<CartSelect> ds = HttpContext.Session.Get<List<CartSelect>>("ssShoppingCart");

            Bill myModel = new Bill();
            myModel.Select = ds;
            myModel.danhsach = getListVC();
            myModel.tinhthanh = getListTinhThanh();
                        myModel.Client = HttpContext.Session.Get<Client>("ssClient");
            return View(myModel);

        }

        [HttpPost]
        [Route("Checkout/Change")]
        public IActionResult Change(Bill bill)
        {
            Bill myModel = new Bill();
            myModel = bill;
            myModel.Giaship = getGiaVC(bill.Client.ThanhPho, bill.Vanchuyen.MaDVVanChuyen);

            List<CartSelect> lstShoppingCart = new List<CartSelect>();

            HttpContext.Session.Set<List<CartSelect>>("ssShoppingCart",null);

            foreach (var pro in bill.Select)
            {
                CartSelect cs = new CartSelect();
                cs.proDisplay = GetProduct(pro.proDisplay.MaSP);
                cs.Soluong = pro.Soluong;

                if (lstShoppingCart == null)
                {
                    lstShoppingCart = new List<CartSelect>();
                }

                lstShoppingCart.Add(cs);
                HttpContext.Session.Set("ssShoppingCart", lstShoppingCart);
            }
            List<CartSelect> ds = HttpContext.Session.Get<List<CartSelect>>("ssShoppingCart");

            myModel.Select = ds;
            myModel.danhsach = getListVC();
            myModel.tinhthanh = getListTinhThanh();
            return View(myModel);
        }

        [HttpPost]
        [Route("Checkout/Submit")]
        public IActionResult Submit(Bill bill)
        {
            Bill myModel = new Bill();
            myModel = bill;
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
               
                using (SqlCommand command = new SqlCommand("ThemDonHang", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    connection.Open();
                    command.Parameters.AddWithValue("@MaKH", DBNull.Value);
                    command.Parameters.AddWithValue("@TenKH", myModel.Client.TenKH);
                    command.Parameters.AddWithValue("@DiaChi", myModel.Client.DiaChi);
                    command.Parameters.AddWithValue("@Quan", myModel.Client.Quan);
                    command.Parameters.AddWithValue("@ThanhPho", myModel.Client.ThanhPho);
                    command.Parameters.AddWithValue("@Email", myModel.Client.Email);
                    command.Parameters.AddWithValue("@SDT", myModel.Client.sdt);
                    command.Parameters.AddWithValue("MaDvVanChuyen", myModel.Vanchuyen.MaDVVanChuyen);
                    command.Parameters.AddWithValue("TongTien", myModel.Price);
                    command.Parameters.AddWithValue("NgayDat", DateTime.Now);
                    command.Parameters.AddWithValue("Trangthai","No");
                    command.ExecuteNonQuery();
                    
                }
                using (SqlCommand command = new SqlCommand("ThemChitietDonHang", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    connection.Open();


                    for (int i = 0; i < myModel.Select.Count; i++)
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("MaSP",myModel.Select[i].proDisplay.MaSP);
                        command.Parameters.AddWithValue("SoLuong", myModel.Select[i].Soluong);
                        command.Parameters.AddWithValue("Gia", myModel.Select[i].proDisplay.Price);
                        command.Parameters.AddWithValue("tenSP", myModel.Select[i].proDisplay.productName);
                        command.ExecuteNonQuery();
                    }
                }
            }
            return View();
        }
    }
}