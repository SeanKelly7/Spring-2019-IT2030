using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class productController : Controller
    {
        // GET: /product
        public string Index()
        {
            return "Product/index is displayed";
        }

		//Get: /product/Browse
		public string Browse()
		{
			return "Browse displayed";
		}

		//Get: /product/Details/105
		public string Details(int id)
		{
			return "Details displayed for id="+id;
		}

		//Get: /product/Location?zip=44124
		public string Location(int zip)
		{
			string message = HttpUtility.HtmlEncode("Product.Location, zip = " + zip);
			return message;
		}
	}
}