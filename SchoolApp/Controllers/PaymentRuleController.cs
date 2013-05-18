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
using System.Xml.Linq;

namespace SchoolApp.Controllers
{
    public class PaymentRuleController : Controller
    {
        private SchoolContext db = new SchoolContext();

        //
        // GET: /PaymentRule/

        public ActionResult Index(int id=0)
        {
            var paymentProfile = new PaymentProfileEditViewModel();
            paymentProfile.PaymentProfile = db.PaymentProfiles.Find(id);
            var paymentRules = db.PaymentRules.Where(x => x.PaymentProfileId == id).ToList();
            var oldOrCurrent = paymentRules.Where(x => x.EffectiveDate <= DateTime.Today).OrderByDescending(x => x.CreatedDate);
            paymentProfile.OldPaymentRules = oldOrCurrent.Skip(1).Take(5).ToList();
            paymentProfile.CurrentPaymentRule = oldOrCurrent.Take(1).FirstOrDefault();
            paymentProfile.FuturePaymentRule = paymentRules.Where(x => x.EffectiveDate > DateTime.Today).FirstOrDefault();
            return PartialView(paymentProfile);
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

        public ActionResult Create(bool isCurrent= false)
        {
            ViewBag.IsCurrent = isCurrent;
            return PartialView(new PaymentRule() { Amount = 0 });
        }

        //
        // POST: /PaymentRule/Create

        [HttpPost]
        public ActionResult Create(PaymentRule paymentrule, int id=0, bool isCurrent=false)
        {
            if (ModelState.IsValid)
            {
                paymentrule.CreatedDate = DateTime.Today;
                paymentrule.PaymentProfileId = id;
                if (isCurrent)
                {
                    paymentrule.EffectiveDate = DateTime.Today;
                }
                db.PaymentRules.Add(paymentrule);
                db.SaveChanges();
                return Content(Boolean.TrueString);
            }
            return Content("Please review your form");
        }

        //
        // GET: /PaymentRule/Edit/5


        public ActionResult Edit(int id = 0, bool current = false)
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
        // POST: /PaymentRule/Edit/5

        [HttpPost]
        public ActionResult Edit(PaymentRule paymentrule, int[] ruleStudents, int[] ruleAmount, bool? Variable)
        {
            if (ModelState.IsValid)
            {
                if (Variable??false)
                {
                    var xelement = new XElement("Root");
                    for (int i = 0; i < ruleStudents.Length; i++)
                    {
                        if (ruleStudents[i] > 0)
                        {
                            xelement.Add(new XElement("Rule", new XAttribute("NoOfStudents", ruleStudents[i]), new XAttribute("Amount", ruleAmount[i])));
                        }
                    }
                    paymentrule.xRule = xelement;
                    paymentrule.Amount = 0;
                    paymentrule.Variable = true;
                }
                else
                {
                    paymentrule.CreatedDate = DateTime.Today;
                    paymentrule.Variable = false;
                }
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