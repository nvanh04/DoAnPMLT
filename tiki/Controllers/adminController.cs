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
        [HttpPost]
        public ActionResult addproduct(string name,string description,string price,string HINH, string category_id, string brand_id, string stock_quantity, string price_discount)
        {
            DataModel db = new DataModel();
            ViewBag.list = db.get("EXEC THEMSP1 N'"+ name+"', '"+ description+"', "+price+", '"+HINH+"', "+category_id+", "+brand_id+", "+stock_quantity+", "+price_discount+";");

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