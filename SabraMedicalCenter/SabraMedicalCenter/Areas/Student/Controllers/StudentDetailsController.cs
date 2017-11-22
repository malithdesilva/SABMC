using SabraMedicalCenter.Areas.Student.Models;
using SabraMedicalCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SabraMedicalCenter.Areas.Student.Controllers
{
   
    public class StudentDetailsController : Controller
    {
        MedicalcentreEntities db = new MedicalcentreEntities();
        // GET: Student/StudentDetails

        public ActionResult Login()
        {
            return View();
        }

        
        [HttpPost]
        public ActionResult Login(StudentLogin deta)
        {
            try
            {
                MC_Student details = db.MC_Student.FirstOrDefault(c => c.Student_NicNo == deta.Password && c.Student_RegNo == deta.Username);
                if (details != null)
                {
                    //FormsAuthentication.SetAuthCookie(credentials.username, false);
                    //returnUrl = returnUrl.Split('/').LastOrDefault();
                    //return RedirectToAction(returnUrl);
                    Session["logindetails"] = details;
                    return RedirectToAction("Index", "StudentDetails");
                    //return RedirectToAction("Index", "StudentDetails", new { id = details.Admin_Id, name = details.Admin_UserName });
                }
                else
                {
                    ViewBag.Error = "Please Provide correct credentials.You are UnAthorized to this system";
                    return View(deta);
                }
            }
            catch (Exception ex)
            {
                return ViewBag.we == ex.InnerException;
            }
        }
     


        public ActionResult Index()
        {
            var deta = (MC_Student)Session["studentdetails"];

            StudentDetailsDTO val = new StudentDetailsDTO();
            val.studentname = deta.Student_Name;
            val.studentregno = deta.Student_RegNo;
            val.age = deta.Student_Age;
            val.nicno = deta.Student_NicNo;
            val.gender = deta.Student_Gender;
            val.dataofbirth = deta.Student_DateOfBirth;
            val.faculty = deta.Student_Faculty;
            val.year = deta.Student_Year;
            val.rationanal = deta.Student_Rational;
            val.mothername = deta.Student_MotherName;
            val.fathernmae = deta.Student_FatherName;
            val.familymembers = deta.Student_FamilyMembers;

            ViewBag.Name = deta.Student_Name;
            ViewBag.NICNO = deta.Student_NicNo;

            return View(val);
        }

        [HttpGet]
        public ActionResult edittudentdetails(string Nicnumber)
        {

            var deta = (MC_Student)Session["studentdetails"];

            StudentDetailsDTO val = new StudentDetailsDTO();
            val.studentname = deta.Student_Name;
            val.studentregno = deta.Student_RegNo;
            val.age = deta.Student_Age;
            val.nicno = deta.Student_NicNo;
            val.gender = deta.Student_Gender;
            val.dataofbirth = deta.Student_DateOfBirth;
            val.faculty = deta.Student_Faculty;
            val.year = deta.Student_Year;
            val.rationanal = deta.Student_Rational;
            val.mothername = deta.Student_MotherName;
            val.fathernmae = deta.Student_FatherName;
            val.familymembers = deta.Student_FamilyMembers;

            ViewBag.Name = deta.Student_Name;
            ViewBag.NICNO = deta.Student_NicNo;

            return View(val);
        }

        [HttpPost]
        public ActionResult edittudentdetails(StudentDetailsDTO deta)
        {

            //var student = (MC_Student)Session["studentdetails"];
            var detaval = (MC_Student)Session["studentdetails"];
            MC_Student student = db.MC_Student.FirstOrDefault(m => m.Student_NicNo == detaval.Student_NicNo);

            if (!ModelState.IsValid)
            {
                return View();
            }

            else
            {
                DateTime tt = DateTime.Now;
                
                if (tt< deta.dataofbirth)
                {
                    ViewBag.dateerror = "Please make sure you can not add future date for";
                    return View();
                }
                else
                {
                    DateTime now = DateTime.Today;
                    if (true)
                    {

                    }
                  //  int age = now.Year - deta.dataofbirth.Year;

                    student.Student_Name = deta.studentname;
                    student.Student_RegNo = deta.studentregno;
                    student.Student_Age = 26 ;
                    student.Student_NicNo = deta.nicno;
                    student.Student_Gender = deta.gender;
                    student.Student_DateOfBirth = deta.dataofbirth;
                    student.Student_Faculty = deta.faculty;
                    student.Student_Year = deta.year;
                    student.Student_Rational = deta.rationanal;
                    student.Student_MotherName = deta.mothername;
                    student.Student_FatherName = deta.fathernmae;
                    student.Student_FamilyMembers = deta.familymembers;

                    //db.Entry(student).State = System.Data.EntityState.Modified;
                    db.SaveChanges();

                    Session["studentdetails"] = student;

                    ViewBag.Name = student.Student_Name;
                    ViewBag.NICNO = student.Student_NicNo;

                    return RedirectToAction("Index");
                }
                
            }

        }

        [HttpGet]
        public ActionResult passwordreset(string resetnicno)
        {
            var deta = (MC_Student)Session["studentdetails"];
            ViewBag.Name = deta.Student_Name;
            ViewBag.NICNO = deta.Student_NicNo;

            return View(resetnicno);
        }

        [HttpPost]
        public ActionResult passwordreset(resetpasswordDTO newvalue)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var deta = (MC_Student)Session["studentdetails"];

            var newval = db.MC_Student.FirstOrDefault(c => c.Student_NicNo == deta.Student_NicNo);

            if (newval != null && newvalue.Password == newvalue.ConfirmPassword)
            {

                newval.Student_Password = newvalue.ConfirmPassword;
                db.SaveChanges();

            }
            ViewBag.Message = "Successfully reset password";
            return View();


        }
        [HttpGet]
        public ActionResult studenthistory(string nicno, string studentname)
        {
            var deta = (MC_Student)Session["studentdetails"];
            try
            {

                //var student = db.MC_Student.FirstOrDefault(c => c.Student_NicNo == deta.Student_NicNo);

                //List<MC_Student_History> studenthistory = db.MC_Student_History.Where(c => c.Student_Pk == student.Student_Pk).ToList();



                return View();
            }
            catch (Exception ex)
            {

                throw ex;

            }



        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
      
        public JsonResult HistoryDetails(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            var deta = (MC_Student)Session["studentdetails"];
            try
            {

                List<StudentHistoryDTO> ddl = new List<StudentHistoryDTO>();
                var student = db.MC_Student.FirstOrDefault(c => c.Student_NicNo == deta.Student_NicNo);

                List<MC_Student_History> studenthistory = db.MC_Student_History.Where(c => c.Student_Pk == student.Student_Pk).ToList();

                studenthistory = studenthistory.Skip(jtStartIndex).Take(jtPageSize).ToList();
                int totalstudent = studenthistory.Count();

                foreach (var item in studenthistory)
                {
                    StudentHistoryDTO dd = new StudentHistoryDTO();
                    dd.descrition = item.Description;
                    dd.disease = item.Disease;
                    dd.medical = item.Medical;
                    dd.date = item.Date;
                    dd.drugname = db.MC_Drug_History.Where(c => c.Student_History_Pk == item.Student_History_Pk).Select(c => c.Drug_Name).ToList();
                    dd.drugqty = db.MC_Drug_History.Where(c => c.Student_History_Pk == item.Student_History_Pk).Select(c => c.Drug_Qty).ToList();

                    ddl.Add(dd);


                }


                switch (jtSorting)
                {
                    case "disease ASC":
                        ddl = ddl.OrderBy(t => t.disease).ToList();
                        break;

                    case "disease DESC":
                        ddl = ddl.OrderByDescending(t => t.disease).ToList();
                        break;

                    case "date ASC":
                        ddl = ddl.OrderBy(t => t.date).ToList();
                        break;

                    case "date DESC":
                        ddl = ddl.OrderByDescending(t => t.date).ToList();
                        break;

                    case "descrition ASC":
                        ddl = ddl.OrderBy(t => t.descrition).ToList();
                        break;

                    case "descrition DESC":
                        ddl = ddl.OrderByDescending(t => t.descrition).ToList();
                        break;

                    case "medical ASC":
                        ddl = ddl.OrderBy(t => t.medical).ToList();
                        break;

                    case "medical DESC":
                        ddl = ddl.OrderByDescending(t => t.medical).ToList();
                        break;

                }

                // var  studenthistory1 = db.MC_Student_History.Where(c => c.Student_Pk == student.Student_Pk).ToList();
                return Json(new { Result = "OK", Records = ddl, TotalRecordCount = totalstudent });
            }
            catch (Exception ex)
            {

                throw ex;

            }

        }
    }
}