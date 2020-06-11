using ShoppingCart1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace ShoppingCart1.Controllers
{
    public class ProductController : Controller
    {
        private const string V = "yes";

        // GET: Product

        ShoppingCartEntities shoppingCart = new ShoppingCartEntities();

        public ActionResult Index(int? i,string name)
        {

            
            ViewModel viewModel = new ViewModel();
            
       
                viewModel.products = shoppingCart.Product.Where(x => x.Display == "yes").ToList().ToPagedList(i ?? 1, 16);
                viewModel.categories = shoppingCart.Category.Where(x=>x.Display=="true").ToList();


                
                return View(viewModel);
            
        }
        public ActionResult  Main()
        {

            int a = 0;
            Session["AdminId"] = null;

            if (TempData["cart"] != null)
            {
                List<Cart> cart2 = TempData["cart"] as List<Cart>;
                foreach (var item in cart2)
                {
                    a += item.total;
                }
                TempData["Total"] = a;
            }
            TempData.Keep();

            return View();
        }

        public ActionResult Addtocart(int? id)
        {

            Product product = shoppingCart.Product.Where(x => x.Id == id).SingleOrDefault();
            var stock = product.Stock;
            if (stock >0)
            {
                if(stock<=10)
                {
                    TempData["Message"] = "Only few items left";
                }


                TempData["Stock"] = "1";


            }
            
            else
            {
                TempData["Stock"] = null;

            }
            return View(product);


        }

        List<Cart> carts = new List<Cart>();
        [HttpPost]
        public ActionResult Addtocart(int qty, int? id)
        {
            Product Product = shoppingCart.Product.Where(x => x.Id == id).SingleOrDefault();
           
            if(qty>Product.Stock)
            {
                TempData["flag"] = "2";
                return RedirectToAction("AddToCart", "Product", new { id = Product.Id });

            }
            else
            {
                    Cart c = new Cart();
                    c.product_id = Product.Id;
                c.image = Product.Image;
                    c.price = (int)Product.Price;
                    c.Name = Product.Name;
                    c.qty = Convert.ToInt32(qty);
                    c.total = c.qty * c.price;
                    if (TempData["cart"] == null)
                    {
                        carts.Add(c);
                        TempData["cart"] = carts;
                    }
                    else
                    {
                        List<Cart> cart2 = TempData["cart"] as List<Cart>;
                        int flag = 0;
                        foreach (var item in cart2)
                        {

                            if (item.product_id == c.product_id)
                            {
                                item.qty += c.qty;
                                item.total += c.total;
                                flag = 1;
                            }
                        }
                        if (flag == 0)
                        {
                            cart2.Add(c);
                        }
                        TempData["cart"] = cart2;

                    }

                Product.Stock = Product.Stock - qty;
                shoppingCart.Entry(Product).State = EntityState.Modified;
                shoppingCart.SaveChanges();
                return RedirectToAction("FrontDesign", "Product");

            }


            

        }
            
        public ActionResult Checkout()
        {





            if (TempData["cart"] == null)
            { TempData.Remove("cart"); }
            if (TempData["Total"]==null)
            {
                TempData.Remove("Total");
            }
            TempData.Keep();
            return View();

        }
        [HttpPost]
        public ActionResult Checkout(Order order)
        {


            if (Session["id"] != null)
            {
                

                List<Cart> list = TempData["cart"] as List<Cart>;
                Invoice invoice = new Invoice();
                //invoice.Customer =   Convert.ToInt32(Session["Id"]);
                invoice.Customer_Id = Convert.ToInt32(Session["id"]);
                invoice.Date = DateTime.Now;
                invoice.Total = (int)TempData["Total"];
                shoppingCart.Invoice.Add(invoice);
                shoppingCart.SaveChanges();







                foreach (var item in list)
                {
                   Order o = new Order();
                   Product product = shoppingCart.Product.Where(x => x.Id == item.product_id).SingleOrDefault();
                   o.Product_Id = item.product_id;
                    TempData["ProductName"] = TempData["ProductName"] + item.Name;
                   o.Invoice_Id = invoice.Id;
                   o.Customer_Id = Convert.ToInt32(Session["id"].ToString());
                   o.Date = DateTime.Now;
                   o.Quantity = item.qty;
                   o.Price = item.price;
                   shoppingCart.Order.Add(o);
                   shoppingCart.SaveChanges();
                }
                /*else
                {

                    TempData["flag"] = "2";
                    Cart c = carts.Where(x => x.product_id == product.Id).SingleOrDefault();
                    carts.Remove(c);

                    int i = 0;
                    foreach (var itm in carts)
                    {
                        i = i + itm.total;
                    }
                    TempData["Total"] = i;
                    if (TempData["cart"] == null)
                    { TempData.Remove("cart"); }
                    if (TempData["Total"].Equals(0))
                    {
                        TempData.Remove("Total");
                    }
                    return RedirectToAction("AddToCart", "Product",new {id=product.Id });


                }*/
                TempData.Remove("Total");
                TempData.Remove("Cart");
                
                return RedirectToAction("LastPage", "Product");



            }
            else
            {
                return RedirectToAction("Login", "Customer", new { id = 1 });
            }
        }

        public ActionResult LastPage()
        {
            //if (Session["id"] != null)
            //{
            return View();
            //}
            //else
            //{
            //    return RedirectToAction("Login", "Customer");
            //}
        }
        /*[HttpPost]
        public ActionResult LastPage(Order order)
        {
            
                List<Cart> list = TempData["cart"] as List<Cart>;
                Invoice invoice = new Invoice();
                //invoice.Customer =   Convert.ToInt32(Session["Id"]);
                invoice.Customer_Id = Convert.ToInt32(Session["id"]);
                invoice.Date = DateTime.Now;
                invoice.Total = (int)TempData["Total"];
                shoppingCart.Invoice.Add(invoice);
                shoppingCart.SaveChanges();

                foreach (var item in list)
                {
                    Order o = new Order();
                    o.Product_Id = item.product_id;
                    o.Invoice_Id = invoice.Id;
                    o.Date = DateTime.Now;
                    o.Quantity = item.qty;
                    o.Price = item.price;
                    shoppingCart.Order.Add(o);
                    shoppingCart.SaveChanges();



                }
                TempData.Remove("Total");
                TempData.Remove("Cart");
                return RedirectToAction("FinalPage", "Product");

            }*/

        public ActionResult Final()
        {
            /*List<Cart> list = TempData["cart"] as List<Cart>;
            Invoice invoice = new Invoice();
            //invoice.Customer =   Convert.ToInt32(Session["Id"]);
            invoice.Customer_Id = Convert.ToInt32(Session["id"]);
            invoice.Date = DateTime.Now;
            invoice.Total = (int)TempData["Total"];
            shoppingCart.Invoice.Add(invoice);
            shoppingCart.SaveChanges();

            foreach (var item in list)
            {
                Order o = new Order();
                o.Product_Id = item.product_id;
                o.Invoice_Id = invoice.Id;
                o.Date = DateTime.Now;
                o.Quantity = item.qty;
                o.Price = item.price;
                shoppingCart.Order.Add(o);
                shoppingCart.SaveChanges();



            }
            TempData.Remove("Total");
            TempData.Remove("Cart");*/
            return View();

        }

        public ActionResult Remove(int? id)
        {

            List<Cart> carts = TempData["Cart"] as List<Cart>;
            Cart c = carts.Where(x => x.product_id == id).SingleOrDefault();
            var p = shoppingCart.Product.Where(x => x.Id == id).SingleOrDefault();
            p.Stock = p.Stock + c.qty;
            shoppingCart.Entry(p).State = EntityState.Modified;
            shoppingCart.SaveChanges();

            carts.Remove(c);
            
            int i = 0;
            foreach (var item in carts)
            {
                i = i + item.total;
            }
            TempData["Total"] = i;
            if (TempData["cart"] == null)
            { TempData.Remove("cart"); }
            if (TempData["Total"].Equals(0))
            {
                TempData.Remove("Total");
            }
            return RedirectToAction("Checkout", "Product");


        }
        public ActionResult search(int? i,string Search)
        {

            if (Search != null)
            {
                ViewModel viewModel = new ViewModel();
                viewModel.products = shoppingCart.Product.Where(x => x.Name.StartsWith(Search) && x.Display == "yes").OrderByDescending(x => x.Id).ToList().ToPagedList(i ?? 1, 16);
                viewModel.categories = shoppingCart.Category.Where(x => x.Display == "true").ToList();



                return View(viewModel);
                

            }
            else
            {
                ViewModel viewModel = new ViewModel();
                viewModel.products = shoppingCart.Product.Where(x => x.Display == "yes").OrderByDescending(x => x.Id).ToList().ToPagedList(i ?? 1, 16);
                viewModel.categories = shoppingCart.Category.Where(x => x.Display == "true").ToList();


                return View(viewModel);
            }
        }

        public ActionResult Order()
        {
            if (Session["id"] != null)
            {
                int a = Convert.ToInt32(Session["id"].ToString());
                return View(shoppingCart.Order.Where(x => x.Customer_Id == a).ToList());
            }
            else
            {
                return RedirectToAction("Login", "Customer");
            }

        }

        public ActionResult Delete(int? id)
        {
            Order order = shoppingCart.Order.Where(x => x.Id == id).SingleOrDefault();
            shoppingCart.Order.Remove(order);
            shoppingCart.SaveChanges();
            return RedirectToAction("Order", "Product");

        }
        public ActionResult Men()
        {
            string s = "MenFashion";
            return View(shoppingCart.Product.Where(x => x.Category.Equals(s) && x.Display == "yes").ToList());
        }
        public ActionResult Electronics()
        {
            string s = "Electronics";
            return View(shoppingCart.Product.Where(x => x.Category.Equals(s) && x.Display == "yes").ToList());
        }
        public ActionResult Women()
        {
            string s = "Women Fashion";
            return View(shoppingCart.Product.Where(x => x.Category.Equals(s) && x.Display == "yes").ToList());
        }
        public ActionResult BooksandMusic()
        {
            string s = "Books and Music";
            return View(shoppingCart.Product.Where(x => x.Category.Equals(s) && x.Display == "yes").ToList());
        }
        public ActionResult Sports()
        {
            string s = "Sports";
            return View(shoppingCart.Product.Where(x => x.Category.Equals(s) && x.Display == "yes").ToList());
        }
        public ActionResult HealthCare()
        {
            string s = "HealthCare";
            return View(shoppingCart.Product.Where(x => x.Category.Equals(s) && x.Display == "yes").ToList());
        }
        public ActionResult Lifestyle()
        {
            string s = "Lifestyle";
            return View(shoppingCart.Product.Where(x => x.Category.Equals(s) && x.Display == "yes").ToList());
        }
        public ActionResult Pharmacy()
        {
            string s = "Pharmacy";
            return View(shoppingCart.Product.Where(x => x.Category.Equals(s) && x.Display == "yes").ToList());
        }
        public ActionResult Categories(int? i, string name)
        {
            ViewModel viewModel = new ViewModel();
            viewModel.products = shoppingCart.Product.Where(x => x.Display == "yes" && x.Category == name).OrderByDescending(x=>x.Id).ToList().ToPagedList(i ?? 1, 16);
            viewModel.categories = shoppingCart.Category.Where(x => x.Display == "true").ToList();

            ViewBag.Name = name;
            return View(viewModel);
        }
        public ActionResult Price(int? i, string lessthanfive, string fivetoone, string onetotwo, string twotofive, string fivetoten, string aboveten, string stock, string categories)
        {
            List<Product> list = new List<Product>();

            ViewModel viewModel = new ViewModel();

            if (lessthanfive == "true")
            {
                ViewBag.lessthanfive = "true";
                list.AddRange(shoppingCart.Product.Where(x => x.Price < 500 && x.Display == "yes").OrderByDescending(x => x.Id));

                //viewModel.products =  viewModel.products + shoppingCart.Product.Where(x => x.Price<500&&x.Display=="yes").ToList().ToPagedList(i ?? 1, 6);
                // viewModel.categories = shoppingCart.Category.ToList();

                //return View(viewModel);
            }
            if (fivetoone == "true")
            {
                ViewBag.fivetoone = "true";
                list.AddRange(shoppingCart.Product.Where(x => x.Price > 500 && x.Price < 1000 && x.Display == "yes"));

                //viewModel.products = shoppingCart.Product.Where(x => x.Price >500 && x.Price < 1000&&x.Display=="yes").ToList().ToPagedList(i ?? 1, 6);
                // viewModel.categories = shoppingCart.Category.ToList();

                //return View(viewModel);
            }
            if (onetotwo == "true")
            {
                ViewBag.onetotwo = "true";
                list.AddRange(shoppingCart.Product.Where(x => x.Price > 1000 && x.Price < 2000 && x.Display == "yes"));

                //viewModel.categories = shoppingCart.Category.ToList();

                //return View(viewModel);
            }
            if (twotofive == "true")
            {
                ViewBag.twotofive = "true";
                list.AddRange(shoppingCart.Product.Where(x => x.Price > 2000 && x.Price < 5000 && x.Display == "yes"));
                // viewModel.products = shoppingCart.Product.Where(x => x.Price > 2000 && x.Price < 5000 && x.Display == "yes").ToList().ToPagedList(i ?? 1, 6);
                //    viewModel.categories = shoppingCart.Category.ToList();

                //  return View(viewModel);


            }
            if (fivetoten == "true")
            {
                ViewBag.fivetoten = "true";
                list.AddRange(shoppingCart.Product.Where(x => x.Price > 5000 && x.Price < 10000 && x.Display == "yes"));
                //viewModel.products = shoppingCart.Product.Where(x => x.Price > 5000 && x.Price < 10000 && x.Display == "yes").ToList().ToPagedList(i ?? 1, 6);
                //     viewModel.categories = shoppingCart.Category.ToList();

                //  return View(viewModel);


            }
            else if (aboveten == "true")
            {
                ViewBag.aboveten = "true";
                list.AddRange(shoppingCart.Product.Where(x => x.Price > 10000 && x.Display == "yes"));
                //viewModel.products = shoppingCart.Product.Where(x => x.Price > 10000 && x.Display == "yes").ToList().ToPagedList(i ?? 1, 6);
                //viewModel.categories = shoppingCart.Category.ToList();

                //return View(viewModel);


            }
            if (stock == "in")
            {
                ViewBag.stock = "in";
                list.AddRange(shoppingCart.Product.Where(x => x.Stock > 0 && x.Display == "yes"));
                //viewModel.products = shoppingCart.Product.Where(x => x.Stock >0&&x.Display == "yes").ToList().ToPagedList(i ?? 1, 6);
                //    viewModel.categories = shoppingCart.Category.ToList();

                //  return View(viewModel);


            }
            if (stock == "out")
            {
                ViewBag.stock = "out";
                list.AddRange(shoppingCart.Product.Where(x => x.Stock == 0 && x.Display == "yes"));
                //viewModel.products = shoppingCart.Product.Where(x => x.Stock == 0&&x.Display=="yes").ToList().ToPagedList(i ?? 1, 6);
                //    viewModel.categories = shoppingCart.Category.ToList();

                //  return View(viewModel);




            }
        
            
                viewModel.products = list.OrderByDescending(x=>x.Id).ToList().ToPagedList(i ?? 1, 16);
            viewModel.categories = shoppingCart.Category.Where(x => x.Display == "true").ToList();
            return View(viewModel);
            
            /*else
            {
                viewModel.products = shoppingCart.Product.Where(x=>x.Display=="yes").OrderByDescending(x=>x.Id).ToList().ToPagedList(i ?? 1, 6);
                viewModel.categories = shoppingCart.Category.ToList();

                return View(viewModel);

            }*/
            


        }
        public ActionResult FrontDesign(string name,int? i)
        {
            int a = 0;
            if (TempData["cart"] != null)
            {
                List<Cart> cart2 = TempData["cart"] as List<Cart>;
                foreach (var item in cart2)
                {
                    a += item.total;
                }
                TempData["Total"] = a;
            }
            TempData.Keep();




            ViewModel viewModel = new ViewModel();

            if (!string.IsNullOrEmpty(name))
            { 
                viewModel.products = shoppingCart.Product.Where(x => x.Display == "yes" && x.Category == name).ToList().ToPagedList(i ?? 1, 16);
                viewModel.categories = shoppingCart.Category.Where(x => x.Display == "true").ToList();


                
                return View(viewModel);
            }
            else
            {
                viewModel.products = shoppingCart.Product.Where(x => x.Display == "yes"&&x.Stock>10).ToList().ToPagedList(i ?? 1, 16);
                viewModel.categories = shoppingCart.Category.Where(x => x.Display == "true").ToList();

                return View(viewModel);
            }
        }
        public ActionResult AddToCart2(int? id)
        {
            Product product = shoppingCart.Product.Where(x => x.Id == id).SingleOrDefault();
            return PartialView("AddtoCart2",product);

        }
        public ActionResult Checkout2()
        {
            return View();
        }
      


    }
}
        