using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using PagedList.Mvc;

namespace ShoppingCart1.Models
{
    public class ViewModel
    {

        public IPagedList<Product> products { get; set; }

        public List<Category> categories { get; set; }
    }
}