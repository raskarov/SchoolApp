using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using SchoolApp.DAL;
using SchoolApp.Models;
using SchoolApp.ViewModels;
using SchoolApp.Extensions;
using System.Data.Entity;
using System;
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
            
            foreach (var group in db.LatestGroups.Include(e => e.Users))
            {
                gvm.Add(new GroupIndexViewModel(group));
            }
            return View(gvm);
        }
        //
        //Get: /Group/GroupInfo
        public ActionResult GroupInfo()
        {
            return PartialView();
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
                groupView.Group.CreatedDate = DateTime.Now;
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
            ViewBag.PaymentProfileId = new SelectList(db.PaymentProfiles, "PaymentProfileId", "Name", model.Group.PaymentProfileId);
            return View(model);
        }

        private GroupCreateEditViewModel GetGroupEditRecords(int id)
        {
            Group group = db.Groups.Include(e=>e.Users).Include(e=>e.ParentGroup).Where(x => x.GroupId == id).FirstOrDefault();
            var GroupStudents = group.Users.Where(x => Roles.IsUserInRole(x.UserName, Helpers.STUDENT_ROLE)).Select(x => x.UserId);
            var GroupTeachers = group.Users.Where(x => Roles.IsUserInRole(x.UserName, Helpers.TEACHER_ROLE)).Select(x => x.UserId);
            var model = new GroupCreateEditViewModel
            {
                Students = new MultiSelectList(db.Students.ToList(), "UserId", "FullName", GroupStudents),
                Teachers = new MultiSelectList(db.Teachers.ToList(), "UserId", "FullName", GroupTeachers),
                Group = group
            };
            return model;
        }
        private GroupCreateEditViewModel GetGroupCreateRecords()
        {
            var model = new GroupCreateEditViewModel
            {
                Students = new MultiSelectList(db.Students.ToList(), "UserId", "FullName"),
                Teachers = new MultiSelectList(db.Teachers.ToList(), "UserId", "FullName")
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

                groupView.Group.CreatedDate = DateTime.Now;
                int GroupId = groupView.Group.GroupId;
                db.Entry(groupView.Group).State = EntityState.Added;
                db.SaveChanges();
                groupView.Group.ParentGroup =  db.Groups.Find(GroupId);
                db.SaveChanges();
                UpdateUsers(groupView);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(groupView);
        }

        private void UpdateUsers(GroupCreateEditViewModel groupView)
        {
            var group = db.Groups.Include(e=>e.Users).Where(x => x.GroupId == groupView.Group.GroupId).FirstOrDefault();
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