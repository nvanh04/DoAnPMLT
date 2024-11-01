using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tiki.Models;

namespace tiki.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            DataModel db = new DataModel();
            ViewBag.listSanPham = db.get("EXEC xuatProducts");
            ViewBag.listDanhMuc = db.get("EXEC xuatDanhMuc");
            return View();
        }

        public ActionResult Danhmuc()
        {
            DataModel db = new DataModel();
            ViewBag.listDanhMuc = db.get("EXEC xuatDanhMuc");
            
            return View();
        }

        public ActionResult chitietsanpham(string id)
        {
            DataModel db = new DataModel();
            ViewBag.listchitietsanpham = db.get("EXEC TIMKIEMProductsID "+id+";");
            ViewBag.listTenCotSanPham = db.get("exec TenCotSanPham");
            ViewBag.listGetProductInfo = db.get("exec GetProductInfo");
           
            return View();
        }

    }
}