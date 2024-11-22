using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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

            ViewBag.list = db.get($"EXEC LayTTKH1 {userId}");

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
            ViewBag.list = db.get("EXEC KIEMTRADANGNHAP5 '" + email + "','" + password + "'");
            if (ViewBag.list != null && ViewBag.list.Count > 0)
            {
                ArrayList userData = (ArrayList)ViewBag.list[0];
                Session["IDKH"] = userData[0];
                Session["VaiTro"] = userData[9];
                Session["TenKH"] = userData[1];
                Session["SoDTKH"] = userData[5];
                Session["Hinh"] = userData[10];

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

            // Kiểm tra xem tài khoản đã tồn tại hay chưa
            var checkUser = db.get($"EXEC KIEMTRATAIKHOAN '{username}', '{email}'");

            if (checkUser.Count > 0)
            {
                // Tài khoản đã tồn tại, trả về thông báo lỗi
                TempData["Error"] = "Tài khoản đã tồn tại. Vui lòng sử dụng tài khoản khác.";
                return RedirectToAction("GiaoDienDangNhap", "Home"); // Hoặc quay lại form đăng ký
            }

            // Nếu tài khoản chưa tồn tại, tiến hành đăng ký
            var result = db.get($"EXEC KIEMTRADANGKY '{username}', '{password}', '{email}', '{hoTen}'");

            if (result.Count > 0)
            {
                // Đăng ký thành công
                return RedirectToAction("GiaoDienDangNhap", "Home");
            }
            else
            {
                // Đăng ký thất bại
                return RedirectToAction("GiaoDienDangNhap", "Home");
            }
        }


        //[HttpPost]
        //public ActionResult XuLyDangKy(string username, string password, string email, string hoTen)
        //{
        //    DataModel db = new DataModel();

        //    // Assuming you have a stored procedure to insert new users:
        //    ViewBag.result = db.get("EXEC KIEMTRADANGKY '" + username + "','" + password + "','" + email + "','" + hoTen + "'");

        //    if (ViewBag.result.Count > 0)
        //    {
        //        // Registration successful, you might want to display a success message or redirect to a confirmation page
        //        return RedirectToAction("GiaoDienDangNhap", "Home");
        //    }
        //    else
        //    {
        //        // Registration failed, you might want to display error messages or re-render the registration form with error messages
        //        return RedirectToAction("Index", "Home");
        //    }
        //}

        public ActionResult Danhmuc(string id)
        {
            DataModel db = new DataModel();
            int userId = Convert.ToInt32(Session["IDKH"]);

            ViewBag.listTTKH = db.get($"EXEC LayTTKH1 {userId}");
            ViewBag.listDanhMuc = db.get("EXEC xuatDanhMuc");
            ViewBag.listSPtheoID = db.get("EXEC GetProductsAndCategoryNameById'" + id+"'");
            ViewBag.categoryName = db.get("EXEC GetCategoryNameById '" + id + "'");
            ViewBag.listGetCategoryNameById = db.get("EXEC GetCategoryNameById'" + id + "'");


            return View();
        }

        public ActionResult chitietsanpham(string id)
        {
            DataModel db = new DataModel();
            int userId = Convert.ToInt32(Session["IDKH"]);

            ViewBag.listTTKH = db.get($"EXEC LayTTKH1 {userId}");
            ViewBag.laythongtintheoidproduct = db.get("EXEC LAY_THONGTIN_USER_THEO_PRODUCT " + id + ";");
            ViewBag.listchitietsanpham = db.get("EXEC TIMKIEMProductsID " + id + ";");
            ViewBag.listTenCotSanPham = db.get("exec TenCotSanPham");
            ViewBag.listGetProductInfo = db.get("exec GetProductInfo");

            return View();
        }
        public ActionResult TimKiemTenSP(string tensp, string id)
        {
            DataModel db = new DataModel();
            int userId = Convert.ToInt32(Session["IDKH"]);

            ViewBag.listTTKH = db.get($"EXEC LayTTKH1 {userId}");
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
            int userId = Convert.ToInt32(Session["IDKH"]);

            ViewBag.listTTKH = db.get($"EXEC LayTTKH1 {userId}");
            return View();
        }
        public ActionResult Thongtincanhan()
        {
            DataModel db = new DataModel();
            int userId = Convert.ToInt32(Session["IDKH"]);
            ViewBag.listTTKH = db.get($"EXEC LayTTKH1 {userId}");
            ViewBag.list = db.get($"EXEC LayTTKH1 {userId}");

            return View();
        }
        public ActionResult SuaThongTinCaNhan()
        {
            DataModel db = new DataModel();
            int userId = Convert.ToInt32(Session["IDKH"]);
            ViewBag.listTTKH = db.get($"EXEC LayTTKH1 {userId}");
            ViewBag.list = db.get($"EXEC LayTTKH1 {userId}");

            return View();
        }
        [HttpPost]
        public ActionResult XuLySuaThongTinCaNhan(string username, string fullname, string email, HttpPostedFileBase hinh, string phone, string DiaChi)
        {
            try
            {
                if (hinh != null && hinh.ContentLength > 0)
                {
                    DataModel db = new DataModel();
                    int userId = Convert.ToInt32(Session["IDKH"]);
                    ViewBag.listTTKH = db.get($"EXEC LayTTKH1 {userId}");
                    ViewBag.list = db.get($"EXEC LayTTKH1 {userId}");
                    string filename = Path.GetFileName(hinh.FileName);
                    string path = Path.Combine(Server.MapPath("~/HINH"), filename);
                    hinh.SaveAs(path);


                    db.get("EXEC SuaThongTinCaNhan2 " + userId + ", " + username + ", '" + email + "', N'" + fullname + "', " + phone + ", N'" + DiaChi + "','" + hinh.FileName + "'");

                }

            }
            catch (Exception) { }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult XemDonHang()
        {
            DataModel db = new DataModel();
            int userId = Convert.ToInt32(Session["IDKH"]);

            ViewBag.listTTKH = db.get($"EXEC LayTTKH1 {userId}");
            ViewBag.XuatDH = db.get($"exec XuatDonHang {userId}");
            ViewBag.XuatCTDH = db.get($"exec XuatChiTietDonHang {userId}");
            return View();
        }
        public ActionResult XemDanhSachDH_User()
        {
            DataModel db = new DataModel();
            int userId = Convert.ToInt32(Session["IDKH"]);

            ViewBag.listTTKH = db.get($"EXEC LayTTKH1 {userId}");
            // Lấy danh sách đơn hàng cho người dùng
            ViewBag.XemDanhSachDH = db.get($"exec XemDanhSachDH {userId}");

            return View();
        }
        public ActionResult XemDanhSachDHCT_User(string id)
        {
            DataModel db = new DataModel();
            int userId = Convert.ToInt32(Session["IDKH"]);

            ViewBag.listTTKH = db.get($"EXEC LayTTKH1 {userId}");
            // Lấy thông tin đơn hàng của người dùng
            ViewBag.XemCTDH = db.get($"exec XemCTDH {userId}, {id}");

            // Lấy chi tiết đơn hàng
            ViewBag.XemDanhSachDHCT = db.get($"exec XemDanhSachDHCT {userId}, {id}");
          
            return View(); ;
        }
        public ActionResult XoaDH_User(string id)
        {
            DataModel db = new DataModel();
            int userId = Convert.ToInt32(Session["IDKH"]);
            db.get($"exec XoaDH {userId}, {id}");
            return RedirectToAction("XemDanhSachDH_User");
        }
        public ActionResult UpdateDH(string id)
        {
            DataModel db = new DataModel();
            int userId = Convert.ToInt32(Session["IDKH"]);

            ViewBag.listTTKH = db.get($"EXEC LayTTKH1 {userId}");
            ViewBag.XemTTDonHang = db.get($"exec XemCTDH '{userId}', '{id}'");
            return View();
        }
        [HttpPost]
        public ActionResult XuLyUpdateDH(string id, string billing_name, string billing_address_1, string billing_tel, string billing_email, string order_comments)
        {
            DataModel db = new DataModel();
            int userId = Convert.ToInt32(Session["IDKH"]);

            ViewBag.listTTKH = db.get($"EXEC LayTTKH1 {userId}");
            ViewBag.XemTTDonHang = db.get($"exec XemCTDH '{userId}', '{id}'");
            db.get("exec CapNhatDH "+userId+", "+id+", N'"+billing_name+"', '"+billing_tel+"', N'"+billing_address_1+"', '"+billing_email+"', N'"+order_comments+"'");
            return RedirectToAction("XemDanhSachDH_User", "Home");
        }
        public ActionResult chitietnbh(string id)
        {
            DataModel db = new DataModel();
             int userId = Convert.ToInt32(Session["IDKH"]);

            ViewBag.listTTKH = db.get($"EXEC LayTTKH1 {userId}");
            ViewBag.list = db.get("EXEC TIMKIEMNBHTHEOID " + id + ";");
            ViewBag.hienthisanphamid2 = db.get($"exec LAY_SANPHAM_THEO_USERID1 {id}");
            ViewBag.LayThongTinNguoiDung = db.get("EXEC LayThongTinNguoiDung " + id + ";");
            return View();
        }
    }
} 