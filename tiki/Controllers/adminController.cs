using System;
using System.Collections.Generic;
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
        public ActionResult addproduct()
        {
            DataModel db = new DataModel();
            return View();
        }
        [HttpPost]
        public ActionResult ThemSP(string ten, string mota, string gia, string hinh, string madanhmuc, string mathuonghieu, string soluongton, string giagiam)
        {
            DataModel db = new DataModel();
            ViewBag.list = db.get("EXEC THEMSP2 N'" + ten + "','" + mota + "', " + gia + ", '" + hinh + "', " + madanhmuc + ", " + mathuonghieu + ", " + soluongton + ", " + giagiam + ";");

            return RedirectToAction("addproduct", "admin");
        }

        public ActionResult categoryproduct()
        {
            return View();
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
            ViewBag.result = db.get("EXEC CreateAdmin '" + username + "', '" + VaiTro + "','" + email + "','" + password_hash + "','" + full_name + "','"+phone_number+"', '"+address+"'");

            if (ViewBag.result.Count > 0)
            {
                // Registration successful, you might want to display a success message or redirect to a confirmation page
                return RedirectToAction("Index", "admin");
            }
            else
            {
                // Registration failed, you might want to display error messages or re-render the registration form with error messages
                return RedirectToAction("Index", "Home");
            }
        }

    }
}