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
            var guardians = db.UserProfiles.Find(id).Guardians??new List<Guardian>();
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

        public ActionResult Create(int id=0)
        {
            return View();
        }

        //
        // POST: /Guardian/Create

        [HttpPost]
        public ActionResult Create(Guardian guardian)
        {
            if (ModelState.IsValid)
            {
                db.Guardians.Add(guardian);
                db.SaveChanges();
                return RedirectToAction("Index");
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
        public ActionResult Edit(Guardian guardian)
        {
            if (ModelState.IsValid)
            {
                db.Entry(guardian).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
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
        public ActionResult DeleteConfirmed(int id)
        {
            Guardian guardian = db.Guardians.Find(id);
            db.Guardians.Remove(guardian);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}