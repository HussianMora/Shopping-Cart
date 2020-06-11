using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShoppingCart1.Models;

namespace ShoppingCart1.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        ShoppingCartEntities ShoppingCartEntities = new ShoppingCartEntities();
        public ActionResult Register()
        {
            if (Session["name"] != null)
            {
                return RedirectToAction("Index", "Product");
            }
            else
            {
                Customer customer = new Customer();
                return View(customer);
            }
        }
        [HttpPost]
        public ActionResult Register(Customer customer)
        {
            using (ShoppingCartEntities shoppingCart = new ShoppingCartEntities())

            {
                if (shoppingCart.Customer.Any(x => x.Name == customer.Name))
                {
                    ViewBag.Message = "Name Already Exits";
                    return View("Register", customer);
                }
                else
                {
                    shoppingCart.Customer.Add(customer);
                    shoppingCart.SaveChanges();
                }

                ModelState.Clear();
                ViewBag.Message = "Successfully Registered";
                return RedirectToAction("Login", "Customer");
            }
        }

        public ActionResult Login()
        {
            
            if (Session["Name"]==null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Product", new { name = Session["Name"].ToString(), id = Session["id"].ToString() });
            }
        }
        [HttpPost]
        public ActionResult Login(Login login,int id=1)
        {
            Customer customer = new Customer();

            var userlogin = ShoppingCartEntities.Customer.SingleOrDefault(x => x.Name == login.Name && x.Password == login.Password);

            if (userlogin != null)
            {
                Session["Name"] = login.Name;
                Session["id"] = userlogin.Customer_Id;
                return RedirectToAction("Index", "My", new { name = login.Name, id = customer.Customer_Id });
            }

            else
            {
                ViewBag.Message = "Invalid Credentials";
                return View();

            }
        }
        public ActionResult Logout()
        {
            Session["Name"] = null;
            return RedirectToAction("Index", "My");
        }
        public ActionResult MyAccount()
        {
            if(Session["Name"]==null)
            {
                return RedirectToAction("Login", "Customer");
            }
            else
            {
                String str = Session["Name"].ToString();
                Customer customer = ShoppingCartEntities.Customer.Where(x =>x.Name==str).SingleOrDefault();
                TempData["Name"] = customer.Name;
                TempData["MobileNo"] = customer.MobileNo;
                TempData["PostalCode"] = customer.PostalCode;
                return View();
            }

        }
        

        
       public ActionResult Edit()

        {
            return View();
            
        }
        [HttpPost]
        public ActionResult Edit(String Name)

        {
            var a = Convert.ToInt16(Session["id"]);
            Customer customer= ShoppingCartEntities.Customer.Where(x => x.Customer_Id ==a).SingleOrDefault();
            customer.Name =Name;
            ShoppingCartEntities.Entry(customer).State = System.Data.Entity.EntityState.Modified;
            ShoppingCartEntities.SaveChanges();
            Session["Name"] = Name;
            return RedirectToAction("MyAccount", "Customer");

        }
        public ActionResult MobileNoEdit()
        {
            return View();
        }
        [HttpPost]
        public ActionResult MobileNoEdit(Int64 Mobile_No)
        {
            var a = Convert.ToInt16(Session["id"]);
            Customer customer = ShoppingCartEntities.Customer.Where(x => x.Customer_Id==a).SingleOrDefault();
            customer.MobileNo =Mobile_No;
            ShoppingCartEntities.Entry(customer).State = System.Data.Entity.EntityState.Modified;
            ShoppingCartEntities.SaveChanges();
            
            return RedirectToAction("MyAccount", "Customer");


        }
        public ActionResult PostalEdit()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PostalEdit(int Postal)
        {
            var a = Convert.ToInt16(Session["id"]);
            Customer customer = ShoppingCartEntities.Customer.Where(x => x.Customer_Id == a).SingleOrDefault();
            customer.PostalCode = Postal;
            ShoppingCartEntities.Entry(customer).State = System.Data.Entity.EntityState.Modified;
            ShoppingCartEntities.SaveChanges();
            return RedirectToAction("MyAccount", "Customer");


        }

    }
}

       
   
    
