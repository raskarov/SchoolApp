using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using SchoolApp.DAL;
using SchoolApp.Models;
using SchoolApp.ViewModels;
using System.Web.Security;
using System.Dynamic;

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
            model.GroupInstances = db.GroupInstances.Include(g => g.Group).Include(g => g.Classroom).ToList();
            List<SelectListItem> sli = new List<SelectListItem>();
            sli.Add(new SelectListItem { Text = "All", Value = "0", Selected = true });
            var teachers = db.Teachers.ToList();
            foreach (var teacher in teachers)
            {
                sli.Add(new SelectListItem { Text = teacher.FullName, Value = teacher.UserId.ToString() });
            }
            model.TeachersList = sli;
            model.ColorLegend = teachers.Select(x => new ColorLegend { FullName = x.FullName, Color = x.HexColor }).ToList();
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

        public ActionResult Create(DateTime start, DateTime end)
        {
            MakeFilteredDropdowns(start.ToString(), end.ToString());
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
            //TODO: review error handling. This will fail since it is not filtering dropdowns for overlapping events!!!
            ViewBag.GroupId = new SelectList(db.Groups, "GroupId", "Name", groupinstance.GroupId);
            ViewBag.ClassroomId = new SelectList(db.Classrooms, "ClassroomID", "Name", groupinstance.ClassroomId);
            return Content("Please review your form");
        }

        //
        // GET: /GroupInstance/Edit/5

        public ActionResult Edit(int id = 0, string start =null, string end = null)
        {
            GroupInstance groupinstance = db.GroupInstances.Find(id);
            if (groupinstance == null)
            {
                return HttpNotFound();
            }
            MakeFilteredDropdowns(start, end, groupinstance, id);
            return PartialView(groupinstance);
        }

        private void MakeFilteredDropdowns(string start, string end, int id=0)
        {
            var groups = db.Groups;
            var classrooms = db.Classrooms;

            if (start != null && end != null)
            {
                IQueryable<Group> filteredGroups;
                IQueryable<Classroom> filteredClassrooms;
                GetFilteredInfo(start, end, id, groups, classrooms, out filteredGroups, out filteredClassrooms);
                ViewBag.GroupId = new SelectList(filteredGroups, "GroupId", "Name");
                ViewBag.ClassroomId = new SelectList(filteredClassrooms, "ClassroomID", "Name");
            }
            else
            {
                    ViewBag.GroupId = new SelectList(groups, "GroupId", "Name");
                    ViewBag.ClassroomId = new SelectList(classrooms, "ClassroomID", "Name");
            }
        }
        private void MakeFilteredDropdowns(string start, string end, GroupInstance groupinstance, int id=0)
        {
            var groups = db.Groups;
            var classrooms = db.Classrooms;

            if (start != null && end != null)
            {
                IQueryable<Group> filteredGroups;
                IQueryable<Classroom> filteredClassrooms;
                GetFilteredInfo(start, end, id, groups, classrooms, out filteredGroups, out filteredClassrooms);
                ViewBag.GroupId = new SelectList(filteredGroups, "GroupId", "Name", groupinstance.GroupId);
                ViewBag.ClassroomId = new SelectList(filteredClassrooms, "ClassroomID", "Name", groupinstance.ClassroomId);
            }
            else
            {
                    ViewBag.GroupId = new SelectList(groups, "GroupId", "Name", groupinstance.GroupId);
                    ViewBag.ClassroomId = new SelectList(classrooms, "ClassroomID", "Name", groupinstance.ClassroomId);
            }
        }

        private void GetFilteredInfo(string start, string end, int id, DbSet<Group> groups, DbSet<Classroom> classrooms, out IQueryable<Group> filteredGroups, out IQueryable<Classroom> filteredClassrooms)
        {
            DateTime startDt = Convert.ToDateTime(start);
            DateTime endDt = Convert.ToDateTime(end);
            //Find any overlapping events
            var overlapping = db.GroupInstances.
                                Where(period =>
                                    ((period.StartDateTime > startDt && period.EndDateTime < endDt)
                                    || (period.EndDateTime > startDt && period.EndDateTime < endDt)
                                    || (period.StartDateTime < startDt && period.EndDateTime > endDt)
                                    || (period.StartDateTime < startDt && period.EndDateTime > endDt))
                                    && period.GroupInstanceId != id).ToList();
            var overlappingGroups = overlapping.Select(x => x.GroupId).ToList();
            filteredGroups = groups.Where(x => !overlappingGroups.Contains(x.GroupId));


            var overlappingClassrooms = overlapping.Select(x => x.ClassroomId).ToList();
            filteredClassrooms = classrooms.Where(x => !overlappingClassrooms.Contains(x.ClassroomID));
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
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditFromResize(GroupInstance groupInstance)
        {
            if (ModelState.IsValid)
            {
                
            }
            return Json(true);
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
                groupInstances = db.GroupInstances.Include(e => e.Group)
                                                  .Include(e => e.Group.Users)
                                                  .ToList()
                                                  .Where(x => x.Group.Users.Where(y => y.UserId == id).Any());
            }
            else
            {
                groupInstances = db.GroupInstances.Include(e => e.Group)
                    .Include(e=>e.Group.Users).ToList();
            }
            if (groupInstances.Any())
            {
                var events = groupInstances.Select(x => new
                {
                    title = x.Group.Name,
                    start = x.StartDateTime.ToString("s"),
                    end = x.EndDateTime.ToString("s"),
                    editable = false,
                    GroupInstanceId = x.GroupInstanceId,
                    ClassroomId = x.ClassroomId,
                    GroupId = x.GroupId,
                    Color = "#ff00ff"//x.Group.Users.FirstOrDefault(y => Roles.IsUserInRole(y.UserName, "Teacher")).HexColor
                });
                return Json(events, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
           
            
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}