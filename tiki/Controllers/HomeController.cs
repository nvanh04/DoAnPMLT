using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tiki.Models;

namespace tiki.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            DataModel db = new DataModel();
            ViewBag.listSanPham = db.get("EXEC xuatProducts");
            ViewBag.listDanhMuc = db.get("EXEC xuatDanhMuc");

            return View();
        }
        public ActionResult GiaoDienDangNhap()
        {
            DataModel db = new DataModel();
            ViewBag.listSanPham = db.get("EXEC xuatProducts");
            ViewBag.listDanhMuc = db.get("EXEC xuatDanhMuc");
            return View();
        }
        [HttpPost]
        public ActionResult XulyDangNhap(string username, string password)
        {
            DataModel db = new DataModel();
            ViewBag.list = db.get("EXEC KIEMTRADANGNHAP4 '" + username + "','" + password + "'");

            if (ViewBag.list.Count > 0)
            {
                Session["taikhoan"] = ViewBag.list[0];
                return RedirectToAction("Index", "admin");
            }
            else
                return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult XuLyDangKy(string username, string password, string email, string hoTen)
        {
            DataModel db = new DataModel();

            // Assuming you have a stored procedure to insert new users:
            ViewBag.result = db.get("EXEC KIEMTRADANGKY '" + username + "','" + password + "','" + email + "','" + hoTen + "'");

            if (ViewBag.result.Count > 0)
            {
                // Registration successful, you might want to display a success message or redirect to a confirmation page
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Registration failed, you might want to display error messages or re-render the registration form with error messages
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Danhmuc()
        {
            DataModel db = new DataModel();
            ViewBag.listDanhMuc = db.get("EXEC xuatDanhMuc");
            
            return View();
        }

        public ActionResult chitietsanpham(string id)
        {
            DataModel db = new DataModel();
            ViewBag.listchitietsanpham = db.get("EXEC TIMKIEMProductsID "+id+";");
            ViewBag.listTenCotSanPham = db.get("exec TenCotSanPham");
            ViewBag.listGetProductInfo = db.get("exec GetProductInfo");
           
            return View();
        }

    }
}