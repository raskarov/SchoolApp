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
    public class GroupInstanceController : Controller
    {
        private SchoolContext db = new SchoolContext();

        //
        // GET: /GroupInstance/

        public ActionResult Index()
        {
            GroupInstanceViewModel model = new GroupInstanceViewModel();
            model.GroupInstances  = db.GroupInstances.Include(g => g.Group).Include(g => g.Classroom).ToList();
            List<SelectListItem> sli = new List<SelectListItem>();
            sli.Add(new SelectListItem { Text = "All", Value = "0", Selected = true });
            foreach (var teacher in db.Teachers.ToList())
            {
                sli.Add(new SelectListItem { Text = teacher.FullName, Value = teacher.UserId.ToString() });
            }
            model.TeachersList = sli;
            return View(model);
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
            return PartialView(groupinstance);
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
            ViewBag.GroupId = new SelectList(db.Groups, "GroupId", "Name", groupinstance.GroupId);
            ViewBag.ClassroomId = new SelectList(db.Classrooms, "ClassroomID", "Name", groupinstance.ClassroomId);
            return Content("Review your form");
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
            return PartialView(groupinstance);
        }

        //
        // POST: /GroupInstance/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            GroupInstance groupinstance = db.GroupInstances.Find(id);
            db.GroupInstances.Remove(groupinstance);
            db.SaveChanges();
            return Content(Boolean.TrueString);
        }

        /// <summary>
        /// Get scheduled events into calendar
        /// </summary>
        /// <returns>Formatted Json events</returns>
        public JsonResult GetEvents(int id = 0)
        {
            IEnumerable<GroupInstance> groupInstances;
            if (id > 0)
            {
                groupInstances = db.GroupInstances.Include(e=>e.Group)
                                                  .Include(e=>e.Group.Users)
                                                  .ToList()
                                                  .Where(x => x.Group.Users.Where(y => y.UserId == id).Any());
            }
            else
            {
                groupInstances = db.GroupInstances.ToList();
            }
            var events = groupInstances.Select(x => new
            {
                title = "Test",
                start = x.StartDateTime.ToString("s"),
                end = x.EndDateTime.ToString("s"),
                editable = false,
                GroupInstanceId = x.GroupInstanceId
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