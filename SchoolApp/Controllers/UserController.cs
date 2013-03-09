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
    public class UserController : Controller
    {
        private SchoolContext db = new SchoolContext();

        //
        // GET: /User/

        public ActionResult Index()
        {
            var RegisteredUsers = Roles.GetUsersInRole(Helpers.REGISTERED_USER_ROLE);
            var Teachers = Roles.GetUsersInRole(Helpers.TEACHER_ROLE);
            var Admins = Roles.GetUsersInRole(Helpers.ADMIN_ROLE);
            var users = db.UserProfiles.Select(x => new UserIndexViewModel
                                                {
                                                    UserId = x.UserId,
                                                    UserName = x.UserName,
                                                    FullName = x.FirstName + " " + x.LastName,
                                                    IsAdmin = Admins.Contains(x.UserName),
                                                    IsTeacher = Teachers.Contains(x.UserName),
                                                    IsRegistered = RegisteredUsers.Contains(x.UserName)
                                                });
            return View(users);
        }

        //
        // GET: /User/Details/5

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
        // GET: /User/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /User/Create

        [HttpPost]
        public ActionResult Create(UserProfile userprofile)
        {
            if (ModelState.IsValid)
            {
                db.UserProfiles.Add(userprofile);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userprofile);
        }

        //
        // GET: /User/Edit/5

        public ActionResult Edit(int id = 0)
        {
            UserProfile userprofile = db.UserProfiles.Find(id);
            if (userprofile == null)
            {
                return HttpNotFound();
            }
            return View(userprofile);
        }

        //
        // POST: /User/Edit/5

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
        // GET: /User/Delete/5

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
        // POST: /User/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            UserProfile userprofile = db.UserProfiles.Find(id);
            db.UserProfiles.Remove(userprofile);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //
        // POST

        [HttpPost]
        public ActionResult UpdateRole(string username, string role, bool value)
        {
            if (value)
            {
                if (!Roles.IsUserInRole(username, role))
                {
                    Roles.AddUserToRole(username, role);
                }
                else
                {
                    return Content(Boolean.FalseString);
                }
            }
            else
            {
                if (Roles.IsUserInRole(username, role))
                {
                    Roles.RemoveUserFromRole(username, role);
                }
                else
                {
                    return Content(Boolean.FalseString);
                }
            }
            return Content(Boolean.TrueString);
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}