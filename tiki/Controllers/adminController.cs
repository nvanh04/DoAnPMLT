using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
    }
}