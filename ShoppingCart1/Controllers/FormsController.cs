using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart1.Controllers
{
    public class FormsController : Controller
    {
        // GET: Forms
        public ActionResult Genreralelements()
        {
            return View();
        }
        public ActionResult Advancedelements()
        {
            return View();
        }
        public ActionResult Editors()
        {
            return View();
        }



    }
}