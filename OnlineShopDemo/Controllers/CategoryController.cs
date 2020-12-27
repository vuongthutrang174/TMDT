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
    public class CategoryController : Controller
    {
        public IConfiguration Configuration { get; }

        private readonly IHostingEnvironment _appEnvironment;
        public CategoryController(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _appEnvironment = hostingEnvironment;
            Configuration = configuration;
        }


        //Lấy danh sách loại SP
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

        //Lấy một loại SP
        public LoaiSanPham GetCate(int ID)
        {
            LoaiSanPham loaiSP = new LoaiSanPham();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("LayLoaiSP", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }

                    command.Parameters.AddWithValue("@MaLoai",ID);
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    loaiSP.CateID = Convert.ToInt32(ds.Tables[0].Rows[0]["maloai"]);
                    loaiSP.CateName = ds.Tables[0].Rows[0]["tenloai"].ToString();
                }
            }
            return loaiSP;
        }

        [Route("Category/Index")]
        public IActionResult Index()
        {
            CategoryViewModel myModel = new CategoryViewModel();
            myModel.CategoryList = GetCategory();
            return View(myModel);
        }


        [Route("Category/CategoryDetail/{ID}")]
        public IActionResult CategoryDetail(int ID)
        {
            CategoryViewModel myModel = new CategoryViewModel();
            myModel.Category = GetCate(ID);
            return View(myModel);
        }


        [Route("Category/CategoryEdit/{ID}")]
        public IActionResult CategoryEdit(int ID)
        {
            CategoryViewModel myModel = new CategoryViewModel();
            myModel.Category = GetCate(ID);
            return View(myModel);
        }


        [HttpPost]
        [Route("Category/Edit")]
        public IActionResult Edit(CategoryViewModel myModel)
        {
            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("CapNhatLoaiSP", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }

                        connection.Open();
                        command.Parameters.AddWithValue("@MaLoai",myModel.Category.CateID);
                        command.Parameters.AddWithValue("@TenLoai",myModel.Category.CateName);
                        command.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("CategoryDetail", "Category",new{ ID = myModel.Category.CateID });
            }
            else
            {
                return RedirectToAction("CategoryEdit", "Category", new { ID = myModel.Category.CateID });
            }
        }


        [Route("Category/CategoryDelete/{ID}")]
        public IActionResult CategoryDelete(int ID)
        {
            CategoryViewModel myModel = new CategoryViewModel();
            myModel.Category = GetCate(ID);
            return View(myModel);
        }


        [HttpPost]
        [Route("Category/Delete")]
        public IActionResult Delete(CategoryViewModel myModel)
        {
            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("XoaLoaiSP", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }
                        connection.Open();

                        command.Parameters.AddWithValue("@MaLoai", myModel.Category.CateID);
                        command.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("Index", "Category");
            }
            else
            {
                return RedirectToAction("CategoryDelete", "Category", new { ID = myModel.Category.CateID });
            }

        }


        [Route("Category/CategoryCreate")]
        public IActionResult CategoryCreate()
        {
            return View();
        }


        [HttpPost]
        [Route("Category/Create")]
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
                return RedirectToAction("Index", "Category");
            }
            else
            {
                return RedirectToAction("CategoryCreate", "Category");
            }
        }


    }
}