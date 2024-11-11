using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
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
        [HttpPost]
        public ActionResult DatHang(string billing_name, string billing_address_1, string billing_tel, string billing_email, string order_comments, string payment_method)
        {
            DataModel db = new DataModel();
            int userId = Convert.ToInt32(Session["IDKH"]);

            decimal cartTotal = Session["CartTotal"] != null ? Convert.ToDecimal(Session["CartTotal"]) : 0;
            string coupon_code = Session["CouponCode"] != null ? Session["CouponCode"].ToString() : string.Empty;
            decimal discount = Session["discount"] != null ? Convert.ToDecimal(Session["discount"]) : 0;
            decimal totalAfterDiscount = cartTotal - discount;
            var result = db.get($"EXEC SaveOrder {userId}, N'{billing_name}', N'{billing_address_1}', '{billing_tel}', '{billing_email}', '{coupon_code}', '{order_comments}', '{payment_method}'");

            if (result != null && result.Count > 0)
            {
                var firstResult = result[0] as Dictionary<string, object>;

                int orderId = firstResult != null && firstResult.ContainsKey("OrderId") && firstResult["OrderId"] != DBNull.Value
                              ? Convert.ToInt32(firstResult["OrderId"])
                              : 0;

                decimal finalAmount = firstResult != null && firstResult.ContainsKey("FinalAmount") && firstResult["FinalAmount"] != DBNull.Value
                                      ? Convert.ToDecimal(firstResult["FinalAmount"])
                                      : 0;

                ViewBag.OrderId = orderId;
                ViewBag.FinalAmount = finalAmount;

                Session.Remove("CartItems");
                Session.Remove("CartTotal");
                Session.Remove("discount");
                Session.Remove("totalAfterDiscount");

                TempData["DatThanhCong"] = "Đặt hàng thành công!";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["DatThatBai"] = "Đặt hàng không thành công. Vui lòng thử lại.";
                return RedirectToAction("ThanhToan", "GioHang");
            }
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
            decimal discount = 0; // Khai báo biến discount bên ngoài

            // Kiểm tra mã giảm giá rỗng
            if (string.IsNullOrWhiteSpace(coupon_code))
            {
                TempData["CouponError"] = "Mã giảm giá không được để trống.";
                return RedirectToAction("Index");
            }

            // Gọi stored procedure ApplyCoupon
            ViewBag.discountResult = db.get($"EXEC ApplyCoupon {userId}, '{coupon_code}', {cartTotal}");


            // Kiểm tra kết quả trả về của stored procedure
            if (ViewBag.discountResult != null && ViewBag.discountResult.Count > 0)
            {
                ArrayList discountResult = (ArrayList)ViewBag.discountResult[0];

                // Chuyển đổi discountResult[0] sang decimal
                if (discountResult.Count > 0 && decimal.TryParse(discountResult[0].ToString(), out discount))
                {
                    if (discount > 0)
                    {
                         // Lưu mã giảm giá vào Session
                        Session["CouponCode"] = coupon_code;
                        TempData["Discount"] = discount;
                        TempData["CouponSuccess"] = $"Bạn đã được giảm giá {discount:C}.";
                    }
                    else
                    {
                        TempData["CouponError"] = "Mã giảm giá không hợp lệ, đã hết hạn hoặc không đáp ứng giá trị đơn hàng tối thiểu.";
                    }
                }
                else
                {
                    TempData["CouponError"] = "Mã giảm giá không hợp lệ, đã hết hạn hoặc không đáp ứng giá trị đơn hàng tối thiểu.";
                }
            }
            else
            {
                TempData["CouponError"] = "Đã xảy ra lỗi khi áp dụng mã giảm giá. Vui lòng thử lại.";
            }

            // Tính tổng tiền sau khi áp dụng mã giảm giá
            decimal totalAfterDiscount = cartTotal - discount;

            // Cập nhật lại các giá trị Session và TempData
            Session["discount"] = discount;
            Session["CartTotal"] = cartTotal;
            TempData["Discount"] = discount;
            TempData["TotalAfterDiscount"] = totalAfterDiscount;
            Session["totalAfterDiscount"] = totalAfterDiscount;
            return RedirectToAction("Index");
        }
        public ActionResult ThanhToan()
        {
            DataModel db = new DataModel();
            int userId = Convert.ToInt32(Session["IDKH"]);

            // Lấy thông tin giỏ hàng của người dùng khi chuyển đến trang thanh toán
            ViewBag.CartItems = db.get($"EXEC GetCartItems1 {userId}");
            ViewBag.cartTotalResult = db.get($"EXEC CalculateCartTotal {userId}");

            ViewBag.list = db.get($"EXEC LayTTKH1 {userId}");
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

            // Thực thi stored procedure để xóa sản phẩm khỏi giỏ hàng
            db.get($"EXEC RemoveCartItem {userId}, {productId}");

            return RedirectToAction("Index");
        }

        // Chuyển đến trang thanh toán
        
       
    }
}
