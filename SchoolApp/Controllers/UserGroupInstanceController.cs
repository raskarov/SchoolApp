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
    public class UserGroupInstanceController : Controller
    {
        private SchoolContext db = new SchoolContext();

        //
        // GET: /UserGroupInstance/

        public ActionResult Index()
        {
            var usergroupinstances = db.UserGroupInstances.Include(u => u.User).Include(u => u.GroupInstance);
            return View(usergroupinstances.ToList());
        }

        //
        // GET: /UserGroupInstance/Details/5

        public ActionResult Details(int id = 0)
        {
            UserGroupInstance usergroupinstance = db.UserGroupInstances.Find(id);
            if (usergroupinstance == null)
            {
                return HttpNotFound();
            }
            return View(usergroupinstance);
        }

        //
        // GET: /UserGroupInstance/Create

        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "UserName");
            ViewBag.GroupInstanceId = new SelectList(db.GroupInstances, "GroupInstanceId", "GroupInstanceId");
            return View();
        }

        //
        // POST: /UserGroupInstance/Create

        [HttpPost]
        public ActionResult Create(UserGroupInstance usergroupinstance)
        {
            if (ModelState.IsValid)
            {
                db.UserGroupInstances.Add(usergroupinstance);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "UserName", usergroupinstance.UserId);
            ViewBag.GroupInstanceId = new SelectList(db.GroupInstances, "GroupInstanceId", "GroupInstanceId", usergroupinstance.GroupInstanceId);
            return View(usergroupinstance);
        }

        //
        // GET: /UserGroupInstance/Edit/5

        public ActionResult Edit(int id = 0)
        {
            UserGroupInstance usergroupinstance = db.UserGroupInstances.Find(id);
            if (usergroupinstance == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "UserName", usergroupinstance.UserId);
            ViewBag.GroupInstanceId = new SelectList(db.GroupInstances, "GroupInstanceId", "GroupInstanceId", usergroupinstance.GroupInstanceId);
            return View(usergroupinstance);
        }

        //
        // POST: /UserGroupInstance/Edit/5

        [HttpPost]
        public ActionResult Edit(UserGroupInstance usergroupinstance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usergroupinstance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "UserName", usergroupinstance.UserId);
            ViewBag.GroupInstanceId = new SelectList(db.GroupInstances, "GroupInstanceId", "GroupInstanceId", usergroupinstance.GroupInstanceId);
            return View(usergroupinstance);
        }

        //
        // GET: /UserGroupInstance/Delete/5

        public ActionResult Delete(int id = 0)
        {
            UserGroupInstance usergroupinstance = db.UserGroupInstances.Find(id);
            if (usergroupinstance == null)
            {
                return HttpNotFound();
            }
            return View(usergroupinstance);
        }

        //
        // POST: /UserGroupInstance/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            UserGroupInstance usergroupinstance = db.UserGroupInstances.Find(id);
            db.UserGroupInstances.Remove(usergroupinstance);
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