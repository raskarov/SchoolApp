using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolApp.Models;
using SchoolApp.DAL;
using SchoolApp.ViewModels;

namespace SchoolApp.Controllers
{
    public class PaymentProfileController : Controller
    {
        private SchoolContext db = new SchoolContext();

        //
        // GET: /PaymentProfile/

        public ActionResult Index()
        {
            var paymentModel = new List<PaymentProfileEditViewModel>();

            foreach (var PaymentProfile in db.PaymentProfiles.ToList())
            {
                var p = new PaymentProfileEditViewModel();
                p.PaymentProfile = PaymentProfile;
                p.CurrentPaymentRule = db.PaymentRules.Where(x => x.PaymentProfileId == PaymentProfile.PaymentProfileId).FirstOrDefault();
                paymentModel.Add(p);
            }
            return View(paymentModel);
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
        //Get: /PaymentProfile/EditPaymentRule/5

        public ActionResult EditPaymentRule(int id=0, bool current=false)
        {
            PaymentRule paymentrule = db.PaymentRules.Find(id);
            if (paymentrule == null)
            {
                return HttpNotFound();
            }
            ViewBag.IsCurrent = current; //Used to hide effective date field for the current payment rule. 
            return PartialView(paymentrule);
        }

        //
        //POST: /PaymentProfile/EditPaymentRule/5
        [HttpPost]
        public ActionResult EditPaymentRule(PaymentRule paymentrule)
        {
            if (ModelState.IsValid)
            {
                paymentrule.CreatedDate = DateTime.Today;
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
            var paymentProfile = new PaymentProfileEditViewModel();
            paymentProfile.PaymentProfile = db.PaymentProfiles.Find(id);
            var paymentRules = db.PaymentRules.Where(x => x.PaymentProfileId == id).ToList();
            var oldOrCurrent = paymentRules.Where(x => x.EffectiveDate <= DateTime.Today).OrderByDescending(x => x.CreatedDate);
            paymentProfile.OldPaymentRules = oldOrCurrent.Skip(1).Take(5).ToList();
            paymentProfile.CurrentPaymentRule = oldOrCurrent.Take(1).FirstOrDefault();
            paymentProfile.FuturePaymentRule = paymentRules.Where(x => x.EffectiveDate >DateTime.Today).FirstOrDefault();
            if (paymentProfile.PaymentProfile == null)
            {
                return HttpNotFound();
            }
            return View(paymentProfile);
        }

        //
        // POST: /PaymentProfile/Edit/5

        [HttpPost]
        public ActionResult Edit(PaymentProfileEditViewModel pvm)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pvm.PaymentProfile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pvm);
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