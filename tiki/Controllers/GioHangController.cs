using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using tiki.Models;

namespace tiki.Controllers
{
    public class GioHangController : Controller
    {
        // Hiển thị giỏ hàng
        public ActionResult Index()
        {
            DataModel db = new DataModel();
            int userId = Convert.ToInt32(Session["IDKH"]); // Lấy ID người dùng từ Session.

            // Thực thi stored procedure và lấy kết quả vào ViewBag.CartItems
            ViewBag.CartItems = db.get("EXEC GetCartItems1 " + userId);
            ViewBag.Discount = 10;
            return View();
        }

        // Thêm sản phẩm vào giỏ hàng
        [HttpPost]
        public ActionResult AddToCart(int productId, int quantity)
        {
            DataModel db = new DataModel();

            // Kiểm tra xem người dùng đã đăng nhập chưa
            if (Session["IDKH"] == null)
            {
                TempData["ErrorMessage"] = "Vui lòng đăng nhập để thêm sản phẩm vào giỏ hàng.";
                return RedirectToAction("GiaoDienDangNhap");
            }

            int userId = Convert.ToInt32(Session["IDKH"]);

            try
            {
                // Thực thi stored procedure để thêm sản phẩm vào giỏ hàng
                db.get($"EXEC AddCartItem {userId}, {productId}, {quantity}");

                // Thông báo thành công
                TempData["SuccessMessage"] = "Sản phẩm đã được thêm vào giỏ hàng thành công!";
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547) // Mã lỗi 547 là lỗi vi phạm khóa ngoại
                {
                    // Thông báo cho người dùng về lỗi sản phẩm không tồn tại
                    TempData["ErrorMessage"] = "Sản phẩm không tồn tại. Vui lòng kiểm tra lại.";
                }
                else
                {
                    // Xử lý các lỗi khác
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi thêm sản phẩm vào giỏ hàng.";
                }
            }

            return RedirectToAction("Index", "GioHang"); // Điều hướng đến trang giỏ hàng sau khi thêm
        }


        // Cập nhật số lượng sản phẩm trong giỏ hàng
        [HttpPost]
        public ActionResult UpdateCart(int productId, int quantity)
        {
            DataModel db = new DataModel();
            int userId = Convert.ToInt32(Session["IDKH"]);

            // Thực thi stored procedure để cập nhật số lượng sản phẩm
            db.get($"EXEC UpdateCartItem {userId}, {productId}, {quantity}");
            
            return RedirectToAction("Index");
        }

        // Xóa sản phẩm khỏi giỏ hàng
        public ActionResult RemoveFromCart(int productId)
        {
            DataModel db = new DataModel();
            int userId = Convert.ToInt32(Session["IDKH"]);

            // Thực thi stored procedure để xóa sản phẩm khỏi giỏ hàng
            db.get($"EXEC RemoveCartItem {userId}, {productId}");

            return RedirectToAction("Index");
        }

        // Áp dụng mã ưu đãi
        [HttpPost]
        public ActionResult ApplyCoupon(string coupon_code)
        {
            DataModel db = new DataModel();
            int userId = Convert.ToInt32(Session["IDKH"]);

            // Gọi stored procedure và lấy kết quả
            var discountList = db.get($"EXEC ApplyCoupon {userId}, '{coupon_code}'");

            // Kiểm tra nếu có kết quả và gán vào ViewBag.Discount
            if (discountList != null && discountList.Count > 0)
            {
                ViewBag.Discount = discountList[0]; // Lấy phần tử đầu tiên nếu có kết quả
            }

            return RedirectToAction("Index");
        }

        // Chuyển đến trang thanh toán
        public ActionResult ThanhToan()
        {
            DataModel db = new DataModel();
            int userId = Convert.ToInt32(Session["IDKH"]);

            // Lấy thông tin giỏ hàng của người dùng khi chuyển đến trang thanh toán
            ViewBag.CartItems = db.get("EXEC GetCartItems " + userId);

            return View();
        }
    }
}
