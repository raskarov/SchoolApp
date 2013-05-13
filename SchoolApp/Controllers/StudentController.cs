using System.Data;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Security;
using SchoolApp.DAL;
using SchoolApp.Models;
using SchoolApp.Extensions;
using System;
using System.Linq;
using SchoolApp.ViewModels;
using System.Collections.Generic;
namespace SchoolApp.Controllers
{
    public class StudentController : Controller
    {
        private SchoolContext db = new SchoolContext();

        //
        // GET: /Student/

        public ActionResult Index()
        {
            return View(db.Students.Include(x=>x.Guardians));
        }
        //
        // GET: /Student/Details/5

        public ActionResult Details(int id = 0)
        {
            var attendence = db.UserGroupInstances.Include(x=>x.User).Include(x=>x.GroupInstance).Include(x=>x.GroupInstance.Group).Where(x => x.UserId == id);
            if (!attendence.Any())
            {
                return HttpNotFound();
            }
            return View(attendence);
        }

        //
        // GET: /Student/Create

        public ActionResult Create()
        {
              var levels = from Level d in Enum.GetValues(typeof(Level))
                             select new { Name = Enum.GetName(typeof(Level), d), Value = Enum.GetName(typeof(Level),d) };

              ViewBag.Level = levels;
            return View();
        }

        //
        // POST: /Student/Create

        [HttpPost]
        public ActionResult Create(UserProfile userprofile)
        {
            if (ModelState.IsValid)
            {
                userprofile.CreationDate = DateTime.Now;
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
        // GET: /Student/Edit/5

        public ActionResult Edit(int id = 0)
        {
            StudentEditViewModel vm = new StudentEditViewModel();

            vm.Student = db.UserProfiles.Find(id);
            if (vm.Student == null)
            {
                return HttpNotFound();
            }
            var levels = from Level d in Enum.GetValues(typeof(Level))
                             select new { Name = Enum.GetName(typeof(Level), d), Value = Enum.GetName(typeof(Level),d) };

            Level currentLevel = vm.Student.StudentLevel;
            vm.LevelsList = new SelectList(levels, "Name", "Value", currentLevel);
            vm.StudentLevel = currentLevel;
            return View(vm); 
        }

        //
        // POST: /Student/Edit/5

        [HttpPost]
        public ActionResult Edit(StudentEditViewModel userprofile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userprofile.Student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userprofile);
        }

        //
        // GET: /Student/Delete/5

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
        // POST: /Student/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            UserProfile userprofile = db.UserProfiles.Find(id);
            db.UserProfiles.Remove(userprofile);
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