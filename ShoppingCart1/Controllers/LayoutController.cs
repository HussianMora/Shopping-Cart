using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart1.Controllers
{
    public class LayoutController : Controller
    {
        // GET: Layout
        public ActionResult Top()
        {
            return View();
        }
        public ActionResult Box()
        {
            return View();
        }
        public ActionResult Fix()
        {
            return View();
        }
        public ActionResult Collapse()
        {
            return View();
        }
    }
}