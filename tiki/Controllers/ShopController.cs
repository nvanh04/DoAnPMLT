﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tiki.Models;

namespace tiki.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop

        public ActionResult shop()
        {
            DataModel db = new DataModel();
            int userId = Convert.ToInt32(Session["IDKH"]);
            ViewBag.listDanhMuc = db.get("SELECT * FROM Categories");
            ViewBag.listnhacungcap = db.get("SELECT * FROM Brands");
            return View();
        }
        [HttpPost]
        public ActionResult themsanpham2(string name, string description, string price, HttpPostedFileBase HINH, string category_id, string brand_id, string stock_quantity, string price_discount)
        {
            try
            {
                // Kiểm tra nếu `user_id` tồn tại trong session
                if (Session["IDKH"] != null)
                {

                    int userId = Convert.ToInt32(Session["IDKH"]); // Lấy `user_id` từ session

                    // Kiểm tra xem có hình ảnh không
                    if (HINH != null && HINH.ContentLength > 0)
                    {
                        // Lưu ảnh vào thư mục
                        string filename = Path.GetFileName(HINH.FileName);
                        string path = Path.Combine(Server.MapPath("~/HINH"), filename);
                        HINH.SaveAs(path);

                        // Tạo câu lệnh SQL với `user_id`
                        DataModel db = new DataModel();
                        string query = "EXEC THEMSP222 N'" + name + "', N'" + description + "', " + price + ", '" + filename + "', " + category_id + ", " + brand_id + ", " + stock_quantity + ", " + price_discount + ", " + userId + ";";

                        // Thực hiện truy vấn để thêm sản phẩm
                        ViewBag.list = db.get(query);
                    }
                }
                else
                {
                    // Nếu không có `user_id` trong session, chuyển hướng tới trang đăng nhập
                    return RedirectToAction("GiaoDienDangNhap", "Home");
                }
            }
            catch (Exception)
            {

            }

            // Chuyển hướng về trang quản lý sản phẩm sau khi thêm thành công
            return RedirectToAction("shoplistproduct", "Shop");
        }
        [HttpPost]
        public ActionResult suasp(string name, string description, string price, string stock_quantity, string category_id, string brand_id, HttpPostedFileBase HINH, string price_discount,string product_id)
        {
            try
            {
                // Kiểm tra nếu `user_id` tồn tại trong session
                if (Session["IDKH"] != null)
                {

                    //int userId = Convert.ToInt32(Session["IDKH"]); // Lấy `user_id` từ session

                    // Kiểm tra xem có hình ảnh không
                    if (HINH != null && HINH.ContentLength > 0)
                    {
                        // Lưu ảnh vào thư mục
                        string filename = Path.GetFileName(HINH.FileName);
                        string path = Path.Combine(Server.MapPath("~/HINH"), filename);
                        HINH.SaveAs(path);

                        // Tạo câu lệnh SQL với `user_id`
                        DataModel db = new DataModel();
                        string query = "EXEC SUAPRODUCT2 N'" + name + "', N'" + description + "', " + price + ", " + stock_quantity + ", " + category_id + ", " + brand_id + ",'" + filename + "', " + price_discount + ", " + product_id + ";";

                        // Thực hiện truy vấn để thêm sản phẩm
                        ViewBag.listsua = db.get(query);
                    }
                }
                else
                {
                    // Nếu không có `user_id` trong session, chuyển hướng tới trang đăng nhập
                    return RedirectToAction("GiaoDienDangNhap", "Home");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }

            // Chuyển hướng về trang quản lý sản phẩm sau khi thêm thành công
            return RedirectToAction("shoplistproduct", "Shop");
        }
        public ActionResult suasanpham(string id)
        {
            DataModel db = new DataModel();
            ViewBag.listchitietsanpham = db.get("EXEC TIMKIEMProductsID " + id + ";");
            ViewBag.listSanpham = db.get("SELECT * FROM Products" );
            ViewBag.listDanhMuc = db.get("SELECT * FROM Categories");
            ViewBag.listnhacungcap = db.get("SELECT * FROM Brands");
            int userId = Convert.ToInt32(Session["IDKH"]);
            ViewBag.list = db.get($"EXEC LayTTKH1 {userId}");
            ViewBag.hienthisanphamid = db.get($"exec LAY_SANPHAM_THEO_USERID1 {userId}");
            return View();
        }
        public ActionResult xoasanpham(string id)
        {
            DataModel db = new DataModel();
            ViewBag.list = db.get("EXEC XOASANPHAMTHEOID " + id);
            return RedirectToAction("shoplistproduct", "Shop");
        }
        public ActionResult shopreview()
        {
            return View();
        }
        public ActionResult shoporderdetails(string id)
        {
            DataModel db = new DataModel();
            int userId = Convert.ToInt32(Session["IDKH"]);
            ViewBag.list = db.get($"EXEC LayTTKH1 {userId}");
            ViewBag.listTongDH = db.get($"EXEC TongDH {userId}");
            ViewBag.LayTTDH = db.get($"EXEC LayTTDH {userId}, {id}");
            ViewBag.ShopDH = db.get($"EXEC ShopDH {id}");
            ViewBag.ShopCTDH = db.get($"EXEC ShopCTDH {id}");
            return View();
        }
        public ActionResult shoporderlist()
        {
            DataModel db = new DataModel();
            int userId = Convert.ToInt32(Session["IDKH"]);
            ViewBag.list = db.get($"EXEC LayTTKH1 {userId}");
            ViewBag.XuatDanhSachDH = db.get($"EXEC GetSellerOrders {userId}");
            if (ViewBag.XuatDanhSachDH != null || ViewBag.XuatDanhSachDH.Count > 0)
            {
                return View();
            }
            else
            {
                TempData["XuatDanhSachDHTB"] = "Không có đơn hàng nào!";
                return View();
            }
        
        }
        public ActionResult category()
        {
            DataModel db = new DataModel();
            ViewBag.listxuatdanhmuc = db.get("SELECT * FROM Categories");
            return View();
        }
        public ActionResult shoplistproduct()
        {
            DataModel db = new DataModel();
            ViewBag.listSanpham = db.get("SELECT * FROM Products");
            ViewBag.listDanhMuc = db.get("SELECT * FROM Categories");
            int userId = Convert.ToInt32(Session["IDKH"]);
            ViewBag.list = db.get($"EXEC LayTTKH1 {userId}");
            ViewBag.hienthisanphamid = db.get($"exec LAY_SANPHAM_THEO_USERID1 {userId}");
            return View();
        }
        public ActionResult RemoveOrder(string orderId)
        {
            DataModel db = new DataModel();
            int userId = Convert.ToInt32(Session["IDKH"]);
            ViewBag.LayTTDH = db.get($"EXEC LayTTDH {userId}, {orderId}");
            // Thực thi stored procedure để xóa sản phẩm khỏi giỏ hàng
            db.get($"EXEC XoaDHNBH {userId}, {orderId}");

            return RedirectToAction("shoporderlist");
            
        }


    }
}