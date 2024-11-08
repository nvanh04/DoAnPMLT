using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using tiki.Models;


namespace tiki.Controllers
{
    public class CustomersController : Controller
    {
        // GET: Customers
        public ActionResult Index()
        {
            DataModel db = new DataModel();
            ViewBag.listUser = db.get("SELECT * FROM Users ");
            return View();
            
        }

    }
   
}