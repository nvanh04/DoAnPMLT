using System;
using System.Collections.Generic;
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
            return View();
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