using System;
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
            ViewBag.listDanhMuc = db.get("SELECT * FROM Categories");
            ViewBag.listnhacungcap = db.get("SELECT * FROM Brands");
            int userId = Convert.ToInt32(Session["IDKH"]);
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
                        string query = "EXEC THEMSP0 N'" + name + "', '" + description + "', " + price + ", '" + HINH.FileName + "', " + category_id + ", " + brand_id + ", " + stock_quantity + ", " + price_discount + ", " + userId + ";";

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
            catch (Exception )
            {
               
            }

            // Chuyển hướng về trang quản lý sản phẩm sau khi thêm thành công
            return RedirectToAction("product", "admin");
        }
        public ActionResult shopreview()
        {
            return View();
        }
        public ActionResult shoporderdetails()
        {
            return View();
        }
        public ActionResult shoporderlist()
        {
            return View();
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
            return View();
        }

    }
}