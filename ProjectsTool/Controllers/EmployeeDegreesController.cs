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
    public class EmployeeDegreesController : Controller
    {
        private AssignmentManagementToolEntities db = new AssignmentManagementToolEntities();

        // GET: EmployeeDegrees
        public ActionResult Index()
        {
            var employeeDegrees = db.EmployeeDegrees.Include(e => e.Degree).Include(e => e.Employee);
            return View(employeeDegrees.ToList());
        }

        // GET: EmployeeDegrees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeDegree employeeDegree = db.EmployeeDegrees.Find(id);
            if (employeeDegree == null)
            {
                return HttpNotFound();
            }
            return View(employeeDegree);
        }

        // GET: EmployeeDegrees/Create
        public ActionResult Create()
        {
            ViewBag.DegreeId = new SelectList(db.Degrees, "ID", "Course");
            ViewBag.EmployeeID = new SelectList(db.Employees, "ID", "Name");
            return View();
        }

        // POST: EmployeeDegrees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,EmployeeID,DegreeId")] EmployeeDegree employeeDegree)
        {
            if (ModelState.IsValid)
            {
                db.EmployeeDegrees.Add(employeeDegree);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DegreeId = new SelectList(db.Degrees, "ID", "Course", employeeDegree.DegreeId);
            ViewBag.EmployeeID = new SelectList(db.Employees, "ID", "Name", employeeDegree.EmployeeID);
            return View(employeeDegree);
        }

        // GET: EmployeeDegrees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeDegree employeeDegree = db.EmployeeDegrees.Find(id);
            if (employeeDegree == null)
            {
                return HttpNotFound();
            }
            ViewBag.DegreeId = new SelectList(db.Degrees, "ID", "Course", employeeDegree.DegreeId);
            ViewBag.EmployeeID = new SelectList(db.Employees, "ID", "Name", employeeDegree.EmployeeID);
            return View(employeeDegree);
        }

        // POST: EmployeeDegrees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,EmployeeID,DegreeId")] EmployeeDegree employeeDegree)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employeeDegree).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DegreeId = new SelectList(db.Degrees, "ID", "Course", employeeDegree.DegreeId);
            ViewBag.EmployeeID = new SelectList(db.Employees, "ID", "Name", employeeDegree.EmployeeID);
            return View(employeeDegree);
        }

        // GET: EmployeeDegrees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeDegree employeeDegree = db.EmployeeDegrees.Find(id);
            if (employeeDegree == null)
            {
                return HttpNotFound();
            }
            return View(employeeDegree);
        }

        // POST: EmployeeDegrees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EmployeeDegree employeeDegree = db.EmployeeDegrees.Find(id);
            db.EmployeeDegrees.Remove(employeeDegree);
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
