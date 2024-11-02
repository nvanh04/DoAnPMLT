using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace tiki.Controllers
{
    public class adminController : Controller
    {
        // GET: admin
        public ActionResult Index()
        {
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
            return View();
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