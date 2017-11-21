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
    public class EmployeePositionsController : Controller
    {
        private AssignmentManagementToolEntities db = new AssignmentManagementToolEntities();

        // GET: EmployeePositions
        public ActionResult Index()
        {
            var employeePositions = db.EmployeePositions.Include(e => e.Employee).Include(e => e.Position).Include(e => e.Project);
            return View(employeePositions.ToList());
        }

        // GET: EmployeePositions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeePosition employeePosition = db.EmployeePositions.Find(id);
            if (employeePosition == null)
            {
                return HttpNotFound();
            }
            return View(employeePosition);
        }

        // GET: EmployeePositions/Create
        public ActionResult Create()
        {
            ViewBag.EmployeeID = new SelectList(db.Employees, "ID", "Name");
            ViewBag.PositionId = new SelectList(db.Positions, "ID", "PositionName");
            ViewBag.ProjectId = new SelectList(db.Projects, "ID", "ProjectName");
            return View();
        }

        // POST: EmployeePositions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,EmployeeID,PositionId,ProjectId,ProjectHours")] EmployeePosition employeePosition)
        {
            if (ModelState.IsValid)
            {
                db.EmployeePositions.Add(employeePosition);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeID = new SelectList(db.Employees, "ID", "Name", employeePosition.EmployeeID);
            ViewBag.PositionId = new SelectList(db.Positions, "ID", "PositionName", employeePosition.PositionId);
            ViewBag.ProjectId = new SelectList(db.Projects, "ID", "ProjectName", employeePosition.ProjectId);
            return View(employeePosition);
        }

        // GET: EmployeePositions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeePosition employeePosition = db.EmployeePositions.Find(id);
            if (employeePosition == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeID = new SelectList(db.Employees, "ID", "Name", employeePosition.EmployeeID);
            ViewBag.PositionId = new SelectList(db.Positions, "ID", "PositionName", employeePosition.PositionId);
            ViewBag.ProjectId = new SelectList(db.Projects, "ID", "ProjectName", employeePosition.ProjectId);
            return View(employeePosition);
        }

        // POST: EmployeePositions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,EmployeeID,PositionId,ProjectId,ProjectHours")] EmployeePosition employeePosition)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employeePosition).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeID = new SelectList(db.Employees, "ID", "Name", employeePosition.EmployeeID);
            ViewBag.PositionId = new SelectList(db.Positions, "ID", "PositionName", employeePosition.PositionId);
            ViewBag.ProjectId = new SelectList(db.Projects, "ID", "ProjectName", employeePosition.ProjectId);
            return View(employeePosition);
        }

        // GET: EmployeePositions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeePosition employeePosition = db.EmployeePositions.Find(id);
            if (employeePosition == null)
            {
                return HttpNotFound();
            }
            return View(employeePosition);
        }

        // POST: EmployeePositions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EmployeePosition employeePosition = db.EmployeePositions.Find(id);
            db.EmployeePositions.Remove(employeePosition);
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
