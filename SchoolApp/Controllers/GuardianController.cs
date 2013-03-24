using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolApp.Models;
using SchoolApp.DAL;

namespace SchoolApp.Controllers
{
    public class GuardianController : Controller
    {
        private SchoolContext db = new SchoolContext();

        //
        // GET: /Guardian/

        public ActionResult Index(int id=0)
        {
            var guardians = db.UserProfiles.Include(x=>x.Guardians).Where(x=>x.UserId==id).First().Guardians??new List<Guardian>();
            return View(guardians);
        }

        //
        // GET: /Guardian/Details/5

        public ActionResult Details(int id = 0)
        {
            Guardian guardian = db.Guardians.Find(id);
            if (guardian == null)
            {
                return HttpNotFound();
            }
            return View(guardian);
        }

        //
        // GET: /Guardian/Create

        public ActionResult Create(int UserId)
        {
            ViewBag.UserId = UserId;
            if (UserId > 0)
            {
                return View();
            }
            return HttpNotFound();
        }

        //
        // POST: /Guardian/Create

        [HttpPost]
        public ActionResult Create(Guardian guardian, int UserId)
        {
            if (ModelState.IsValid)
            {
                if (UserId > 0)
                {
                    db.Guardians.Add(guardian);
                    db.SaveChanges();
                    var student = db.UserProfiles.Include(x=>x.Guardians).Where(x => x.UserId == UserId).FirstOrDefault();
                    if (student != null)
                    {
                        student.Guardians.Add(guardian);
                        db.SaveChanges();
                    }
                    
                }

                return RedirectToAction("Edit", "Student", new { id = UserId });
            }

            return View(guardian);
        }

        //
        // GET: /Guardian/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Guardian guardian = db.Guardians.Find(id);
            if (guardian == null)
            {
                return HttpNotFound();
            }
            return View(guardian);
        }

        //
        // POST: /Guardian/Edit/5

        [HttpPost]
        public ActionResult Edit(Guardian guardian, int UserId = 0)
        {
            if (ModelState.IsValid)
            {
                db.Entry(guardian).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", "Student", new { id = UserId });
            }
            return View(guardian);
        }

        //
        // GET: /Guardian/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Guardian guardian = db.Guardians.Find(id);
            if (guardian == null)
            {
                return HttpNotFound();
            }
            return View(guardian);
        }

        //
        // POST: /Guardian/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id, int UserId)
        {
            Guardian guardian = db.Guardians.Find(id);
            db.Guardians.Remove(guardian);
            db.SaveChanges();
            return RedirectToAction("Edit", "Student", new { id = UserId });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}