using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolApp.Models;
using DefaultConnection.DAL;
using SchoolApp.ViewModels;

namespace SchoolApp.Controllers
{
    public class GroupController : Controller
    {
        private SchoolContext db = new SchoolContext();

        //
        // GET: /Group/

        public ActionResult Index()
        {
            var gvm = new List<GroupIndexViewModel>();
            foreach (var group in db.Groups.Include("Users"))
            {
                gvm.Add(new GroupIndexViewModel(group));
            }
            return View(gvm);
        }

        //
        // GET: /Group/Details/5

        public ActionResult Details(int id = 0)
        {
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        //
        // GET: /Group/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Group/Create

        [HttpPost]
        public ActionResult Create(Group group)
        {
            if (ModelState.IsValid)
            {
                db.Groups.Add(group);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(group);
        }

        //
        // GET: /Group/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        //
        // POST: /Group/Edit/5

        [HttpPost]
        public ActionResult Edit(Group group)
        {
            if (ModelState.IsValid)
            {
                db.Entry(group).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(group);
        }

        //
        // GET: /Group/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        //
        // POST: /Group/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Group group = db.Groups.Find(id);
            db.Groups.Remove(group);
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