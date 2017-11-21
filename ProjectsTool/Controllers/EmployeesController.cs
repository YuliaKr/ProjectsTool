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
    public class EmployeesController : Controller
    {
        private AssignmentManagementToolEntities db = new AssignmentManagementToolEntities();

        // GET: Employees
        public ActionResult Index()
        {
            var employees = db.Employees.Include(e => e.HeadQuater).Take(30);

            foreach (var em in employees)
            {
                string p = "";
                string d = "";
                string a = "";
                var positions = (from ep in db.EmployeePositions
                              where ep.EmployeeID == em.ID
                              select ep).ToList();

                foreach (var r in positions)
                {
                    if (r.ProjectId != null)
                    {
                        p += r.Position.PositionName.ToString() + " for " + r.Project.ProjectName.ToString() + "; ";
                    }
                    else
                    {
                        p += r.Position.PositionName.ToString() + "; ";
                    }
                }
                em.Positions = p;

                var degrees = (from ed in db.EmployeeDegrees
                                 where ed.EmployeeID == em.ID
                                 select ed).ToList();

                foreach (var de in degrees)
                {
                    if(de.Degree.Level != null)
                    {
                        d += de.Degree.Level.ToString() + " in " + de.Degree.Course.ToString() + "; ";
                    }
                    else
                    {
                        d += de.Degree.Course.ToString() + "; ";
                    }
                }
                em.Degrees = d;
                var addresses = (from ea in db.EmployeeAddresses
                               where ea.EmployeeID == em.ID
                               select ea).ToList();

                foreach (var ad in addresses)
                {
                    if (ad.Residence == 0)
                    {
                        a += ad.Address.City + " " + ad.Address.Street + " " + ad.Address.Number + "; ";
                    }
                    else
                    {
                        a += "Residence address: " + ad.Address.City + " " + ad.Address.Street + " " + ad.Address.Number + "; ";
                    }
                }
                em.Addresses = a;
            }
            return View(employees.ToList());
        }

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            ViewBag.HQid = new SelectList(db.HeadQuaters, "ID", "HQname");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,BSN,Name,Surname,HQid")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                //return RedirectToAction("Index");
                ViewBag.SuccessMessage = "Success!";
            }

            ViewBag.HQid = new SelectList(db.HeadQuaters, "ID", "HQname", employee.HQid);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.HQid = new SelectList(db.HeadQuaters, "ID", "HQname", employee.HQid);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,BSN,Name,Surname,HQid")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HQid = new SelectList(db.HeadQuaters, "ID", "HQname", employee.HQid);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
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

        //random isertion of employees
        public static string GenerateName(int len)
        {
            Random r = new Random();
            string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
            string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
            string Name = "";
            Name += consonants[r.Next(consonants.Length)].ToUpper();
            Name += vowels[r.Next(vowels.Length)];
            int b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
            while (b < len)
            {
                Name += consonants[r.Next(consonants.Length)];
                b++;
                Name += vowels[r.Next(vowels.Length)];
                b++;
            }
            return Name;
        }


        public ActionResult InsertRandom()
        {
            for (int i = 0; i < 10000; i++)
            {
                Random rname = new Random();
                int rn = rname.Next(4, 9);
                Random rsurname = new Random();
                int rs = rsurname.Next(7, 12);
                Random rbsn = new Random();
                int rb = rbsn.Next(100000, 999999);
           

                var hqs = (from hq in db.HeadQuaters
                                 select hq.ID).ToList();

                var projects = (from pr in db.Projects
                                select pr.ID).ToList();

                var positions = (from po in db.Positions
                                 select po.ID).ToList();

                Random phours = new Random();
                int ph = rname.Next(1, 100);

                Employee randomEmployee = new Employee();
                randomEmployee.Name = GenerateName(rn);
                randomEmployee.Surname = GenerateName(rs);
                randomEmployee.BSN = rb;
                randomEmployee.HQid = hqs.ElementAt(new Random(DateTime.Now.Millisecond).Next(hqs.Count()));
                db.Employees.Add(randomEmployee);
                db.SaveChanges();
                var employeeId = randomEmployee.ID;

                EmployeePosition employeePosition = new EmployeePosition();
                employeePosition.EmployeeID = employeeId;
                employeePosition.PositionId = positions.ElementAt(new Random(DateTime.Now.Millisecond).Next(positions.Count()));
                employeePosition.ProjectId = projects.ElementAt(new Random(DateTime.Now.Millisecond).Next(projects.Count()));
                employeePosition.ProjectHours = ph;
                db.EmployeePositions.Add(employeePosition);
                db.SaveChanges();
            }
            return null;
        }

        public ActionResult Queries()
        {
            StatisticsViewModel svm = new StatisticsViewModel();

            ViewBag.ProjectId = new SelectList(db.Projects, "ID", "ProjectName");
            ViewBag.EmployeeId = new SelectList(db.Employees, "ID", "Name");

            var ep = db.EmployeePositions.Take(50);

            svm.TotalWorkingHoursAll = Classes.MapReduce.Reduce(ep, 0.0, (accamulator, x) => accamulator + x.ProjectHours.Value);

            int totalEmployeesForProjects = Classes.MapReduce.Reduce(ep, 0, (accamulator, x) =>
            {
                if (x.ProjectHours != null)
                    accamulator++;
                return accamulator;
            });

            svm.AverageHoursPerEmployee = Math.Round(svm.TotalWorkingHoursAll / totalEmployeesForProjects, 2);

            return View(svm);
        }

        public ActionResult CheckHardworkersNumber(int ProjectId)
        {
          
            ViewBag.ProjectId = new SelectList(db.Projects, "ID", "ProjectName", ProjectId);
            IEnumerable<Tuple<EmployeePosition, Employee>> c = Classes.MapReduce.Join(db.EmployeePositions.Take(30), db.Employees.Take(30), l => l.Item1.EmployeeID == l.Item2.ID && l.Item1.ProjectHours > 20 && l.Item1.ProjectId.HasValue && l.Item1.ProjectId.Value == ProjectId);
            ViewBag.Count = c.Count();

            ViewBag.EmployeeId = new SelectList(db.Employees, "ID", "Name", 1);

            return View("Queries");

        }

        public ActionResult CheckSalaryPerEmployee(int EmployeeId)
        {

            ViewBag.EmployeeId = new SelectList(db.Employees, "ID", "Name", EmployeeId);
            ViewBag.ProjectId = new SelectList(db.Projects, "ID", "ProjectName", 1);
            IEnumerable<Tuple<EmployeePosition, Position>> c = Classes.MapReduce.Join(db.EmployeePositions.Take(30), db.Positions, l => l.Item1.PositionId == l.Item2.ID && l.Item1.EmployeeID == EmployeeId);
            var gross = Classes.MapReduce.Reduce(c, 0, (accamulator, y) => accamulator + y.Item1.ProjectHours.Value * y.Item2.HourFee.Value);
            ViewBag.Gross = gross;

            return View("Queries");

        }
    }
}
