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
    public class ProjectsController : Controller
    {
        private AssignmentManagementToolEntities db = new AssignmentManagementToolEntities();

        // GET: Projects
        public ActionResult Index()
        {
            var projects = db.Projects.Include(p => p.HeadQuater);

            foreach (var pr in projects)
            {
                var hq = (from hqs in db.HeadQuaters
                          where hqs.ID == pr.HQid
                          select hqs).FirstOrDefault();

                if (hq.MonthlyRent >= pr.Budget * 0.1)
                {
                    pr.Payable = "The project cannot pay rent";
                }
                else
                {
                    pr.Payable = "The project can pay the rent";
                }
            }

            return View(projects.ToList());
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            ViewBag.HQid = new SelectList(db.HeadQuaters, "ID", "HQname");
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ProjectName,Budget,TotalHours,HQid")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HQid = new SelectList(db.HeadQuaters, "ID", "HQname", project.HQid);
            return View(project);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            ViewBag.HQid = new SelectList(db.HeadQuaters, "ID", "HQname", project.HQid);
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ProjectName,Budget,TotalHours,HQid")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HQid = new SelectList(db.HeadQuaters, "ID", "HQname", project.HQid);
            return View(project);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
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


        //random isertion of projects
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
            for (int i = 0; i < 100; i++)
            {
                Random rname = new Random();
                int rn = rname.Next(7, 11);
                Random rbudget = new Random();
                int rb = rbudget.Next(10000, 1000000);
                Random rtotalhours = new Random();
                int rh = rtotalhours.Next(100, 10000);

                var hqs = (from hq in db.HeadQuaters
                           select hq.ID).ToList();

                Project randomProject = new Project();
                randomProject.ProjectName = GenerateName(rn);
                randomProject.TotalHours = rh;
                randomProject.Budget = rb;
                randomProject.HQid = hqs.ElementAt(new Random(DateTime.Now.Millisecond).Next(hqs.Count()));
                db.Projects.Add(randomProject);
                db.SaveChanges();
            }
            return null;
        }
    }
}
