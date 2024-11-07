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
    public class GioHangController : Controller
    {
        // Hiển thị giỏ hàng
        public ActionResult Index()
        {
            DataModel db = new DataModel();
            int userId = Convert.ToInt32(Session["IDKH"]); // Lấy ID người dùng từ Session.

            // Thực thi stored procedure với UserId trực tiếp trong chuỗi truy vấn
            ViewBag.CartItems = db.get("EXEC GetCartItems " + userId);

            return View();
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
