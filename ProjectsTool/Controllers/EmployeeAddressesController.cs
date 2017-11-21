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
    public class EmployeeAddressesController : Controller
    {
        private AssignmentManagementToolEntities db = new AssignmentManagementToolEntities();

        // GET: EmployeeAddresses
        public ActionResult Index()
        {
            var employeeAddresses = db.EmployeeAddresses.Include(e => e.Address).Include(e => e.Employee);
            return View(employeeAddresses.ToList());
        }

        // GET: EmployeeAddresses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeAddress employeeAddress = db.EmployeeAddresses.Find(id);
            if (employeeAddress == null)
            {
                return HttpNotFound();
            }
            return View(employeeAddress);
        }

        // GET: EmployeeAddresses/Create
        public ActionResult Create()
        {
            ViewBag.AddressId = new SelectList(db.Addresses, "ID", "Street");
            ViewBag.EmployeeID = new SelectList(db.Employees, "ID", "Name");
            return View();
        }

        // POST: EmployeeAddresses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,EmployeeID,AddressId,Residence")] EmployeeAddress employeeAddress)
        {
            if (ModelState.IsValid)
            {
                db.EmployeeAddresses.Add(employeeAddress);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AddressId = new SelectList(db.Addresses, "ID", "Street", employeeAddress.AddressId);
            ViewBag.EmployeeID = new SelectList(db.Employees, "ID", "Name", employeeAddress.EmployeeID);
            return View(employeeAddress);
        }

        // GET: EmployeeAddresses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeAddress employeeAddress = db.EmployeeAddresses.Find(id);
            if (employeeAddress == null)
            {
                return HttpNotFound();
            }
            ViewBag.AddressId = new SelectList(db.Addresses, "ID", "Street", employeeAddress.AddressId);
            ViewBag.EmployeeID = new SelectList(db.Employees, "ID", "Name", employeeAddress.EmployeeID);
            return View(employeeAddress);
        }

        // POST: EmployeeAddresses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,EmployeeID,AddressId,Residence")] EmployeeAddress employeeAddress)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employeeAddress).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AddressId = new SelectList(db.Addresses, "ID", "Street", employeeAddress.AddressId);
            ViewBag.EmployeeID = new SelectList(db.Employees, "ID", "Name", employeeAddress.EmployeeID);
            return View(employeeAddress);
        }

        // GET: EmployeeAddresses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeAddress employeeAddress = db.EmployeeAddresses.Find(id);
            if (employeeAddress == null)
            {
                return HttpNotFound();
            }
            return View(employeeAddress);
        }

        // POST: EmployeeAddresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EmployeeAddress employeeAddress = db.EmployeeAddresses.Find(id);
            db.EmployeeAddresses.Remove(employeeAddress);
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
