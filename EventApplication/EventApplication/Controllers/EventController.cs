using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EventApplication.Models;

namespace EventApplication.Controllers
{
    public class EventController : Controller
    {
        private EventApplicationDB db = new EventApplicationDB();
		
        public ActionResult Register(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        public ActionResult Find()
        {
            return View();
        }

        public ActionResult EventSearch(string eSearchString, string lSearchString)
        {
            var @event = GetEvents(eSearchString, lSearchString);
            if(@event.Count == 0)
            {
                return PartialView("_NothingFound");
            }
            else
            {
                return PartialView("_Find", @event);
            }
        }

        private List<Event> GetEvents(string eSearchString, string lSearchString)
        {
            return db.Events
                .Where(a => (a.Title.Contains(eSearchString) || a.Type.Title.Contains(eSearchString)) && (a.City.Contains(lSearchString) || a.State.Contains(lSearchString)) && a.StartDate > DateTime.Now)
                .OrderBy(a => a.StartDate)
                .ThenBy(a => a.Title)
                .ToList();
        }
		
        [Authorize]
        public ActionResult Index()
        {
            var events = db.Events.Include(a => a.Type);
            return View(events.ToList());
        }
		
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }
		
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.EventTypeId = new SelectList(db.EventTypes, "EventTypeId", "Title");
            return View();
        }
		
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "EventId,EventTypeId,Title,Desc,StartDate,EndDate,City,State,OrgName,OrgContact,TicMax,TicAvail")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Events.Add(@event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EventTypeId = new SelectList(db.EventTypes, "EventTypeId", "Title", @event.EventTypeId);
            return View(@event);
        }
		
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            ViewBag.EventTypeId = new SelectList(db.EventTypes, "EventTypeId", "Title", @event.EventTypeId);
            return View(@event);
        }
		
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "EventId,EventTypeId,Title,Desc,StartDate,EndDate,City,State,OrgName,OrgContact,TicMax,TicAvail")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EventTypeId = new SelectList(db.EventTypes, "EventTypeId", "Title", @event.EventTypeId);
            return View(@event);
        }
		
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }
		
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = db.Events.Find(id);
            db.Events.Remove(@event);
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
