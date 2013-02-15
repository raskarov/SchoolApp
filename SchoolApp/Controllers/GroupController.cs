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
using System.Web.Security;

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
            return View(GetGroupCreateRecords());
        }

        //
        // POST: /Group/Create

        [HttpPost]
        public ActionResult Create(GroupCreateEditViewModel groupView)
        {
            if (ModelState.IsValid)
            {
                db.Groups.Add(groupView.Group);
                db.SaveChanges();
                UpdateUsers(groupView);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(groupView);
        }

        //
        // GET: /Group/Edit/5

        public ActionResult Edit(int id = 0)
        {
            var model = GetGroupEditRecords(id);

            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        private GroupCreateEditViewModel GetGroupEditRecords(int id)
        {
            Group group = db.Groups.Include("Users").Where(x => x.GroupId == id).FirstOrDefault();
            var AllStudents = Roles.GetUsersInRole("Student");
            var GroupStudents = group.Users.Where(x => Roles.IsUserInRole(x.UserName, "Student")).Select(x => x.UserId);
            var FilteredStudents = db.UserProfiles.Where(x => AllStudents.Contains(x.UserName)).ToList();
            var AllTeachers = Roles.GetUsersInRole("Teacher");
            var GroupTeachers = group.Users.Where(x => Roles.IsUserInRole(x.UserName, "Teacher")).Select(x => x.UserId);
            var FilteredTeachers = db.UserProfiles.Where(x => AllTeachers.Contains(x.UserName)).ToList();
            var model = new GroupCreateEditViewModel
            {
                Students = new MultiSelectList(FilteredStudents, "UserId", "FullName", GroupStudents),
                Teachers = new MultiSelectList(FilteredTeachers, "UserId", "FullName", GroupTeachers),
                Group = group
            };
            return model;
        }
        private GroupCreateEditViewModel GetGroupCreateRecords()
        {
            var AllStudents = Roles.GetUsersInRole("Student");
            var FilteredStudents = db.UserProfiles.Where(x => AllStudents.Contains(x.UserName)).ToList();
            var AllTeachers = Roles.GetUsersInRole("Teacher");
            var FilteredTeachers = db.UserProfiles.Where(x => AllTeachers.Contains(x.UserName)).ToList();
            var model = new GroupCreateEditViewModel
            {
                Students = new MultiSelectList(FilteredStudents, "UserId", "FullName"),
                Teachers = new MultiSelectList(FilteredTeachers, "UserId", "FullName")
            };
            return model;
        }
        //
        // POST: /Group/Edit/5

        [HttpPost]
        public ActionResult Edit(GroupCreateEditViewModel groupView, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                db.Entry(groupView.Group).State = EntityState.Modified;
                UpdateUsers(groupView);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(groupView);
        }

        private void UpdateUsers(GroupCreateEditViewModel groupView)
        {
            var group = db.Groups.Include("Users").Where(x => x.GroupId == groupView.Group.GroupId).FirstOrDefault();
            if (groupView.SelectedTeacherIds == null)
            {
                groupView.SelectedTeacherIds = new int[0];
            }
            if (groupView.SelectedStudentIds == null)
            {
                groupView.SelectedStudentIds = new int[0];
            }
            foreach (var user in db.UserProfiles)
            {
                if (groupView.SelectedTeacherIds.Contains(user.UserId) || groupView.SelectedStudentIds.Contains(user.UserId))
                {
                    if (!group.Users.Contains(user))
                    {
                        group.Users.Add(user);
                    }
                }
                else
                {
                    if (group.Users.Contains(user))
                    {
                        group.Users.Remove(user);
                    }
                }
            }
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