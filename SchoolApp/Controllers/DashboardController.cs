using SchoolApp.DAL;
using SchoolApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolApp.Controllers
{
    public class DashboardController : Controller
    {
        private SchoolContext db = new SchoolContext();
        //
        // GET: /Dashboard/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Infoboxes()
        {
            var vm = new InfoboxesViewModel();
            vm.StudentCount = db.Students.Count();
            var date = DateTime.Now.AddMonths(-1);
            vm.StudentsIncrease = db.Students.Where(x => x.CreationDate >= date).Count();
            vm.TeacherCount = db.Teachers.Count();
            vm.TeacherIncrease = db.Teachers.Where(x => x.CreationDate >= date).Count();
            return PartialView(vm);
        }
        public ActionResult MyClasses()
        {
            var vm = new MyClassesViewModel();
            return PartialView();
        }
    }
}
