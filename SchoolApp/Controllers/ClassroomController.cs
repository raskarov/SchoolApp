using System.Data;
using System.Linq;
using System.Web.Mvc;
using SchoolApp.DAL;
using SchoolApp.Extensions;
using SchoolApp.Models;

namespace SchoolApp.Controllers
{
    public class ClassroomController : Controller
    {
        private SchoolContext db = new SchoolContext();

        //
        // GET: /Classroom/

        public ActionResult Index()
        {
            return View(db.Classrooms.ToList());
        }

        //
        // GET: /Classroom/Details/5

        public ActionResult Details(int id = 0)
        {
            Classroom classroom = db.Classrooms.Find(id);
            if (classroom == null)
            {
                return HttpNotFound();
            }
            return View(classroom);
        }

        //
        // GET: /Classroom/Create
        //[Authorize(Roles = Helpers.ADMIN_ROLE)]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Classroom/Create

        [HttpPost]
        public ActionResult Create(Classroom classroom)
        {
            if (ModelState.IsValid)
            {
                db.Classrooms.Add(classroom);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(classroom);
        }

        //
        // GET: /Classroom/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Classroom classroom = db.Classrooms.Find(id);
            if (classroom == null)
            {
                return HttpNotFound();
            }
            return View(classroom);
        }

        //
        // POST: /Classroom/Edit/5

        [HttpPost]
        public ActionResult Edit(Classroom classroom)
        {
            if (ModelState.IsValid)
            {
                db.Entry(classroom).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(classroom);
        }

        //
        // GET: /Classroom/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Classroom classroom = db.Classrooms.Find(id);
            if (classroom == null)
            {
                return HttpNotFound();
            }
            return View(classroom);
        }

        //
        // POST: /Classroom/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Classroom classroom = db.Classrooms.Find(id);
            db.Classrooms.Remove(classroom);
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