using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShoppingCart1.Models;
using PagedList.Mvc;
using PagedList;

namespace ShoppingCart1.Controllers
{
    public class NewProductController : Controller
    {
        private ShoppingCartEntities db = new ShoppingCartEntities();
        public ActionResult Index(int? i, string name)
        {
           ViewModel viewModel = new ViewModel();
           if (!string.IsNullOrEmpty(name))
           {
             viewModel.products = db.Product.Where(x => x.Category == name).ToList().ToPagedList(i ?? 1, 6);
             viewModel.categories = db.Category.ToList();
             ViewBag.Name = name;
             return View(viewModel);
           }
           else
           {
            viewModel.products = db.Product.ToList().ToPagedList(i ?? 1, 6);
            viewModel.categories = db.Category.ToList();
            return View(viewModel);
           }
        }
        public ActionResult NewPrice(int? i, string lessthanfive, string fivetoone, string onetotwo, string twotofive, string fivetoten, string aboveten, string stock, string categories)
        {
            List<Product> list = new List<Product>();
            ViewModel viewModel = new ViewModel();
            if (lessthanfive == "true")
            {
                ViewBag.lessthanfive = "true";
                list.AddRange(db.Product.Where(x => x.Price < 500 ).OrderByDescending(x => x.Id));
            }
            if (fivetoone == "true")
            {
                ViewBag.fivetoone = "true";
                list.AddRange(db.Product.Where(x => x.Price > 500 && x.Price < 1000));
            }
            if (onetotwo == "true")
            {
                ViewBag.onetotwo = "true";
                list.AddRange(db.Product.Where(x => x.Price > 1000 && x.Price < 2000 ));

            }
            if (twotofive == "true")
            {
                ViewBag.twotofive = "true";
                list.AddRange(db.Product.Where(x => x.Price > 2000 && x.Price < 5000));
              
            }
            if (fivetoten == "true")
            {
                ViewBag.fivetoten = "true";
                list.AddRange(db.Product.Where(x => x.Price > 5000 && x.Price < 10000 ));
               
            }
            else if (aboveten == "true")
            {
                ViewBag.aboveten = "true";
                list.AddRange(db.Product.Where(x => x.Price > 10000 ));
               
            }
            if (stock == "in")
            {
                ViewBag.stock = "in";
                list.AddRange(db.Product.Where(x => x.Stock > 0 ));
              
            }
            if (stock == "out")
            {
                ViewBag.stock = "out";
                list.AddRange(db.Product.Where(x => x.Stock == 0));
 
            }

            viewModel.products = list.OrderByDescending(x => x.Id).ToList().ToPagedList(i ?? 1, 6);
            viewModel.categories = db.Category.ToList();
            return View(viewModel);
        }
        public ActionResult NewCategories(int? i, string name)
        {
            ViewModel viewModel = new ViewModel();
            ViewBag.Category = name;
            viewModel.products = db.Product.Where(x => x.Category == name).OrderByDescending(x => x.Id).ToList().ToPagedList(i ?? 1, 6);
            viewModel.categories = db.Category.ToList();
            ViewBag.Name = name;
            return View(viewModel);
        }
        public ActionResult Newsearch(string Search, int? i)
        {
            ViewModel viewModel = new ViewModel();

            if (Search != null)
            {
                viewModel.products = db.Product.Where(x => x.Name.StartsWith(Search)).OrderByDescending(x => x.Id).ToList().ToPagedList(i ?? 1, 6);
                viewModel.categories = db.Category.ToList();
                return View(viewModel);

            }
            else
            {
                viewModel.products = db.Product.OrderByDescending(x => x.Id).ToList().ToPagedList(i ?? 1, 6);
                viewModel.categories = db.Category.ToList();

                return View(viewModel);
            }
        }



        // GET: NewProduct/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: NewProduct/Create
        public ActionResult Create()
        {
            var categories = db.Category.Where(x => x.Display == "true").ToList();

            SelectList list = new SelectList(categories, "Name", "Name");
            ViewBag.CategoryList = list;
            return View();
        }

        // POST: NewProduct/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {


            product.Display = "yes";
            string filename = Path.GetFileNameWithoutExtension(product.ImageFile.FileName);
            string extension = Path.GetExtension(product.ImageFile.FileName);
            filename = filename + extension;
            product.Image = "~/Content/Image/" + filename;
            filename = Path.Combine(Server.MapPath("~/Content/Image/"), filename);
            product.ImageFile.SaveAs(filename);

            db.Product.Add(product);
            db.SaveChanges();
            return RedirectToAction("Index");


        }

        // GET: NewProduct/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            var categories = db.Category.Where(x => x.Display == "true").ToList();

            SelectList list = new SelectList(categories, "Name", "Name");
            ViewBag.CategoryList = list;

            return View(product);
        }

        // POST: NewProduct/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {


            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        // GET: NewProduct/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: NewProduct/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Product.Find(id);
            db.Product.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
