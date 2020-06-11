using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCart1.BootstrapModal
{
    public class ModelFooter
    {
        public string SubmitButtonText { get; set; } = "Submit";
        public string CancelButtonText { get; set; } = "Cancel";
        public string SubmitButtonID { get; set; } = "btn-submit";
        public string CancelButtonID { get; set; } = "btn-cancel";
        public bool OnlyCancelButton { get; set; }

    }
}