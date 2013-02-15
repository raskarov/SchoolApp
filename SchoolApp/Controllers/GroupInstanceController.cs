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
    public class GroupInstanceController : Controller
    {
        private SchoolContext db = new SchoolContext();

        //
        // GET: /GroupInstance/

        public ActionResult Index()
        {
            var groupinstances = db.GroupInstances.Include(g => g.Group).Include(g => g.Classroom);
            return View(groupinstances.ToList());
        }

        //
        // GET: /GroupInstance/Details/5

        public ActionResult Details(int id = 0)
        {
            GroupInstance groupinstance = db.GroupInstances.Find(id);
            if (groupinstance == null)
            {
                return HttpNotFound();
            }
            return View(groupinstance);
        }

        //
        // GET: /GroupInstance/Create

        public ActionResult Create()
        {
            ViewBag.GroupId = new SelectList(db.Groups, "GroupId", "Name");
            ViewBag.ClassroomId = new SelectList(db.Classrooms, "ClassroomID", "Name");
            return PartialView();
        }

        //
        // POST: /GroupInstance/Create

        [HttpPost]
        public ActionResult Create(GroupInstance groupinstance)
        {
            if (ModelState.IsValid)
            {
                db.GroupInstances.Add(groupinstance);
                db.SaveChanges();
                return Content(Boolean.TrueString);
            }

            ViewBag.GroupId = new SelectList(db.Groups, "GroupId", "Name", groupinstance.GroupId);
            ViewBag.ClassroomId = new SelectList(db.Classrooms, "ClassroomID", "Name", groupinstance.ClassroomId);
            return Content("Please review your form");
        }

        //
        // GET: /GroupInstance/Edit/5

        public ActionResult Edit(int id = 0)
        {
            GroupInstance groupinstance = db.GroupInstances.Find(id);
            if (groupinstance == null)
            {
                return HttpNotFound();
            }
            ViewBag.GroupId = new SelectList(db.Groups, "GroupId", "Name", groupinstance.GroupId);
            ViewBag.ClassroomId = new SelectList(db.Classrooms, "ClassroomID", "Name", groupinstance.ClassroomId);
            return View(groupinstance);
        }

        //
        // POST: /GroupInstance/Edit/5

        [HttpPost]
        public ActionResult Edit(GroupInstance groupinstance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(groupinstance).State = EntityState.Modified;
                db.SaveChanges();
                return Content(Boolean.TrueString);
            }
            else
            {
                return Content("Review your form");
            }
            //ViewBag.GroupId = new SelectList(db.Groups, "GroupId", "Name", groupinstance.GroupId);
            //ViewBag.ClassroomId = new SelectList(db.Classrooms, "ClassroomID", "Name", groupinstance.ClassroomId);
            //return View(groupinstance);
        }

        //
        // GET: /GroupInstance/Delete/5

        public ActionResult Delete(int id = 0)
        {
            GroupInstance groupinstance = db.GroupInstances.Find(id);
            if (groupinstance == null)
            {
                return HttpNotFound();
            }
            return View(groupinstance);
        }

        //
        // POST: /GroupInstance/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            GroupInstance groupinstance = db.GroupInstances.Find(id);
            db.GroupInstances.Remove(groupinstance);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult GetEvents()
        {
            var groupInstances = db.GroupInstances.ToList();

            var events = groupInstances.Select(x => new
            {
                title = "Test",
                start = x.StartDateTime.ToString("s"),
                end = x.EndDateTime.ToString("s"),
                editable = false
            });

            return Json(events, JsonRequestBehavior.AllowGet);
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}