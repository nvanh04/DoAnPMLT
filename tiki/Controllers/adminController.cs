using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("GiaoDienDangNhap", "Home");
            }

            return View();

        }
        public ActionResult eCommerce()
        {
            return View();
        }
        public ActionResult product()
        {
            return View();
        }

        ////[HttpPost]
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
            return View();
        }
        [HttpPost]
        public ActionResult themdanhmuc(string category_name)
        {
            DataModel db = new DataModel();
            ViewBag.list = db.get("EXEC THEMDANHMUC1 N'" + category_name + "';");

            return RedirectToAction("OrderDetails", "admin");
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
    }
}