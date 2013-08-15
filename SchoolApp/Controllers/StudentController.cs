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
using System.IO;
namespace SchoolApp.Controllers
{
    public class StudentController : Controller
    {
        private SchoolContext db = new SchoolContext();

        //
        // GET: /Student/
        public ActionResult Card()
        {
            return View();
        }
        public ActionResult Index()
        {
            var students = db.Students.Include(x => x.Guardians);
            foreach (var student in students)
            {
                if (student.Remarks != null && student.Remarks.Length>100)
                {
                    student.Remarks = student.Remarks.Substring(0, 100) + "...";
                }
            }
            return View(students);
        }
        //
        // GET: /Student/Details/5

        public ActionResult Details(int id = 0)
        {
            var attendence = db.UserGroupInstances.Include(x=>x.User).Include(x=>x.GroupInstance).Include(x=>x.GroupInstance.Group).Where(x => x.UserId == id);
            return View(attendence);
        }

        //
        // GET: /Student/Create

        public ActionResult Create()
        {
            StudentEditViewModel vm = new StudentEditViewModel();

              var levels = from Level d in Enum.GetValues(typeof(Level))
                             select new { Name = Enum.GetName(typeof(Level), d), Value = Enum.GetName(typeof(Level),d) };

              vm.LevelsList = new SelectList(levels, "Name", "Value");

              vm.Student = new UserProfile();

              return View(vm);
        }

        //
        // POST: /Student/Create

        [HttpPost]
        public ActionResult Create(StudentEditViewModel userprofile)
        {
            if (ModelState.IsValid)
            {
                var student = userprofile.Student;
                student.CreationDate = DateTime.Now;
                //Todo: currently requires two trips to db to autogenerate username.
                db.UserProfiles.Add(student);
                db.SaveChanges();
                student.UserName = "Student" + student.UserId;
                student.CreationDate = DateTime.Now;
                db.Entry(student).State = EntityState.Modified;

                var photo = this.Session["Photo"] as byte[];
                if (photo != null)
                {
                    userprofile.Student.Photo = photo;
                }
                
                db.SaveChanges();
                Roles.AddUserToRole(userprofile.Student.UserName, Helpers.STUDENT_ROLE);
                this.Session["Photo"] = null;
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
            Roles.RemoveUserFromRoles(userprofile.UserName, Roles.GetRolesForUser(userprofile.UserName));
            db.UserProfiles.Remove(userprofile);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AddPhoto(int id = 0)
        {
            StudentPhotoViewModel vm = new StudentPhotoViewModel();

            var user = new UserProfile();
            if (id != 0)
            {
                user = db.UserProfiles.Find(id);
            }
            if (user == null)
            {
                return HttpNotFound();
            }
            vm.FullName = user.FullName;
            vm.UserId = user.UserId;
            return View(vm);
        }
        [HttpPost]
        public ActionResult AddPhoto(StudentPhotoViewModel model)
        {
            var stream = Request.InputStream;
            string dump;

            using (var reader = new StreamReader(stream))
                dump = reader.ReadToEnd();

            var path = Server.MapPath("~/test.jpg");
            System.IO.File.WriteAllBytes(path, String_To_Bytes2(dump));
            if (!ModelState.IsValid)
            {
                return View(model);
            }
           var student = db.UserProfiles.Find(model.UserId);
           MemoryStream target = new MemoryStream();
           model.Photo.InputStream.CopyTo(target);
           byte[] data = target.ToArray();
           student.Photo = data;
           db.Entry(student).State = EntityState.Modified;
           db.SaveChanges();
           return View(model);
        }

        //
        // GET: /Student/GetPhoto

        public ActionResult GetPhoto(int id = 0)
        {
            var student = db.UserProfiles.Find(id);
            if (student != null && student.Photo != null)
            {
                return File(student.Photo, "image/jpeg");
            }
            var photo = this.Session["Photo"] as byte[];
            if (photo != null)
            {
                return File(photo, "image/jpeg");
            }
            return File("~/content/images/anon.jpg", "image/jpeg");
        }
        public ActionResult ShowPhoto(int id = 0)
        {
            var student = db.UserProfiles.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return PartialView(student);
        }

        //
        // POST: /Student/Capture

        [HttpPost]
        public ActionResult Capture(StudentPhotoViewModel model)
        {
            var stream = Request.InputStream;
            string dump;

            using (var reader = new StreamReader(stream))
            {
                dump = reader.ReadToEnd();
            }

            var bytes = String_To_Bytes2(dump);
            if (model.UserId == 0)
            {
                this.Session["Photo"] = bytes;
            }
            else
            {
                var student = db.UserProfiles.Find(model.UserId);
                student.Photo = bytes;
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
            }
            return View(model);
        }
        public ActionResult CardView()
        {
            var vm = new List<StudentCardViewModel>();
            var students = db.Students;
            foreach (var student in students)
            {
                var studentView = new StudentCardViewModel();
                studentView.Email = student.Email;
                studentView.FullName = student.FullName;
                studentView.Phone = student.Phone;
                studentView.UserId = student.UserId;
                studentView.Level = student.StudentLevel.ToString();
                vm.Add(studentView);
            }
            return View(vm);
        }
        private byte[] String_To_Bytes2(string strInput)
        {
            int numBytes = (strInput.Length) / 2;
            byte[] bytes = new byte[numBytes];

            for (int x = 0; x < numBytes; ++x)
            {
                bytes[x] = Convert.ToByte(strInput.Substring(x * 2, 2), 16);
            }

            return bytes;
        }
        [HttpPost]
        public ActionResult MakeStudent(int UserId)
        {
            var user = db.UserProfiles.Find(UserId);
            user.FutureStudent = true;
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