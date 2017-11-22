using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SabraMedicalCenter.Areas.Admin.Models;
using SabraMedicalCenter.Models;

namespace SabraMedicalCenter.Areas.Admin.Controllers
{
    public class addDStudentDetailsController : Controller
    {
        private SabraMedicalCenterContext db = new SabraMedicalCenterContext();

        // GET: Admin/addDStudentDetails
        public ActionResult Index()
        {
            return View(db.StudentDetails.ToList());
        }

        // GET: Admin/addDStudentDetails/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentDetails studentDetails = db.StudentDetails.Find(id);
            if (studentDetails == null)
            {
                return HttpNotFound();
            }
            return View(studentDetails);
        }

        // GET: Admin/addDStudentDetails/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/addDStudentDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "studentname,studentregno,age,nicno,gender,dataofbirth,faculty,year,rationanal,familymembers,fathernmae,mothername")] StudentDetails studentDetails)
        {
            if (ModelState.IsValid)
            {
                db.StudentDetails.Add(studentDetails);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(studentDetails);
        }

        // GET: Admin/addDStudentDetails/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentDetails studentDetails = db.StudentDetails.Find(id);
            if (studentDetails == null)
            {
                return HttpNotFound();
            }
            return View(studentDetails);
        }

        // POST: Admin/addDStudentDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "studentname,studentregno,age,nicno,gender,dataofbirth,faculty,year,rationanal,familymembers,fathernmae,mothername")] StudentDetails studentDetails)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentDetails).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(studentDetails);
        }

        // GET: Admin/addDStudentDetails/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentDetails studentDetails = db.StudentDetails.Find(id);
            if (studentDetails == null)
            {
                return HttpNotFound();
            }
            return View(studentDetails);
        }

        // POST: Admin/addDStudentDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            StudentDetails studentDetails = db.StudentDetails.Find(id);
            db.StudentDetails.Remove(studentDetails);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
