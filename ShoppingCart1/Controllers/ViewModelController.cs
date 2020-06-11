using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShoppingCart1.Models;
using PagedList.Mvc;
using PagedList;

namespace ShoppingCart1.Controllers
{
    public class ViewModelController : Controller
    {
        // GET: ViewModel
        ShoppingCartEntities shoppingCartEntities = new ShoppingCartEntities();
        public ActionResult Index(int? i)
        {
            ViewModel viewModel = new ViewModel();
            viewModel.products = shoppingCartEntities.Product.ToList().ToPagedList(i??1,6);
            viewModel.categories = shoppingCartEntities.Category.ToList();

            return View(viewModel);
        }
    }
}