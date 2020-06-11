using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart1.Controllers
{
    public class ChartController : Controller
    {
        // GET: Chart
        public ActionResult Chartjs()
        {
            return View();
        }
        public ActionResult Morris()
        {
            return View();
        }
        public ActionResult Flot()
        {
            return View();
        }
        public ActionResult InlineChart()
        {
            return View();
        }
    }
}