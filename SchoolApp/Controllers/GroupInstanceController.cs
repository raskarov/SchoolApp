﻿using System;
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
        
        [HttpPost]
        public String SaveDroppedGroup(int GroupId, string Date)
        {
            if (GroupId > 0 && !String.IsNullOrEmpty(Date))
            {
                var GroupInstance = new GroupInstance();
                DateTime dt = Convert.ToDateTime(Date);
                GroupInstance.StartDateTime = dt;
                GroupInstance.EndDateTime = dt.AddHours(1);
                GroupInstance.GroupId = GroupId;
                db.GroupInstances.Add(GroupInstance);
                db.SaveChanges();
                return Boolean.TrueString;
            }
            return Boolean.FalseString;
        }
        //
        // GET: /GroupInstance/

        public ActionResult Index()
        {
            GroupInstanceViewModel model = new GroupInstanceViewModel();
            model.GroupInstances = db.GroupInstances.Include(g => g.Group).Include(g => g.Classroom).ToList();
            List<SelectListItem> sli = new List<SelectListItem>();
            sli.Add(new SelectListItem { Text = "Все", Value = "0", Selected = true });
            var teachers = db.Teachers.Include(x=>x.Groups).Where(x=>x.Groups.Count>0).ToList();
            var cl = new List<ColorLegend>();
            foreach (var teacher in teachers)
            {
                var colorListItem = new ColorLegend();
                colorListItem.Color = teacher.HexColor;
                colorListItem.FullName = teacher.FullName;
                colorListItem.GroupList = teacher.Groups.Select(x=> new GroupList{Name = x.Name, GroupId = x.GroupId}).ToList();
                cl.Add(colorListItem);
                sli.Add(new SelectListItem { Text = teacher.FullName, Value = teacher.UserId.ToString() });
            }
            model.TeachersList = sli;
            model.ColorLegend = cl;
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
                groupinstance.RecurrenceRule = "RRULE:" + rp.ToString();
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
                }
                db.Entry(inst).State = EntityState.Detached;
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
            GroupInstance groupinstance = db.GroupInstances.Find(id);
            if (!String.IsNullOrEmpty(RemoveInstance))
            {
                if (Convert.ToBoolean(RemoveInstance) == false)
                {
                    db.GroupInstances.Remove(groupinstance);
                }
                else
                {
                    iCalendarSerializer serializer = new iCalendarSerializer();
                    iCalendarCollection icalCollection = new iCalendarCollection();
                    using (TextReader tr = new StringReader(groupinstance.RecurrenceRule))
                    {
                        icalCollection = (iCalendarCollection)serializer.Deserialize(tr);
                    }

                    Event ev = new Event();
                    if (icalCollection.Count == 0)
                    {
                        RecurrencePattern rp = new RecurrencePattern(groupinstance.RecurrenceRule);
                        ev.RecurrenceRules.Add(rp);
                    }
                    else
                    {
                        ev = (Event)icalCollection.First().Events.First();
                    }
                 
                    PeriodList pl = new PeriodList();
                    if (ev.ExceptionDates.Count > 0)
                    {
                        pl.AddRange(ev.ExceptionDates.First());
                    }
                    var sd = Convert.ToDateTime(StartDate);
                    var time = new iCalDateTime(sd);
                   
                    pl.Add(time);
                    ev.ExceptionDates.Add(pl);
                    iCalendar ical = new iCalendar();
                    ical.Events.Add(ev);
                    serializer = new iCalendarSerializer(ical);
                    groupinstance.RecurrenceRule = serializer.SerializeToString(ical);
                    db.Entry(groupinstance).State = EntityState.Modified;
                }
                db.SaveChanges();
            }
            else
            {
                    db.GroupInstances.Remove(groupinstance);
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
                var filteredGroups  =db.GroupInstances.Include(e => e.Group)
                                                  .Include(e => e.Group.Users)
                                                  .ToList()
                                                  .Where(x => x.Group.Users.Where(y => y.UserId == id
                                                       &&(((x.StartDateTime>=start 
                                                           ||x.EndDateTime<=end)
                                                       &&x.RecurrenceRule==null)
                                                      ||x.RecurrenceRule!=null)).Any());
                groupInstances = filteredGroups;
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
                        iCalendarSerializer serializer = new iCalendarSerializer();
                        iCalendarCollection ical = new iCalendarCollection();
                        using (TextReader tr = new StringReader(instance.RecurrenceRule))
                        {
                            ical = (iCalendarCollection)serializer.Deserialize(tr);
                        }

                            Event ev = new Event();
                            if (ical.Count == 0)
                            {
                                RecurrencePattern rp = new RecurrencePattern(instance.RecurrenceRule);
                                ev.RecurrenceRules.Add(rp);
                            }
                            else
                            {
                                ev = (Event)ical.First().Events.First();
                            }
                            var ex = ev.ExceptionDates;
                        ev.Start = new iCalDateTime(instance.StartDateTime);
                        var occ = ev.GetOccurrences(start.AddDays(-1), end);
                        if (occ != null)
                        {
                            foreach (var occurence in occ)
                            {
                                    var user = instance.Group.Users.FirstOrDefault(y => Roles.IsUserInRole(y.UserName, "Teacher"));
                                    var color = "#FFFFFF";
                                    if (user != null)
                                    {
                                        color = user.HexColor;
                                    }
                                    list.Add(new
                                    {
                                        title = instance.Group.Name,
                                        start = CreateNewDateTime(occurence.Period.StartTime, instance.StartDateTime).ToString("s"),
                                        end = CreateNewDateTime(occurence.Period.EndTime, instance.EndDateTime).ToString("s"),
                                        editable = false,
                                        GroupInstanceId = instance.GroupInstanceId,
                                        ClassroomId = instance.ClassroomId,
                                        GroupId = instance.GroupId,
                                        Color = color,
                                        RecurrenceRule = recurrence.RecurrenceRule
                                    });
                            }
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