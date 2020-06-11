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

namespace ShoppingCart1.Controllers
{
    public class TempProductController : Controller
    {
        private ShoppingCartEntities db = new ShoppingCartEntities();

        // GET: TempProduct
        public ActionResult Index()
        {
            var product = db.Product.Include(p => p.Category1);
            return View(product.ToList());
        }

        // GET: TempProduct/Details/5
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

        // GET: TempProduct/Create
        public ActionResult Create()
        {
            ViewBag.Category_Id = new SelectList(db.Category, "Category_Id", "Name");
            return View();
        }

        // POST: TempProduct/Create
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

        // GET: TempProduct/Edit/5
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
            ViewBag.Category_Id = new SelectList(db.Category, "Category_Id", "Name", product.Category_Id);
            return View(product);
        }

        // POST: TempProduct/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Price,Description,Image,Stock,Category,Display,Category_Id")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Category_Id = new SelectList(db.Category, "Category_Id", "Name", product.Category_Id);
            return View(product);
        }

        // GET: TempProduct/Delete/5
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

        // POST: TempProduct/Delete/5
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
