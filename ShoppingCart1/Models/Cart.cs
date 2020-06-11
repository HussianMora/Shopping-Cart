using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCart1.Models
{
    public class Cart
    {
        public int product_id { get; set; }
        public string Name { get; set; }

        public int price { get; set; }

        public int qty { get; set; }

        public int total { get; set; }

        public string image { get; set; }



    }
}