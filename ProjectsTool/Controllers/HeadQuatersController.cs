using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectsTool.Models;

namespace ProjectsTool.Controllers
{
    public class HeadQuatersController : Controller
    {
        private AssignmentManagementToolEntities db = new AssignmentManagementToolEntities();

        // GET: HeadQuaters
        public ActionResult Index()
        {
            var headQuaters = db.HeadQuaters.Include(h => h.Address);
            return View(headQuaters.ToList());
        }

        // GET: HeadQuaters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HeadQuater headQuater = db.HeadQuaters.Find(id);
            if (headQuater == null)
            {
                return HttpNotFound();
            }
            return View(headQuater);
        }

        // GET: HeadQuaters/Create
        public ActionResult Create()
        {
            ViewBag.AddressId = new SelectList(db.Addresses, "ID", "Street");
            return View();
        }

        // POST: HeadQuaters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,HQname,NumberOfRooms,MonthlyRent,AddressId")] HeadQuater headQuater)
        {
            if (ModelState.IsValid)
            {
                db.HeadQuaters.Add(headQuater);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AddressId = new SelectList(db.Addresses, "ID", "Street", headQuater.AddressId);
            return View(headQuater);
        }

        // GET: HeadQuaters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HeadQuater headQuater = db.HeadQuaters.Find(id);
            if (headQuater == null)
            {
                return HttpNotFound();
            }
            ViewBag.AddressId = new SelectList(db.Addresses, "ID", "Street", headQuater.AddressId);
            return View(headQuater);
        }

        // POST: HeadQuaters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,HQname,NumberOfRooms,MonthlyRent,AddressId")] HeadQuater headQuater)
        {
            if (ModelState.IsValid)
            {
                db.Entry(headQuater).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AddressId = new SelectList(db.Addresses, "ID", "Street", headQuater.AddressId);
            return View(headQuater);
        }

        // GET: HeadQuaters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HeadQuater headQuater = db.HeadQuaters.Find(id);
            if (headQuater == null)
            {
                return HttpNotFound();
            }
            return View(headQuater);
        }

        // POST: HeadQuaters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HeadQuater headQuater = db.HeadQuaters.Find(id);
            db.HeadQuaters.Remove(headQuater);
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
