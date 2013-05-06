using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolApp.Models;
using SchoolApp.DAL;
using System.Web.Security;
using SchoolApp.Extensions;
using SchoolApp.ViewModels;

namespace SchoolApp.Controllers
{
    public class FutureStudentController : Controller
    {
        private SchoolContext db = new SchoolContext();

        //
        // GET: /FutureStudent/

        public ActionResult Index()
        {
            return View(db.FutureStudents.Include(x=>x.Guardians).ToList());
        }

        //
        // GET: /FutureStudent/Details/5

        public ActionResult Details(int id = 0)
        {
            UserProfile userprofile = db.UserProfiles.Find(id);
            if (userprofile == null)
            {
                return HttpNotFound();
            }
            return View(userprofile);
        }

        //
        // GET: /FutureStudent/Create

        public ActionResult Create()
        {
            var levels = from Level d in Enum.GetValues(typeof(Level))
                         select new { Name = Enum.GetName(typeof(Level), d), Value = Enum.GetName(typeof(Level), d) };

            ViewBag.Level = levels;
            return View();
        }

        //
        // POST: /FutureStudent/Create

        [HttpPost]
        public ActionResult Create(UserProfile userprofile)
        {
            if (ModelState.IsValid)
            {
                userprofile.CreationDate = DateTime.Now;
                userprofile.FutureStudent = true;
                //Todo: currently requires two trips to db to autogenerate username.
                db.UserProfiles.Add(userprofile);
                db.SaveChanges();
                userprofile.UserName = "Student" + userprofile.UserId;
                db.Entry(userprofile).State = EntityState.Modified;
                db.SaveChanges();
                Roles.AddUserToRole(userprofile.UserName, Helpers.STUDENT_ROLE);
                return RedirectToAction("Index");
            }

            return View(userprofile);
        }
        //
        // GET: /FutureStudent/Edit/5

        public ActionResult Edit(int id = 0)
        {
            StudentEditViewModel vm = new StudentEditViewModel();

            vm.Student = db.UserProfiles.Find(id);
            if (vm.Student == null)
            {
                return HttpNotFound();
            }
            var levels = from Level d in Enum.GetValues(typeof(Level))
                         select new { Name = Enum.GetName(typeof(Level), d), Value = Enum.GetName(typeof(Level), d) };

            Level currentLevel = vm.Student.StudentLevel;
            vm.LevelsList = new SelectList(levels, "Name", "Value", currentLevel);
            vm.StudentLevel = currentLevel;
            return View(vm); 
        }

        //
        // POST: /FutureStudent/Edit/5

        [HttpPost]
        public ActionResult Edit(UserProfile userprofile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userprofile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userprofile);
        }

        //
        // GET: /FutureStudent/Delete/5

        public ActionResult Delete(int id = 0)
        {
            UserProfile userprofile = db.UserProfiles.Find(id);
            if (userprofile == null)
            {
                return HttpNotFound();
            }
            return View(userprofile);
        }

        //
        // POST: /FutureStudent/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserProfile userprofile = db.UserProfiles.Find(id);
            db.UserProfiles.Remove(userprofile);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult MakeStudent(int UserId)
        {
            var user = db.UserProfiles.Find(UserId);
            user.FutureStudent = false;
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            return Content(Boolean.TrueString);
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}