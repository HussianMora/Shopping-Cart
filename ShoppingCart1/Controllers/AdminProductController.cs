using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShoppingCart1.Models;

namespace ShoppingCart1.Controllers
{
    public class AdminProductController : Controller
    {
        private ShoppingCartEntities db = new ShoppingCartEntities();

        // GET: AdminProduct
        public ActionResult  MainPage()
        {
         
                return View();
          
        }


          public ActionResult Order()
        {

                return View(db.Order.ToList());
           
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login login)
        {
            if (login.Name == "admin" && login.Password == "1234")
            {
                Session["Adminid"] = "1";
                ViewBag.SuccessMessage = "Authenticated Successfully";
                return RedirectToAction("MainPage", "AdminProduct");
            }
            else
            {
                ViewBag.Message = "Invalid Credentials";
                return View();

            }

        }
        
        

    }
}
