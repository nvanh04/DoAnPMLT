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
            int userId = Convert.ToInt32(Session["IDKH"]);
            ViewBag.listDemSanPham = db.get($"exec DemSP {userId}");
            // Gọi stored procedure để lấy thông tin giỏ hàng
            ViewBag.CartItems = db.get($"EXEC GetCartItems1 {userId}");

            // Gọi stored procedure để lấy giá trị CartTotal và gán trực tiếp vào ViewBag và Session
            ViewBag.cartTotalResult = db.get($"EXEC CalculateCartTotal {userId}");
            decimal cartTotal = ViewBag.cartTotalResult != null && ViewBag.cartTotalResult.Count > 0
                                ? Convert.ToDecimal(ViewBag.cartTotalResult[0][0])
                                : 0;

            ViewBag.CartTotal = cartTotal;
            Session["CartTotal"] = cartTotal;

            // Thiết lập các giá trị khác cho ViewBag, lấy từ TempData
            ViewBag.Discount = TempData["Discount"] ?? 0;
            ViewBag.TotalAfterDiscount = cartTotal - (decimal)ViewBag.Discount;
            ViewBag.CouponError = TempData["CouponError"];
            ViewBag.CouponSuccess = TempData["CouponSuccess"];

            return View();
        }

        // Áp dụng mã ưu đãi
        [HttpPost]
        public ActionResult ApplyCoupon(string coupon_code)
        {
            DataModel db = new DataModel();

            // Kiểm tra xem người dùng đã đăng nhập chưa
            if (Session["IDKH"] == null)
            {
                TempData["ErrorMessage"] = "Vui lòng đăng nhập để áp dụng mã giảm giá.";
                return RedirectToAction("GiaoDienDangNhap");
            }

            int userId = Convert.ToInt32(Session["IDKH"]);

            // Lấy giá trị tạm tính (CartTotal) từ Session
            decimal cartTotal = Session["CartTotal"] != null ? Convert.ToDecimal(Session["CartTotal"]) : 0;
            decimal discount = 0;

            try
            {
                // Gọi stored procedure ApplyCoupon để lấy giá trị giảm giá dựa trên mã giảm giá
                var discountList = db.get($"EXEC ApplyCoupon {userId}, '{coupon_code}'") as ArrayList;

                // Kiểm tra discountList và lấy giá trị DiscountAmount nếu có
                if (discountList != null && discountList.Count > 0 && discountList[0] is Dictionary<string, object> row)
                {
                    // Lấy giá trị giảm giá từ cột DiscountAmount
                    if (row.ContainsKey("DiscountAmount") && decimal.TryParse(row["DiscountAmount"]?.ToString(), out discount) && discount > 0)
                    {
                        TempData["CouponSuccess"] = $"Mã giảm giá đã được áp dụng. Giảm giá: {discount:N0}₫.";
                    }
                    else
                    {
                        TempData["CouponError"] = "Mã giảm giá không hợp lệ hoặc không đáp ứng điều kiện.";
                    }
                }
                else
                {
                    TempData["CouponError"] = "Mã giảm giá không hợp lệ hoặc không đáp ứng điều kiện.";
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547) // Mã lỗi 547 là lỗi vi phạm khóa ngoại hoặc mã không tồn tại
                {
                    TempData["ErrorMessage"] = "Mã giảm giá không hợp lệ. Vui lòng kiểm tra lại.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi áp dụng mã giảm giá.";
                }
            }

            // Tính tổng tiền sau khi áp dụng mã giảm giá
            decimal totalAfterDiscount = cartTotal - discount;

            // Cập nhật lại các giá trị Session và TempData
            Session["CartTotal"] = cartTotal;
            TempData["Discount"] = discount;
            TempData["TotalAfterDiscount"] = totalAfterDiscount;

            return RedirectToAction("Index");
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
        public ActionResult UpdateCart(List<int> productIds, List<int> quantities)
        {
            DataModel db = new DataModel();
            int userId = Convert.ToInt32(Session["IDKH"]);

            // Duyệt qua từng sản phẩm và cập nhật số lượng
            for (int i = 0; i < productIds.Count; i++)
            {
                int productId = productIds[i];
                int quantity = quantities[i];

                // Thực thi stored procedure để cập nhật số lượng từng sản phẩm
                db.get($"EXEC UpdateCartItem {userId}, {productId}, {quantity}");
            }

            return RedirectToAction("Index");
        }


        // Xóa sản phẩm khỏi giỏ hàng
        public ActionResult RemoveFromCart(int productId)
        {
            DataModel db = new DataModel();
            int userId = Convert.ToInt32(Session["IDKH"]);
            decimal orderTotal = 0;
            // Thực thi stored procedure để xóa sản phẩm khỏi giỏ hàng
            db.get($"EXEC RemoveCartItem {userId}, {productId}, {orderTotal}");

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
