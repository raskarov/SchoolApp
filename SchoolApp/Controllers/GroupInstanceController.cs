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
using SchoolApp.Extensions;
using DDay.iCal;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Web.UI.HtmlControls;
using DDay.iCal.Serialization.iCalendar;

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
            sli.Add(new SelectListItem { Text = "Все", Value = "0", Selected = true });
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
                //default recurrence once a week forever
                RecurrencePattern rp = new RecurrencePattern(FrequencyType.Weekly);
                groupinstance.RecurrenceRule = rp.ToString();

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
            var groups = db.LatestGroups;
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

        private void GetFilteredInfo(string start, string end, int id, IQueryable<Group> groups, DbSet<Classroom> classrooms, out IQueryable<Group> filteredGroups, out IQueryable<Classroom> filteredClassrooms)
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
                var inst = db.GroupInstances.Where(x => x.GroupInstanceId == groupinstance.GroupInstanceId).First();
                if (groupinstance.RecurrenceRule != null)
                {
                   
                    groupinstance.StartDateTime = inst.StartDateTime.Date + new TimeSpan(groupinstance.StartDateTime.Hour, groupinstance.StartDateTime.Minute, 0);
                    groupinstance.EndDateTime = inst.EndDateTime.Date + new TimeSpan(groupinstance.EndDateTime.Hour, groupinstance.EndDateTime.Minute, 0);
                    db.Entry(inst).State = EntityState.Detached;
                }
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

        public ActionResult Delete(string start, string end, int id = 0)
        {

            GroupInstanceDeleteViewModel givw = new GroupInstanceDeleteViewModel();
            GroupInstance groupinstance = db.GroupInstances.Find(id);
            if (groupinstance == null)
            {
                return HttpNotFound();
            }
            givw.GroupInstance = groupinstance;
            givw.StartDate = start;
            givw.EndDate = end;
            return PartialView(givw);
        }

        //
        // POST: /GroupInstance/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id, string RemoveInstance, string StartDate, string EndDate)
        {
            if (!String.IsNullOrEmpty(RemoveInstance))
            {
                GroupInstance groupinstance = db.GroupInstances.Find(id);
                if (Convert.ToBoolean(RemoveInstance) == false)
                {
                    db.GroupInstances.Remove(groupinstance);
                }
                else
                {
                    RecurrencePattern rp = new RecurrencePattern(groupinstance.RecurrenceRule);
                    RecurringComponent rc = new RecurringComponent();
                    
                    Event ev = new Event();
                    ev.RecurrenceRules.Add(rp);
                    PeriodList pl = new PeriodList();
                    Convert.ToDateTime(StartDate);
                    pl.Add(new iCalDateTime(StartDate));
                    pl.Add(new iCalDateTime(EndDate));
                    rc.ExceptionDates.Add(pl);
                    ev.ExceptionDates.Add(pl);
                    iCalendar ical = new iCalendar();
                    ical.Events.Add(ev);
                    iCalendarSerializer serializer = new iCalendarSerializer(ical);
                    groupinstance.RecurrenceRule = serializer.SerializeToString(ical);
                    db.Entry(groupinstance).State = EntityState.Modified;
                }
                db.SaveChanges();
            }
            return Content(Boolean.TrueString);
        }
        DateTime CreateNewDateTime(IDateTime date, DateTime time)
        {
            return new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);
        }
        /// <summary>
        /// Get scheduled events into calendar
        /// </summary>
        /// <returns>Formatted Json events</returns>
        public JsonResult GetEvents( DateTime start, DateTime end,int id = 0)
        {
            IEnumerable<GroupInstance> groupInstances;
            if (id > 0)
            {
                groupInstances = db.GroupInstances.Include(e => e.Group)
                                                  .Include(e => e.Group.Users)
                                                  .ToList()
                                                  .Where(x => x.Group.Users.Where(y => y.UserId == id
                                                       &&((x.StartDateTime>=start 
                                                           ||x.EndDateTime<=end)
                                                       &&x.RecurrenceRule==null)
                                                      ||x.RecurrenceRule!=null).Any());
            }
            else
            {
                groupInstances = db.GroupInstances.Include(e => e.Group)
                    .Include(e=>e.Group.Users).Where(x=>((x.StartDateTime>=start ||x.EndDateTime<=end)&&x.RecurrenceRule==null)||x.RecurrenceRule!=null).ToList();
            }
            if (groupInstances.Any())
            {
                var recurrence = groupInstances.Where(x => !String.IsNullOrWhiteSpace(x.RecurrenceRule)).FirstOrDefault();
                var list = new List<dynamic>();
                foreach (var instance in groupInstances)
                {
                    if (!String.IsNullOrWhiteSpace(instance.RecurrenceRule))
                    {
                        iCalendar ical = new iCalendar();
                        
                        RecurrencePattern rp = new RecurrencePattern(instance.RecurrenceRule);
                        Event ev = new Event();
                        ev.RecurrenceRules.Add(rp);
                        ev.Start = new iCalDateTime(recurrence.StartDateTime);
                        var occ = ev.GetOccurrences(start.AddDays(-1), end);


                        foreach (var occurence in occ)
                        {
                            list.Add(new
                            {
                                title = instance.Group.Name,
                                start = CreateNewDateTime(occurence.Period.StartTime, instance.StartDateTime).ToString("s"),
                                end = CreateNewDateTime(occurence.Period.EndTime, instance.EndDateTime).ToString("s"),
                                editable = false,
                                GroupInstanceId = instance.GroupInstanceId,
                                ClassroomId = instance.ClassroomId,
                                GroupId = instance.GroupId,
                                Color = instance.Group.Users.FirstOrDefault(y => Roles.IsUserInRole(y.UserName, "Teacher")).HexColor,
                                RecurrenceRule = recurrence.RecurrenceRule
                            });
                        }
                    }
                    else
                    {
                        list.Add(new
                         {
                             title = instance.Group.Name,
                             start = instance.StartDateTime.ToString("s"),
                             end = instance.EndDateTime.ToString("s"),
                             editable = false,
                             GroupInstanceId = instance.GroupInstanceId,
                             ClassroomId = instance.ClassroomId,
                             GroupId = instance.GroupId,
                             Color = instance.Group.Users.FirstOrDefault(y => Roles.IsUserInRole(y.UserName, "Teacher")).HexColor
                         });
                       
                    }
                }
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        //GET: /GroupInstance/GetEventDetails/1
        public ActionResult GetEventDetails(string StartDateTime, string EndDateTime,int id = 0)
        {
            if (id>0)
            {
                var instance = db.GroupInstances
                            .Include(e => e.Group)
                            .Include(e => e.Group.Users)
                            .Include(e => e.Classroom)
                            .Where(x => x.GroupInstanceId == id).FirstOrDefault();
                if (instance != null)
                {
                    var details = new EventDetails();
                    details.StartDateTime = StartDateTime;
                    details.EndDateTime = EndDateTime;
                    details.Students = instance.Group.Users.Where(x => Roles.IsUserInRole(x.UserName, Helpers.STUDENT_ROLE)).Select(x => x.FullName).ToList();
                    details.Teachers = instance.Group.Users.Where(x => Roles.IsUserInRole(x.UserName, Helpers.TEACHER_ROLE)).Select(x => x.FullName).ToList();
                    details.GroupInstanceId = instance.GroupInstanceId;
                    return PartialView("_EventBodyPartial", details);
                }
           
            }
            return Content(Boolean.FalseString);

        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}