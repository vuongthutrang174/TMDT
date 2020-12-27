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
    public class ProductController : Controller
    {

        public IConfiguration Configuration { get; }

        private readonly IHostingEnvironment _appEnvironment;
        public ProductController(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
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
        private ProEdit GetProduct(int ID)
        {
            ProEdit pro = new ProEdit();

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
        public List<LoaiSanPham> GetCategory()
        {
            List<LoaiSanPham> CateList = new List<LoaiSanPham>();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("LayDSLoaiHang", connection))
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
                        LoaiSanPham loaiSP = new LoaiSanPham();
                        loaiSP.CateID = Convert.ToInt32(ds.Tables[0].Rows[i]["maloai"]);
                        loaiSP.CateName = ds.Tables[0].Rows[i]["tenloai"].ToString();
                        CateList.Add(loaiSP);
                    }
                }
            }
            return CateList;
        }

        [HttpGet]
        [Route("Product/Index")]
        public IActionResult Index()
        {
            ViewProductModel myModel = new ViewProductModel();
            myModel.ProductList = getList();
            myModel.CateList = GetCategory();
            return View(myModel);
        }

        [Route("Product/DetailProduct/{ID}")]
        public IActionResult DetailProduct(int ID)
        {
            ViewProductModel myModel = new ViewProductModel();
            myModel.proEdit = GetProduct(ID);
            myModel.CateList = GetCategory();
            myModel.Soluong = GetInfoWareHouse(ID);
            return View(myModel);
        }


        [HttpPost]
        [Route("Product/Create")]
        public async Task<IActionResult> Create(ViewProductModel model, IFormFile pic)
        {
            ViewProductModel myModel = new ViewProductModel();

            if (ModelState.IsValid)
            {
                if (pic != null)
                {
                    var file = pic;
                    //There is an error here
                    var uploads = Path.Combine(_appEnvironment.WebRootPath, "Images");
                    var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                        string filePath = "Images\\" + fileName;
                        model.product.Image = filePath;
                    }
                    string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand("ThemSP", connection))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;

                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }

                            connection.Open();

                            int MaLoai = Convert.ToInt32(model.product.ProductType);
                            double Gia = Convert.ToDouble(model.product.Price);

                            command.Parameters.AddWithValue("@TenSP", model.product.productName);
                            command.Parameters.AddWithValue("@MoTa", model.product.Description);
                            command.Parameters.AddWithValue("@MaLoai", MaLoai);
                            command.Parameters.AddWithValue("@Gia", Gia);
                            command.Parameters.AddWithValue("@MauSac", model.product.color);
                            command.Parameters.AddWithValue("@hinhanh", model.product.Image);
                            command.Parameters.AddWithValue("@Modify", DateTime.Now);
                            command.ExecuteNonQuery();

                            return RedirectToAction("Index", "Product");
                        }
                    }

                }
                else
                {

                    return RedirectToAction("Index", "Product");
                }
            }
            else
            {

                return RedirectToAction("Index", "Product");
            }
        }

        [HttpGet]
        [Route("Product/EditProduct/{ID}")]
        public IActionResult EditProduct(int ID)
        {
            ViewProductModel myModel = new ViewProductModel();
            myModel.proEdit = GetProduct(ID);
            myModel.CateList = GetCategory();
            return View(myModel);
        }

        [HttpPost]
        [Route("Product/Edit")]
        public async Task<IActionResult> Edit(ProEdit proEdit, IFormFile pic)
        {
            ViewProductModel myModel = new ViewProductModel();
            myModel.proEdit = proEdit;
            myModel.CateList = GetCategory();
            if (ModelState.IsValid)
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
                        proEdit.Image = filePath;
                    }

                    string connectionString = Configuration["ConnectionStrings:DefaultConnection"];

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand("CapNhatSP", connection))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;

                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }

                            connection.Open();

                            command.Parameters.AddWithValue("@MaSP", proEdit.MaSP);
                            command.Parameters.AddWithValue("@TenSP", proEdit.productName);
                            command.Parameters.AddWithValue("@MoTa", proEdit.Description);
                            command.Parameters.AddWithValue("@MaLoai", proEdit.productType);
                            command.Parameters.AddWithValue("@TrangThai", proEdit.Stock);
                            command.Parameters.AddWithValue("@Gia", proEdit.Price);
                            command.Parameters.AddWithValue("@MauSac", proEdit.Color);
                            command.Parameters.AddWithValue("@hinhanh", proEdit.Image);
                            command.Parameters.AddWithValue("@Modify", DateTime.Now);
                            command.ExecuteNonQuery();
                        }
                    }
                    return RedirectToAction("DetailProduct", "Product", new { id = Convert.ToInt32(proEdit.MaSP) });
                }
                else
                {
                    string connectionString = Configuration["ConnectionStrings:DefaultConnection"];

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand("CapNhatSP", connection))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;

                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();

                            int MaLoai = Convert.ToInt32(proEdit.productType);
                            double Gia = Convert.ToDouble(proEdit.Price);

                            command.Parameters.AddWithValue("@MaSP", proEdit.MaSP);
                            command.Parameters.AddWithValue("@TenSP", proEdit.productName);
                            command.Parameters.AddWithValue("@MoTa", proEdit.Description);
                            command.Parameters.AddWithValue("@MaLoai", proEdit.productType);
                            command.Parameters.AddWithValue("@TrangThai", proEdit.Stock);
                            command.Parameters.AddWithValue("@Gia", proEdit.Price);
                            command.Parameters.AddWithValue("@MauSac", proEdit.Color);
                            command.Parameters.AddWithValue("@hinhanh", proEdit.Image);
                            command.Parameters.AddWithValue("@Modify", DateTime.Now);
                            command.ExecuteNonQuery();
                        }
                    }
                    return RedirectToAction("DetailProduct", "Product", new { id = Convert.ToInt32(proEdit.MaSP) });
                }
            }
            return RedirectToAction("EditProduct", "Product", new { id = Convert.ToInt32(proEdit.MaSP) });
        }

        [HttpGet]
        [Route("DashBoard/DeleteProduct/{ID:int}")]
        public IActionResult DeleteProduct(int ID)
        {
            ViewProductModel myModel = new ViewProductModel();
            myModel.proEdit = GetProduct(ID);
            myModel.CateList = GetCategory();
            return View(myModel);
        }

        [HttpPost]
        [Route("Product/Delete")]
        public IActionResult Delete(ProEdit proEdit)
        {
            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("XoaSP", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }

                        connection.Open();
                        command.Parameters.AddWithValue("@MaSP", proEdit.MaSP);
                        command.ExecuteNonQuery();

                    }
                }
                return RedirectToAction("Index", "Product");
            }
            else
            {
                return View();
            }


        }

        private warehouseModel GetInfoWareHouse(int ID)
        {
            warehouseModel sl = new warehouseModel();
            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DefaultConnection"];

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("LaySoLuongSP", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }

                        connection.Open();
                        command.Parameters.AddWithValue("@Masp", ID);
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            sl.MaSp = Convert.ToInt32(ds.Tables[0].Rows[i]["MaSp"]);
                            sl.TenSP = ds.Tables[0].Rows[i]["tenSP"].ToString();
                            sl.soluong = Convert.ToInt32(ds.Tables[0].Rows[i]["SoLuong"]);
                        }
                    }
                }
            }
            return sl;
        }

        [Route("Product/WareHouse/{ID}")]
        public IActionResult WareHouse(int ID)
        {
            warehouseModel myModel = new warehouseModel();
            myModel = GetInfoWareHouse(ID);
            return View(myModel);
        } 


        [Route("Product/WareHouseUpdate")]
        public IActionResult WareHouseUpdate(warehouseModel myModel)
        {
            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DefaultConnection"];

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("CapNhatSoLuong", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }
                        connection.Open();

                        command.Parameters.AddWithValue("@Masp", myModel.MaSp);
                        command.Parameters.AddWithValue("@SoLuong", myModel.soluong);
                        command.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("DetailProduct", "Product", new { id = Convert.ToInt32(myModel.MaSp) });
            }
            return View(myModel);
        }

    }
}