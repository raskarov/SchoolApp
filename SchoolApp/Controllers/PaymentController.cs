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
    public class PaymentController : Controller
    {
        private SchoolContext db = new SchoolContext();

        //
        // GET: /Payment/

        public ActionResult Index()
        {
            return View(db.Students.ToList());
        }

        //
        // GET: /Payment/Details/5

        public ActionResult Details(int id = 0)
        {
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        //
        // GET: /Payment/Create

        public ActionResult Create()
        {
            //TODO: Create ViewModel
            ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "UserName");
            return View();
        }

        //
        // POST: /Payment/Create

        [HttpPost]
        public ActionResult Create(Payment payment)
        {
            if (ModelState.IsValid)
            {
                payment.TransactionDateTime = DateTime.Now;
                db.Payments.Add(payment);
                db.SaveChanges();
                return Content(Boolean.TrueString);
            }
            return Content("Please review your form");
        }

        //
        // GET: /Payment/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "UserName", payment.UserId);
            return View(payment);
        }

        //
        // POST: /Payment/Edit/5

        [HttpPost]
        public ActionResult Edit(Payment payment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(payment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "UserName", payment.UserId);
            return View(payment);
        }

        //
        // GET: /Payment/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        //
        // POST: /Payment/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Payment payment = db.Payments.Find(id);
            db.Payments.Remove(payment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public PartialViewResult PaymentList(int id = 0)
        {
            var Payments = db.Payments.Where(x => x.UserId == id).OrderByDescending(x=>x.TransactionDateTime);
            ViewBag.Balance = Payments.Any()?Payments.Sum(x => x.Amount):0;
            return PartialView("_PaymentListPartial", Payments);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}