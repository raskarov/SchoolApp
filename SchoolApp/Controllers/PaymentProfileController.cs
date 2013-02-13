using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolApp.Models;
using DefaultConnection.DAL;

namespace SchoolApp.Controllers
{
    public class PaymentProfileController : Controller
    {
        private SchoolContext db = new SchoolContext();

        //
        // GET: /PaymentProfile/

        public ActionResult Index()
        {
            return View(db.PaymentProfiles.ToList());
        }

        //
        // GET: /PaymentProfile/Details/5

        public ActionResult Details(int id = 0)
        {
            PaymentProfile paymentprofile = db.PaymentProfiles.Find(id);
            if (paymentprofile == null)
            {
                return HttpNotFound();
            }
            return View(paymentprofile);
        }

        //
        // GET: /PaymentProfile/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /PaymentProfile/Create

        [HttpPost]
        public ActionResult Create(PaymentProfile paymentprofile)
        {
            if (ModelState.IsValid)
            {
                db.PaymentProfiles.Add(paymentprofile);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(paymentprofile);
        }

        //
        // GET: /PaymentProfile/Edit/5

        public ActionResult Edit(int id = 0)
        {
            PaymentProfile paymentprofile = db.PaymentProfiles.Find(id);
            if (paymentprofile == null)
            {
                return HttpNotFound();
            }
            return View(paymentprofile);
        }

        //
        // POST: /PaymentProfile/Edit/5

        [HttpPost]
        public ActionResult Edit(PaymentProfile paymentprofile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paymentprofile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(paymentprofile);
        }

        //
        // GET: /PaymentProfile/Delete/5

        public ActionResult Delete(int id = 0)
        {
            PaymentProfile paymentprofile = db.PaymentProfiles.Find(id);
            if (paymentprofile == null)
            {
                return HttpNotFound();
            }
            return View(paymentprofile);
        }

        //
        // POST: /PaymentProfile/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            PaymentProfile paymentprofile = db.PaymentProfiles.Find(id);
            db.PaymentProfiles.Remove(paymentprofile);
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