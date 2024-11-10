using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using tiki.Models;

namespace tiki.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string id)
        {
            DataModel db = new DataModel();
            int userId = Convert.ToInt32(Session["IDKH"]);
            ViewBag.listDemSanPham = db.get($"exec DemSP {userId}");
            ArrayList DemSP = (ArrayList)ViewBag.listDemSanPham[0];

            Session["DemSP"] = DemSP[0];
            ViewBag.listSanPham = db.get("EXEC xuatProducts");
            ViewBag.listDanhMuc = db.get("EXEC xuatDanhMuc");
            ViewBag.listSPtheoID = db.get("EXEC SPtheoID'" + id + "'");
            return View();
        }
        public ActionResult GiaoDienDangNhap()
        {
            DataModel db = new DataModel();
            return View();
        }
        [HttpPost]
        public ActionResult XulyDangNhap(string email, string password)
        {
            DataModel db = new DataModel();
            //ViewBag.listCheckAdmin = db.get("EXEC CheckAdmin '" + email + "','" + password + "'");
            //ViewBag.listCheckNBH = db.get("EXEC CheckNBH1 '" + email + "','" + password + "'");
            ViewBag.list = db.get("EXEC KIEMTRADANGNHAP5 '" + email + "','" + password + "'");

            //// Nếu là Admin
            //if (ViewBag.listCheckAdmin != null && ViewBag.listCheckAdmin.Count > 0)
            //{
            //    ArrayList adminData = (ArrayList)ViewBag.listCheckAdmin[0];
            //    Session["IDAdmin"] = adminData[0];
            //    Session["TenAdmin"] = adminData[1];
            //    Session["VaiTro"] = adminData[2];
            //    return RedirectToAction("Index", "admin");
            //}

            //// Nếu là Nhà bán hàng (NBH)
            //if (ViewBag.listCheckNBH != null && ViewBag.listCheckNBH.Count > 0)
            //{
            //    ArrayList nbhData = (ArrayList)ViewBag.listCheckNBH[0];
            //    Session["IDNBH"] = nbhData[0];
            //    Session["TenNBH"] = nbhData[1];
            //    Session["MatKhauNBH"] = nbhData[2];
            //    return RedirectToAction("shop", "Shop");
            //}

            // Nếu là khách hàng (KH)
            if (ViewBag.list != null && ViewBag.list.Count > 0)
            {
                ArrayList userData = (ArrayList)ViewBag.list[0];
                Session["IDKH"] = userData[0];
                Session["VaiTro"] = userData[9];
                Session["TenKH"] = userData[1];
                Session["SoDTKH"] = userData[5];
                return RedirectToAction("Index", "Home");
            }
            // Nếu không tìm thấy tài khoản
            else { return RedirectToAction("GiaoDienDangNhap"); }
            // Nếu không tìm thấy tài khoản
            
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

        public ActionResult Danhmuc(string id)
        {
            DataModel db = new DataModel();
            ViewBag.listDanhMuc = db.get("EXEC xuatDanhMuc");
            ViewBag.listSPtheoID = db.get("EXEC GetProductsAndCategoryNameById'" + id+"'");
            ViewBag.categoryName = db.get("EXEC GetCategoryNameById '" + id + "'");
            ViewBag.listGetCategoryNameById = db.get("EXEC GetCategoryNameById'" + id + "'");


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
        public ActionResult TimKiemTenSP(string tensp, string id)
        {
            DataModel db = new DataModel();
            ViewBag.listSPtheoID = db.get("EXEC GetProductsAndCategoryNameById'" + id + "'");
            ViewBag.list = db.get("EXEC TIMKIEMTHEOTEN '" + tensp + "'");
            return View();
        }
        public ActionResult UpdateUserRoleToNBH()
        {
            DataModel db = new DataModel();
            int userId = Convert.ToInt32(Session["IDKH"]);
            ViewBag.UpdateUserRoleToNBH = db.get($"EXEC UpdateUserRoleToNBH {userId}");
            return RedirectToAction("shop", "Shop");
        }
        public ActionResult Theodoidonhang()
        {
            DataModel db = new DataModel();
            return View();
        }
        public ActionResult Thongtincanhan()
        {
            DataModel db = new DataModel();
            return View();
        }
    }
}