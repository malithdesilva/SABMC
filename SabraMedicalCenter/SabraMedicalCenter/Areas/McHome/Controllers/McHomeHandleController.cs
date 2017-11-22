using SabraMedicalCenter.Areas.Student.Models;
using SabraMedicalCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SabraMedicalCenter.Areas.McHome.Controllers
{
    public class McHomeHandleController : Controller
    {
        // GET: McHome/McHomeHandle

        MedicalcentreEntities db = new MedicalcentreEntities();
        // GET: HomeStudent
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Logingstudent()
        {
            if (Session["studentdetails"] == null)
            {
                Session["name"] = null;
                Session["nic"] = null;
            }

            return View();
        }



        [HttpPost]

        public ActionResult Logingstudent(StudentDTO student)
        {
            if (!ModelState.IsValid)
            {
                
                return View();
            }

            else
            {
                var details = db.MC_Student.FirstOrDefault(c => c.Student_NicNo == student.username && c.Student_NicNo == student.password
                    && c.Student_Password == null ||
                    (c.Student_NicNo == student.username && c.Student_Password == student.password));

                if (details != null)
                {
                    Session["studentdetails"] = details;
                    Session["name"] = details.Student_Name;
                    Session["nic"] = details.Student_NicNo;

                   // FormsAuthentication.SetAuthCookie(student.username, false);


                    //return RedirectToAction("Index", "StudentDetails", new { area = "Student" });


                    return RedirectToAction("Index", "StudentDetails", new { area = "Student" });

                }

                else
                {
                    ViewBag.Error = "Please enter corect user name and password";
                    return View();
                }

            }
        }

        [HttpGet]
        public ActionResult logoutstudent()
        {

            Session["name"] = null;
            return RedirectToAction("Index", "McHomeHandle", new { area = "McHome" });

        }
    }
}