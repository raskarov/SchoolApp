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
    public class PaymentRuleController : Controller
    {
        private SchoolContext db = new SchoolContext();

        //
        // GET: /PaymentRule/

        public ActionResult Index()
        {
            var paymentrules = db.PaymentRules.Include(p => p.PaymentProfile);
            return View(paymentrules.ToList());
        }

        //
        // GET: /PaymentRule/Details/5

        public ActionResult Details(int id = 0)
        {
            PaymentRule paymentrule = db.PaymentRules.Find(id);
            if (paymentrule == null)
            {
                return HttpNotFound();
            }
            return View(paymentrule);
        }

        //
        // GET: /PaymentRule/Create

        public ActionResult Create()
        {
            ViewBag.PaymentProfileId = new SelectList(db.PaymentProfiles, "PaymentProfileId", "Name");
            return View();
        }

        //
        // POST: /PaymentRule/Create

        [HttpPost]
        public ActionResult Create(PaymentRule paymentrule)
        {
            if (ModelState.IsValid)
            {
                db.PaymentRules.Add(paymentrule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PaymentProfileId = new SelectList(db.PaymentProfiles, "PaymentProfileId", "Name", paymentrule.PaymentProfileId);
            return View(paymentrule);
        }

        //
        // GET: /PaymentRule/Edit/5

        public ActionResult Edit(int id = 0)
        {
            PaymentRule paymentrule = db.PaymentRules.Find(id);
            if (paymentrule == null)
            {
                return HttpNotFound();
            }
            ViewBag.PaymentProfileId = new SelectList(db.PaymentProfiles, "PaymentProfileId", "Name", paymentrule.PaymentProfileId);
            return View(paymentrule);
        }

        //
        // POST: /PaymentRule/Edit/5

        [HttpPost]
        public ActionResult Edit(PaymentRule paymentrule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paymentrule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PaymentProfileId = new SelectList(db.PaymentProfiles, "PaymentProfileId", "Name", paymentrule.PaymentProfileId);
            return View(paymentrule);
        }
        //
        // GET: /PaymentRule/EditPartial/5

        public ActionResult EditPartial(int id = 0)
        {
            PaymentRule paymentrule = db.PaymentRules.Find(id);
            if (paymentrule == null)
            {
                return HttpNotFound();
            }
            ViewBag.PaymentProfileId = new SelectList(db.PaymentProfiles, "PaymentProfileId", "Name", paymentrule.PaymentProfileId);
            return View(paymentrule);
        }

        //
        // POST: /PaymentRule/EditPartial/5

        [HttpPost]
        public ActionResult EditPartial(PaymentRule paymentrule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paymentrule).State = EntityState.Modified;
                db.SaveChanges();
                return Content(Boolean.TrueString);
            }
            else
            {
                return Content("Please review your form");
            }
        }
        //
        // GET: /PaymentRule/Delete/5

        public ActionResult Delete(int id = 0)
        {
            PaymentRule paymentrule = db.PaymentRules.Find(id);
            if (paymentrule == null)
            {
                return HttpNotFound();
            }
            return View(paymentrule);
        }

        //
        // POST: /PaymentRule/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            PaymentRule paymentrule = db.PaymentRules.Find(id);
            db.PaymentRules.Remove(paymentrule);
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