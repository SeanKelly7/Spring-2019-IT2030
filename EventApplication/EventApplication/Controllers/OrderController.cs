using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EventApplication.Models;

namespace EventApplication.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        EventApplicationDB db = new EventApplicationDB();
		
        public ActionResult Index()
        {
            OrderCart cart = OrderCart.GetCart(HttpContext);

            OrderCartViewModel vm = new OrderCartViewModel()
            {
                OrderItems = cart.GetCartItems()
            };

            return View(vm);
        }
		
        public ActionResult Summary(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order @order = db.Orders.Find(id);
            if (@order == null)
            {
                return HttpNotFound();
            }
            return View(@order);
        }
		
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order @order = db.Orders.Find(id);
            if (@order == null)
            {
                return HttpNotFound();
            }
            return View(@order);
        }
		
        public ActionResult AddToCart(int id, int count)
        {
            OrderCart cart = OrderCart.GetCart(HttpContext);
            var atcResult = cart.AddToCart(id, count);

            if ( atcResult == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return RedirectToAction("Summary", new { id =  atcResult });
        }
		
        [HttpPost]
        public ActionResult CancelOrder(int id)
        {
            OrderCart cart = OrderCart.GetCart(HttpContext);

            string newStatus = cart.CancelOrder(id);

            OrderCartRemoveViewModel vm = new OrderCartRemoveViewModel()
            {
                DeleteId = id,
                Status = newStatus
            };

            return Json(vm);
        }
    }
}