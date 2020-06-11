using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart1.Controllers
{
    public class TablesController : Controller
    {
        // GET: Tables
        public ActionResult Simpletables()
        {
            return View();
        }
        public ActionResult Datatables()
        {
            return View();
        }
    }
}