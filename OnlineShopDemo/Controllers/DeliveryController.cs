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
    public class DeliveryController : Controller
    {
        public IConfiguration Configuration { get; }

        private readonly IHostingEnvironment _appEnvironment;
        public DeliveryController(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _appEnvironment = hostingEnvironment;
            Configuration = configuration;
        }


        public List<deliverySevicesModel> GetDeliverySevices()
        {
            List<deliverySevicesModel> DS = new List<deliverySevicesModel>();
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
                        deliverySevicesModel Dv = new deliverySevicesModel();
                        Dv.MaDv = Convert.ToInt32(ds.Tables[0].Rows[i]["MaDvVanChuyen"]);
                        Dv.TenDv = ds.Tables[0].Rows[i]["TenDv"].ToString();
                        Dv.GiaNoiThanh = Convert.ToDouble(ds.Tables[0].Rows[i]["GiaNoiThanh"]);
                        Dv.GiaNgoaiThanh = Convert.ToDouble(ds.Tables[0].Rows[i]["GiaNgoaiThanh"]);
                        DS.Add(Dv);
                    }
                }
            }
            return DS;
        }
        public deliverySevicesModel GetDeliverySevice(int ID)
        {
            deliverySevicesModel Dv = new deliverySevicesModel();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("LayDonViVanChuyen", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    command.Parameters.AddWithValue("@MaDv", ID);
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                        Dv.MaDv = Convert.ToInt32(ds.Tables[0].Rows[0]["MaDvVanChuyen"]);
                        Dv.TenDv = ds.Tables[0].Rows[0]["TenDv"].ToString();
                        Dv.GiaNoiThanh = Convert.ToDouble(ds.Tables[0].Rows[0]["GiaNoiThanh"]);
                        Dv.GiaNgoaiThanh = Convert.ToDouble(ds.Tables[0].Rows[0]["GiaNgoaiThanh"]);
                }
            }
            return Dv;
        }

        [Route("Delivery/Index")]
        public IActionResult Index()
        {
            DeliveryViewModel myModel = new DeliveryViewModel();
            myModel.DS = GetDeliverySevices();
            return View(myModel);
        }

        [Route("Delivery/DeliveryDetail/{ID}")]
        public IActionResult DeliveryDetail(int ID)
        {
            DeliveryViewModel myModel = new DeliveryViewModel();
            myModel.dv = GetDeliverySevice(ID);
            return View(myModel);
        }

        [Route("Delivery/DeliveryEdit/{ID}")]
        public IActionResult DeliveryEdit(int ID)
        {
            DeliveryViewModel myModel = new DeliveryViewModel();
            myModel.dv = GetDeliverySevice(ID);
            return View(myModel);
        }

        [HttpPost]
        [Route("Delivery/Edit")]
        public IActionResult Edit(DeliveryViewModel model)
        {
            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("CapNhatDvVanChuyen", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }
                        connection.Open();
                        command.Parameters.AddWithValue("@MaDv", model.dv.MaDv);
                        command.Parameters.AddWithValue("@TenDv", model.dv.TenDv);
                        command.Parameters.AddWithValue("@GiaNoiThanh", model.dv.GiaNoiThanh);
                        command.Parameters.AddWithValue("@GiaNgoaiThanh", model.dv.GiaNgoaiThanh);

                        command.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("DeliveryDetail", "Delivery",new { ID = model.dv.MaDv });
            }
            else
            {
                return RedirectToAction("DeliveryEdit", "Delivery", new { ID = model.dv.MaDv });
            }
        }


        [Route("Delivery/DeliveryDelete/{ID}")]
        public IActionResult DeliveryDelete(int ID)
        {
            DeliveryViewModel myModel = new DeliveryViewModel();
            myModel.dv = GetDeliverySevice(ID);
            return View(myModel);
        }


        [HttpPost]
        [Route("Delivery/Delete")]
        public IActionResult Delete(DeliveryViewModel model)
        {
            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("XoaDvVanChuyen", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }
                        connection.Open();
                        command.Parameters.AddWithValue("@MaDv", model.dv.MaDv);
                     
                        command.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("Index", "Delivery");
            }
            else
            {
                return RedirectToAction("DeliveryDelete", "Delivery", new { ID = model.dv.MaDv });
            }

        }

        [Route("Delivery/DeliveryCreate")]
        public IActionResult DeliveryCreate()
        {
            return View();
        }

        [HttpPost]
        [Route("Delivery/Create")]
        public IActionResult Create(DeliveryViewModel model)
        {
            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("ThemDvVanChuyen", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }
                        connection.Open();
                        command.Parameters.AddWithValue("@TenDv", model.dv.TenDv);
                        command.Parameters.AddWithValue("@GiaNoiThanh", model.dv.GiaNoiThanh);
                        command.Parameters.AddWithValue("@GiaNgoaiThanh", model.dv.GiaNgoaiThanh);
                        command.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("Index", "Delivery");
            }
            else
            {
                return RedirectToAction("DeliveryCreate", "Delivery");
            }

        }



    }
}