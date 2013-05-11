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

namespace SchoolApp.Controllers
{
    public class UserGroupInstanceController : Controller
    {
        private SchoolContext db = new SchoolContext();

        //
        // GET: /UserGroupInstance/

        public ActionResult Index()
        {
            var usergroupinstances = db.UserGroupInstances.Include(u => u.User).Include(u => u.GroupInstance);
            return View(usergroupinstances.ToList());
        }

        //
        // GET: /UserGroupInstance/Details/5

        public ActionResult Details(int id = 0)
        {
            UserGroupInstance usergroupinstance = db.UserGroupInstances.Find(id);
            if (usergroupinstance == null)
            {
                return HttpNotFound();
            }
            return View(usergroupinstance);
        }

        //
        // GET: /UserGroupInstance/Create

        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "UserName");
            ViewBag.GroupInstanceId = new SelectList(db.GroupInstances, "GroupInstanceId", "GroupInstanceId");
            return View();
        }

        //
        // POST: /UserGroupInstance/Create

        [HttpPost]
        public ActionResult Create(UserGroupInstance usergroupinstance)
        {
            if (ModelState.IsValid)
            {
                db.UserGroupInstances.Add(usergroupinstance);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "UserName", usergroupinstance.UserId);
            ViewBag.GroupInstanceId = new SelectList(db.GroupInstances, "GroupInstanceId", "GroupInstanceId", usergroupinstance.GroupInstanceId);
            return View(usergroupinstance);
        }

        //
        // GET: /UserGroupInstance/Edit/5

        public ActionResult Edit(string Date, int id = 0) //this is a GroupInstanceId, not UserGroupInstanceId. TODO: make more obvious in routing.
        {
            DateTime InstanceDate = Convert.ToDateTime(Date);
            GroupInstance groupInstance = db.GroupInstances.Include(x => x.Group)
                                            .Include(x => x.Group.Users)
                                            .Where(x => x.GroupInstanceId == id).FirstOrDefault();
            var ugi = new List<UserGroupInstance>();
            if (groupInstance != null)
            {
                var group = GetGroupByDate(db.Groups.Include(x => x.Users), groupInstance.GroupId, InstanceDate);
                var studentsInInstance = group.Users.Where(x => Roles.IsUserInRole(x.UserName, "Student") && x.FutureStudent == false);
                var UserGroupInstances = db.UserGroupInstances.Include(x => x.GroupInstance).Include(x => x.User).Where(x => x.GroupInstanceId == id).ToList().Where(x=>x.InstanceDateTime.Date == InstanceDate.Date);
                
                foreach (var student in studentsInInstance.Except(UserGroupInstances.Select(x => x.User)))
                {
                    var newUserGroupInstance = new UserGroupInstance();
                    newUserGroupInstance.User = student;
                    newUserGroupInstance.UserId = student.UserId;
                    newUserGroupInstance.AttendanceTaken = DateTime.Now;
                    newUserGroupInstance.InstanceDateTime = InstanceDate;
                    newUserGroupInstance.Present = AttendanceType.NA;
                    newUserGroupInstance.GroupInstance = groupInstance;
                    ugi.Add(newUserGroupInstance);
                }
            }
            return View(ugi);
        }
        public Group GetGroupByDate(IQueryable<Group> Groups, int GroupId, DateTime Date)
        {
            var r = Groups.FirstOrDefault(g => g.GroupId == GroupId);
            if (r != null && r.ParentGroup != null)
            {
                if (r.CreatedDate < Date)
                {
                    return r;
                }
                else
                {
                    return GetGroupByDate(Groups, r.ParentGroup.GroupId, Date);  // Get the parent, if existing..
                }
            }
            else
                return r;   // Return the matching record
        }
        //
        // POST: /UserGroupInstance/Edit/5

        [HttpPost]
        public ActionResult Edit(UserGroupInstance usergroupinstance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usergroupinstance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "UserName", usergroupinstance.UserId);
            ViewBag.GroupInstanceId = new SelectList(db.GroupInstances, "GroupInstanceId", "GroupInstanceId", usergroupinstance.GroupInstanceId);
            return View(usergroupinstance);
        }

        //
        // GET: /UserGroupInstance/Delete/5

        public ActionResult Delete(int id = 0)
        {
            UserGroupInstance usergroupinstance = db.UserGroupInstances.Find(id);
            if (usergroupinstance == null)
            {
                return HttpNotFound();
            }
            return View(usergroupinstance);
        }

        //
        // POST: /UserGroupInstance/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            UserGroupInstance usergroupinstance = db.UserGroupInstances.Find(id);
            db.UserGroupInstances.Remove(usergroupinstance);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Save(Dictionary<string,string> groups)
        {
            //Jquery only sends string,string. 
            Dictionary<int, AttendanceType> groupsParsed = groups.ToDictionary(x => Convert.ToInt32(x.Key), x => (AttendanceType)Convert.ToInt32(x.Value));
            var groupInstances = db.UserGroupInstances.Include(x=>x.User).Where(x => groupsParsed.Keys.Contains(x.UserGroupInstanceID));
            foreach (KeyValuePair<int, AttendanceType> kvp in groupsParsed)
            {
                var groupInstance = groupInstances.Where(x => x.UserGroupInstanceID == kvp.Key).First();
                var PaymentProfileId = db.GroupInstances.Include(x=>x.Group).Where(x => x.GroupInstanceId == groupInstance.GroupInstanceId).First().Group.PaymentProfileId;
                var amount = db.PaymentRules.Where(x => x.PaymentProfileId == PaymentProfileId && x.EffectiveDate <= DateTime.Today).First().Amount;
                groupInstance.Present = kvp.Value;
                if (kvp.Value == AttendanceType.Absent || kvp.Value == AttendanceType.Present)
                {
                    Payment payment = new Payment();
                    payment.Amount = amount*-1;
                    payment.comments = "Списано через страницу посещений (" + Membership.GetUser().UserName + ")";
                    payment.UserId = groupInstance.UserId;
                    payment.TransactionDateTime = DateTime.Now;
                    db.Payments.Add(payment);
                }
                db.Entry(groupInstance).State = EntityState.Modified;
            }
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