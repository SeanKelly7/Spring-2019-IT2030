using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using EventApplication.Models;

namespace EventApplication.Controllers
{
    public class HomeController : Controller
    {
        private EventApplicationDB db = new EventApplicationDB();

        public ActionResult LastMinuteDeals()
        {
            var e = GetLastMinuteDeal();
            return PartialView("_LastMinuteDeals", e);
        }

        private List<Event> GetLastMinuteDeal()
        {
            return db.Events
                .Where(a => DbFunctions.AddDays(a.StartDate, -2) < DateTime.Now && a.StartDate > DateTime.Now)
                .ToList();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}