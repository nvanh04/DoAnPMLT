using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using tiki.Models;
namespace tiki.Controllers
{
    public class adminController : Controller
    {
        // GET: admin
        public ActionResult Index()
        {
            DataModel db = new DataModel();
            //if (Session["taikhoan"] == null)
            //{
            //    return RedirectToAction("GiaoDienDangNhap", "Home");
            //}

            return View();

        }
        public ActionResult eCommerce()
        {
            return View();
        }
        public ActionResult product()
        {
            DataModel db = new DataModel();
            ViewBag.listSanpham = db.get("SELECT * FROM Products");
            ViewBag.listDanhMuc = db.get("SELECT * FROM Categories");
            return View();
        }
        public ActionResult addproduct()
        {
            DataModel db = new DataModel();
            ViewBag.listDanhMuc = db.get("SELECT * FROM Categories");
            ViewBag.listnhacungcap = db.get("SELECT * FROM Brands");



            return View();
        }
        [HttpPost]
        public ActionResult themsanpham(string name, string description, string price, HttpPostedFileBase HINH, string category_id, string brand_id, string stock_quantity, string price_discount)
        {
            try
            {
                if (HINH != null && HINH.ContentLength > 0)
                {
                    string filename = Path.GetFileName(HINH.FileName);
                    string path = Path.Combine(Server.MapPath("~/HINH"), filename);
                    HINH.SaveAs(path);

                    DataModel db = new DataModel();
                    ViewBag.list = db.get("EXEC THEMSP2 N'" + name + "', '" + description + "', " + price + ", '" + HINH.FileName + "', " + category_id + ", " + brand_id + ", " + stock_quantity + ", " + price_discount + ";");
                }
            }
            catch (Exception) { }
            //return View("addproduct");
            return RedirectToAction("product", "admin");
        }

        public ActionResult categoryproduct()
        {
            DataModel db = new DataModel();
            ViewBag.listxuatdanhmuc = db.get("SELECT * FROM Categories");
            return View();
        }
        public ActionResult Xoadanhmuc(string id)
        {
            DataModel db = new DataModel();
            ViewBag.listxoasanpham = db.get("EXEC XOADANHMUCTHEOID " + id);
            return RedirectToAction("categoryproduct", "admin");
        }
        [HttpPost]
        public ActionResult themdanhmuc(string category_name)
        {
            DataModel db = new DataModel();
            ViewBag.list = db.get("EXEC THEMLOAISP '" + category_name + "'");

            return RedirectToAction("categoryproduct", "admin");
        }
        public ActionResult OrderDetails()
        {
            return View();
        }
        public ActionResult OrderList()
        {
            return View();
        }
        public ActionResult Reviews()
        {
            return View();
        }

        //Phân quyền
        public ActionResult Create()
        {
            DataModel db = new DataModel();
            return View();
        }
        [HttpPost]
        public ActionResult XuLyDangKyadmin(string username, string VaiTro, string email, string password_hash, string full_name, string phone_number, string address)
        {
            DataModel db = new DataModel();

            // Assuming you have a stored procedure to insert new users:
            ViewBag.result = db.get("EXEC AddNBH1 '" + username + "', '" + VaiTro + "','" + email + "','" + password_hash + "','" + full_name + "','"+phone_number+"', '"+address+"'");

            if (ViewBag.result.Count > 0)
            {
                return RedirectToAction("GiaoDienDangNhap", "homne");
            }
            else
            {
                // Registration failed, you might want to display error messages or re-render the registration form with error messages
                return RedirectToAction("Index", "Home");
            }
        }
        
        public ActionResult Xoanguoidung(string id)
        {
            DataModel db = new DataModel();
            ViewBag.list = db.get("EXEC XOANGUOIDUNGTHEOID " + id);
            return RedirectToAction("Index", "Customers");

        }

    }
}