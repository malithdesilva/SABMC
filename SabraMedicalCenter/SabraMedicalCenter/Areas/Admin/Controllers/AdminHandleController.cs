using SabraMedicalCenter.Areas.Admin.Models;
using SabraMedicalCenter.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Rotativa;
using System.Web.Security;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;

using Newtonsoft.Json;


namespace SabraMedicalCenter.Areas.Admin.Controllers
{
    [Authorize]
    public class AdminHandleController : Controller
    {

        MedicalcentreEntities db = new MedicalcentreEntities();
        // GET: Admin/AdminHandle
        /// <summary>
        /// Admin Login main view
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "AdminHandle");
        }
        /// <summary>
        /// Admin login post 
        /// Check Valid user
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(AdminLogin credentials, string returnUrl)

        {
            try
            {
                MC_Admin details = db.MC_Admin.FirstOrDefault(c => c.Admin_UserName
                == credentials.username && c.Admin_Password == credentials.password);
                if (details != null)
                {
                    FormsAuthentication.SetAuthCookie(credentials.username, false);

                    if (returnUrl == null)
                    {
                        return RedirectToAction("AdminPanel", "AdminHandle");
                    }
                    returnUrl = returnUrl.Split('/').LastOrDefault();
                    returnUrl = returnUrl.Split('?').FirstOrDefault();
                    return RedirectToAction(returnUrl);

                }
                else
                {
                    ViewBag.Error = "Please Provide correct credentials.You are UnAthorized to this system";
                    return View(credentials);
                }
            }
            catch (Exception ex)
            {
                return ViewBag.we == ex.InnerException;
            }



        }
        /// <summary>
        /// Direct to Admin panel after user is login succesfully
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AdminPanel()
        {

            var admin = ((MC_Admin)Session["logindetails"]);

            List<DataPoint> dataPoints = new List<DataPoint>();
            List<DataPoint> dataPoints1 = new List<DataPoint>();
            List<DataPoint> dataPoints2 = new List<DataPoint>();
            List<DataPoint> dataPoints3 = new List<DataPoint>();
            List<DataPoint> dataPoints4 = new List<DataPoint>();
            List<DrugStockDTO> ddl = new List<DrugStockDTO>();

            List<MC_Ddrug_Stoke> dd = db.MC_Ddrug_Stoke.ToList();

            foreach (var item in dd)
            {
                DrugStockDTO aa = new DrugStockDTO();

                aa.drugname = item.Drug_Name;
                aa.drugcode = item.Drug_Code;
                aa.exipiredate = item.Expire_Date;
                aa.balance = db.MC_Drug_Balance.Where(c => c.Drug_Code == item.Drug_Code).
                Select(c => c.Balance).FirstOrDefault();

                ddl.Add(aa);
            }

            ViewBag.medeta = ddl;


            List<string> faculties = new List<string>();
            var de = db.MC_Student.Select(c => c.Student_Faculty).Distinct().ToList();

            //List<int> pks = db.MC_Student_History.Where(c => c.Date.Value.Month == DateTime.Now.Month).Select(c => c.Student_Pk).ToList();
            List<int> pks = db.MC_Student_History.Where(c => c.Date.Value.Month == 10).Select(c => c.Student_Pk).ToList();

            foreach (var pk in pks)
            {
                var faculty = db.MC_Student.Where(c => c.Student_Pk == pk).Select(v => v.Student_Faculty).ToString();
                var hh = db.MC_Student.FirstOrDefault(c => c.Student_Pk == pk).Student_Faculty;
                faculties.Add(hh);
            }

            foreach (string label in de)
            {
                int y = faculties.Where(c => c.Contains(label)).Count();
                //var co = faculties.OrderBy().

                //xValue.Add(item);
                //yValue.Add(count);

                dataPoints.Add(new DataPoint(y, label,label));

            }
           
            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints, _jsonSetting);
           
            DateTime dateTime = new DateTime(2017, 01, 1, 0, 0, 0);
         
            for (int i = 1; i < 13; i++)
            {
                string label = "";
                int y = 50;
                 y = db.MC_Student_History.Where(x => x.Date.Value.Month == i && x.MC_Student.Student_Gender == "Male").ToList().Count();
              
                label = dateTime.ToString("dd MMM");

                dataPoints1.Add(new DataPoint(y, label));
                dateTime = dateTime.AddMonths(1);
            }
            ViewBag.DataPoints1 = JsonConvert.SerializeObject(dataPoints1, _jsonSetting);

            for (int i = 1; i < 13; i++)
            {
                string label = "";
                int y = 50;
                y = db.MC_Student_History.Where(x => x.Date.Value.Month == i && x.MC_Student.Student_Gender == "Female").ToList().Count();

                label = dateTime.ToString("dd MMM");

                dataPoints2.Add(new DataPoint(y, label));
                dateTime = dateTime.AddMonths(1);
            }
            ViewBag.DataPoints2 = JsonConvert.SerializeObject(dataPoints2, _jsonSetting);


            for (int i = 1; i < 5; i++)
            {
                string label = "";
                int y = 50;
                y = db.MC_Student_History.Where(x => x.MC_Student.Student_Year == i && x.MC_Student.Student_Gender == "Female").ToList().Count();

                label = dateTime.ToString("dd MMM");

                dataPoints3.Add(new DataPoint(y, label, i + " year Female"));
                dateTime = dateTime.AddMonths(1);
            }
            ViewBag.DataPoints3 = JsonConvert.SerializeObject(dataPoints3, _jsonSetting);

            for (int i = 1; i < 5; i++)
            {
                string label = "";
                int y = 50;
                y = db.MC_Student_History.Where(x => x.MC_Student.Student_Year == i && x.MC_Student.Student_Gender == "Male").ToList().Count();

                label = dateTime.ToString("dd MMM");

                dataPoints4.Add(new DataPoint(y, label,i+" year Male"));
                dateTime = dateTime.AddMonths(1);
            }
            ViewBag.DataPoints4 = JsonConvert.SerializeObject(dataPoints4, _jsonSetting);

            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult chart()
        {
            ArrayList xValue = new ArrayList();
            ArrayList yValue = new ArrayList();
            var de = db.MC_Student.Select(c => c.Student_Gender).ToList();
            var year = db.MC_Student.Select(c => c.Student_Year).ToList();
            foreach (var item in de)
            {
                xValue.Add(item);

            }
            foreach (var item in year)
            {
                yValue.Add(item);

            }
            new Chart(width: 1000, height: 400, theme: ChartTheme.Blue).AddTitle("Chart of year wise")
                .AddSeries("Default", chartType: "Column", xValue: xValue, yValues: yValue).Write("jpeg");


            return null;
        }

        public ActionResult Barchart()
        {
            ArrayList xValue = new ArrayList();
            ArrayList yValue = new ArrayList();
            var de = db.MC_Student.Select(c => c.Student_Gender).ToList();
            var year = db.MC_Student.Select(c => c.Student_Year).ToList();
            foreach (var item in de)
            {
                xValue.Add(item);

            }
            foreach (var item in year)
            {
                yValue.Add(item);

            }
            new Chart(width: 400, height: 400, theme: ChartTheme.Blue).AddTitle("Chart of gender wise")
                .AddSeries("Default", chartType: "Bar", xValue: xValue, yValues: yValue).Write("bmp");


            return null;
        }
        public ActionResult Piechart()
        {
            ArrayList xValue = new ArrayList();
            ArrayList yValue = new ArrayList();
            List<string> faculties = new List<string>();
            var de = db.MC_Student.Select(c => c.Student_Faculty).Distinct().ToList();
            List<int> pks = db.MC_Student_History.Where(c => c.Date.Value.Month == DateTime.Now.Month).Select(c => c.Student_Pk).ToList();
            foreach (var pk in pks)
            {
                var faculty = db.MC_Student.Where(c => c.Student_Pk == pk).Select(v => v.Student_Faculty).ToString();
                var hh = db.MC_Student.FirstOrDefault(c => c.Student_Pk == pk).Student_Faculty;
                faculties.Add(hh);
            }
            foreach (var item in de)
            {
                var count = faculties.Where(c => c.Contains(item)).Count();
                //var co = faculties.OrderBy().

                xValue.Add(item);
                yValue.Add(count);

            }


            //foreach (var item in year)
            //{
            //    yValue.Add(item);

            //}
            new Chart(width: 400, height: 400, theme: ChartTheme.Blue).AddTitle("Chart of faculties")
                .AddSeries("Default", chartType: "pie", xValue: xValue, yValues: yValue).Write("jpeg");


            return null;
        }

       

        public ActionResult threechart()
        {
            ArrayList xValue = new ArrayList();
            ArrayList yValue = new ArrayList();
            var de = db.MC_Student.Select(c => c.Student_Gender).ToList();
            var year = db.MC_Student.Select(c => c.Student_Year).ToList();
            foreach (var item in de)
            {
                xValue.Add(item);

            }
            foreach (var item in year)
            {
                yValue.Add(item);

            }
            new Chart(width: 800, height: 200, theme: ChartTheme.Blue).AddTitle("Chart for Gender")
                .AddSeries("Default", chartType: "Candlestick", xValue: xValue, yValues: yValue).Write("bmp");


            return null;
        }
        /// <summary>
        /// Get student details and view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult StudentDetails()
        {
            var students = db.MC_Student.ToList();



            return View(students);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectedtextid"></param>
        /// <param name="selectedvalue"></param>
        /// <returns></returns>
        public JsonResult AddSessionstudentdetails(string selectedtextid, string selectedvalue)
        {

            Session["selectid"] = selectedtextid;
            Session["selectval"] = selectedvalue;



            return Json(new { Result = "OK" });
        }

        public JsonResult showstudentdetails(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {

            try
            {
                if (Session["selectid"] == null && Session["selectval"] == null)
                {
                    Session["selectid"] = "";
                    Session["selectval"] = "";
                }



                string dk = Session["selectid"].ToString();

                string bb = Session["selectval"].ToString();





                List<StudentDetails> dd = new List<StudentDetails>();
                // var studetdetails =db.MC_Student.ToList();

                if (dk == "Student Name")
                {
                    var studentde1 = db.MC_Student.Where(c => c.Student_Name.Contains(bb)).ToList();
                    var studentde = studentde1.Skip(jtStartIndex).Take(jtPageSize).ToList();
                    int totalrecord = studentde1.Count();


                    foreach (var item in studentde)
                    {
                        StudentDetails aa = new StudentDetails();

                        aa.studentname = item.Student_Name;
                        aa.studentregno = item.Student_RegNo;
                        aa.nicno = item.Student_NicNo;
                        aa.gender = item.Student_Gender;
                        aa.age = item.Student_Age;
                        aa.dataofbirth = item.Student_DateOfBirth;
                        aa.faculty = item.Student_Faculty;
                        aa.year = item.Student_Year;
                        aa.rationanal = item.Student_Rational;
                        aa.mothername = item.Student_MotherName;
                        aa.fathernmae = item.Student_FatherName;
                        aa.familymembers = item.Student_FamilyMembers;

                        dd.Add(aa);
                        switch (jtSorting)
                        {
                            case "studentname ASC":
                                dd = dd.OrderBy(t => t.studentname).ToList();
                                break;

                            case "studentname DESC":
                                dd = dd.OrderByDescending(t => t.studentname).ToList();
                                break;

                            case "studentregno ASC":
                                dd = dd.OrderBy(t => t.studentregno).ToList();
                                break;

                            case "studentregno DESC":
                                dd = dd.OrderByDescending(t => t.studentregno).ToList();
                                break;

                            case "nicno ASC":
                                dd = dd.OrderBy(t => t.nicno).ToList();
                                break;

                            case "nicno DESC":
                                dd = dd.OrderByDescending(t => t.nicno).ToList();
                                break;

                            case "gender ASC":
                                dd = dd.OrderBy(t => t.gender).ToList();
                                break;

                            case "gender DESC":
                                dd = dd.OrderByDescending(t => t.gender).ToList();
                                break;

                            case "age ASC":
                                dd = dd.OrderBy(t => t.age).ToList();
                                break;

                            case "age DESC":
                                dd = dd.OrderByDescending(t => t.age).ToList();
                                break;

                            case "dataofbirth ASC":
                                dd = dd.OrderBy(t => t.dataofbirth).ToList();
                                break;

                            case "dataofbirth DESC":
                                dd = dd.OrderByDescending(t => t.dataofbirth).ToList();
                                break;

                            case "faculty ASC":
                                dd = dd.OrderBy(t => t.faculty).ToList();
                                break;

                            case "faculty DESC":
                                dd = dd.OrderByDescending(t => t.faculty).ToList();
                                break;

                            case "year ASC":
                                dd = dd.OrderBy(t => t.year).ToList();
                                break;

                            case "year DESC":
                                dd = dd.OrderByDescending(t => t.year).ToList();
                                break;



                        }


                    }
                    return Json(new { Result = "OK", Records = dd, TotalRecordCount = totalrecord });

                }

                else if (dk == "Student RegNo")
                {
                    var std1 = db.MC_Student.Where(c => c.Student_RegNo.Contains(bb)).ToList();
                    var std = std1.Skip(jtStartIndex).Take(jtPageSize).ToList();
                    int totalrecord = std1.Count();

                    foreach (var item in std)
                    {
                        StudentDetails aa = new StudentDetails();

                        aa.studentname = item.Student_Name;
                        aa.studentregno = item.Student_RegNo;
                        aa.nicno = item.Student_NicNo;
                        aa.gender = item.Student_Gender;
                        aa.age = item.Student_Age;
                        aa.dataofbirth = item.Student_DateOfBirth;
                        aa.faculty = item.Student_Faculty;
                        aa.year = item.Student_Year;
                        aa.rationanal = item.Student_Rational;
                        aa.mothername = item.Student_MotherName;
                        aa.fathernmae = item.Student_FatherName;
                        aa.familymembers = item.Student_FamilyMembers;

                        dd.Add(aa);
                        switch (jtSorting)
                        {
                            case "studentname ASC":
                                dd = dd.OrderBy(t => t.studentname).ToList();
                                break;

                            case "studentname DESC":
                                dd = dd.OrderByDescending(t => t.studentname).ToList();
                                break;

                            case "studentregno ASC":
                                dd = dd.OrderBy(t => t.studentregno).ToList();
                                break;

                            case "studentregno DESC":
                                dd = dd.OrderByDescending(t => t.studentregno).ToList();
                                break;

                            case "nicno ASC":
                                dd = dd.OrderBy(t => t.nicno).ToList();
                                break;

                            case "nicno DESC":
                                dd = dd.OrderByDescending(t => t.nicno).ToList();
                                break;

                            case "gender ASC":
                                dd = dd.OrderBy(t => t.gender).ToList();
                                break;

                            case "gender DESC":
                                dd = dd.OrderByDescending(t => t.gender).ToList();
                                break;

                            case "age ASC":
                                dd = dd.OrderBy(t => t.age).ToList();
                                break;

                            case "age DESC":
                                dd = dd.OrderByDescending(t => t.age).ToList();
                                break;

                            case "dataofbirth ASC":
                                dd = dd.OrderBy(t => t.dataofbirth).ToList();
                                break;

                            case "dataofbirth DESC":
                                dd = dd.OrderByDescending(t => t.dataofbirth).ToList();
                                break;

                            case "faculty ASC":
                                dd = dd.OrderBy(t => t.faculty).ToList();
                                break;

                            case "faculty DESC":
                                dd = dd.OrderByDescending(t => t.faculty).ToList();
                                break;

                            case "year ASC":
                                dd = dd.OrderBy(t => t.year).ToList();
                                break;

                            case "year DESC":
                                dd = dd.OrderByDescending(t => t.year).ToList();
                                break;



                        }

                    }

                    return Json(new { Result = "OK", Records = dd, TotalRecordCount = totalrecord });
                }

                else if (dk == "Faculty Name")
                {
                    var dt1 = db.MC_Student.Where(c => c.Student_Faculty.Contains(bb)).ToList();
                    var dt = dt1.Skip(jtStartIndex).Take(jtPageSize).ToList();
                    int totalrecord = dt1.Count();

                    foreach (var item in dt)
                    {
                        StudentDetails aa = new StudentDetails();

                        aa.studentname = item.Student_Name;
                        aa.studentregno = item.Student_RegNo;
                        aa.nicno = item.Student_NicNo;
                        aa.gender = item.Student_Gender;
                        aa.age = item.Student_Age;
                        aa.dataofbirth = item.Student_DateOfBirth;
                        aa.faculty = item.Student_Faculty;
                        aa.year = item.Student_Year;
                        aa.rationanal = item.Student_Rational;
                        aa.mothername = item.Student_MotherName;
                        aa.fathernmae = item.Student_FatherName;
                        aa.familymembers = item.Student_FamilyMembers;

                        dd.Add(aa);

                        switch (jtSorting)
                        {
                            case "studentname ASC":
                                dd = dd.OrderBy(t => t.studentname).ToList();
                                break;

                            case "studentname DESC":
                                dd = dd.OrderByDescending(t => t.studentname).ToList();
                                break;

                            case "studentregno ASC":
                                dd = dd.OrderBy(t => t.studentregno).ToList();
                                break;

                            case "studentregno DESC":
                                dd = dd.OrderByDescending(t => t.studentregno).ToList();
                                break;

                            case "nicno ASC":
                                dd = dd.OrderBy(t => t.nicno).ToList();
                                break;

                            case "nicno DESC":
                                dd = dd.OrderByDescending(t => t.nicno).ToList();
                                break;

                            case "gender ASC":
                                dd = dd.OrderBy(t => t.gender).ToList();
                                break;

                            case "gender DESC":
                                dd = dd.OrderByDescending(t => t.gender).ToList();
                                break;

                            case "age ASC":
                                dd = dd.OrderBy(t => t.age).ToList();
                                break;

                            case "age DESC":
                                dd = dd.OrderByDescending(t => t.age).ToList();
                                break;

                            case "dataofbirth ASC":
                                dd = dd.OrderBy(t => t.dataofbirth).ToList();
                                break;

                            case "dataofbirth DESC":
                                dd = dd.OrderByDescending(t => t.dataofbirth).ToList();
                                break;

                            case "faculty ASC":
                                dd = dd.OrderBy(t => t.faculty).ToList();
                                break;

                            case "faculty DESC":
                                dd = dd.OrderByDescending(t => t.faculty).ToList();
                                break;

                            case "year ASC":
                                dd = dd.OrderBy(t => t.year).ToList();
                                break;

                            case "year DESC":
                                dd = dd.OrderByDescending(t => t.year).ToList();
                                break;



                        }


                    }

                    return Json(new { Result = "OK", Records = dd, TotalRecordCount = totalrecord });

                }

                else
                {
                    int totalrecord = 0;
                    Nullable<System.DateTime> Datenow = DateTime.Now.Date;
                    var studetdetail = db.MC_Student_History.Where(c => c.Date == Datenow).ToList();
                    var studetdetails1 = db.MC_Student.ToList();

                    var studets1 = db.MC_Student_History.Where(c => c.Date == Datenow).Join(db.MC_Student, sc => sc.Student_Pk, s => s.Student_Pk, (sc, s) => new { sc, s })
                        .Select(m => new
                        {
                            Student_Name = m.s.Student_Name,
                            Student_NicNo = m.s.Student_NicNo,
                            Student_Pk = m.s.Student_Pk,
                            Student_Faculty = m.s.Student_Faculty,
                            Student_Gender = m.s.Student_Gender,
                            Student_Age=m.s.Student_Age,
                            Student_DateOfBirth=m.s.Student_DateOfBirth,
                            Student_Year=m.s.Student_Year,
                            Student_Rational=m.s.Student_Rational,
                            Student_MotherName=m.s.Student_MotherName,
                            Student_FatherName=m.s.Student_FatherName,
                            Student_FamilyMembers=m.s.Student_FamilyMembers




                        }).OrderBy(l => l.Student_Pk).Distinct();
                    MC_Student rr = new MC_Student();
                    List<MC_Student> yy = new List<MC_Student>();
                    foreach (var item in studets1)
                    {
                        rr.Student_Name = item.Student_Name;
                        rr.Student_NicNo = item.Student_NicNo;
                        rr.Student_Pk = item.Student_Pk;
                        rr.Student_Faculty = item.Student_Faculty;
                        yy.Add(rr);
                    }
                    if (studetdetail.Count()>0)
                    {
                        var studetdetails = yy.Skip(jtStartIndex).Take(jtPageSize).ToList();
                        totalrecord = studets1.Count();

                        foreach (var item in studets1)
                        {
                            StudentDetails aa = new StudentDetails();

                            aa.studentname = item.Student_Name;
                            aa.studentregno = item.Student_NicNo;
                            aa.nicno = item.Student_NicNo;
                            aa.gender = item.Student_Gender;
                            aa.age = item.Student_Age;
                            aa.dataofbirth = item.Student_DateOfBirth;
                            aa.faculty = item.Student_Faculty;
                            aa.year = item.Student_Year;
                            aa.rationanal = item.Student_Rational;
                            aa.mothername = item.Student_MotherName;
                            aa.fathernmae = item.Student_FatherName;
                            aa.familymembers = item.Student_FamilyMembers;

                            dd.Add(aa);

                        }

                    }


                    switch (jtSorting)
                    {
                        case "studentname ASC":
                            dd = dd.OrderBy(t => t.studentname).ToList();
                            break;

                        case "studentname DESC":
                            dd = dd.OrderByDescending(t => t.studentname).ToList();
                            break;

                        case "studentregno ASC":
                            dd = dd.OrderBy(t => t.studentregno).ToList();
                            break;

                        case "studentregno DESC":
                            dd = dd.OrderByDescending(t => t.studentregno).ToList();
                            break;

                        case "nicno ASC":
                            dd = dd.OrderBy(t => t.nicno).ToList();
                            break;

                        case "nicno DESC":
                            dd = dd.OrderByDescending(t => t.nicno).ToList();
                            break;

                        case "gender ASC":
                            dd = dd.OrderBy(t => t.gender).ToList();
                            break;

                        case "gender DESC":
                            dd = dd.OrderByDescending(t => t.gender).ToList();
                            break;

                        case "age ASC":
                            dd = dd.OrderBy(t => t.age).ToList();
                            break;

                        case "age DESC":
                            dd = dd.OrderByDescending(t => t.age).ToList();
                            break;

                        case "dataofbirth ASC":
                            dd = dd.OrderBy(t => t.dataofbirth).ToList();
                            break;

                        case "dataofbirth DESC":
                            dd = dd.OrderByDescending(t => t.dataofbirth).ToList();
                            break;

                        case "faculty ASC":
                            dd = dd.OrderBy(t => t.faculty).ToList();
                            break;

                        case "faculty DESC":
                            dd = dd.OrderByDescending(t => t.faculty).ToList();
                            break;

                        case "year ASC":
                            dd = dd.OrderBy(t => t.year).ToList();
                            break;

                        case "year DESC":
                            dd = dd.OrderByDescending(t => t.year).ToList();
                            break;



                    }
                    return Json(new { Result = "OK", Records = dd, TotalRecordCount = totalrecord });

                }



            }




            catch (Exception)
            {

                throw;
            }



        }

        [HttpGet]
        public ActionResult AddStudent()
        {
            AddStudent detasil = new Models.AddStudent();
            var Faculties = db.MC_Student.Select(c => c.Student_Faculty).Distinct().ToList();
            var genders = db.MC_Student.Select(c => c.Student_Gender).Distinct().ToList();
            //detasil.Student_Gender = genders;
            //detasil.Student_Faculty = Faculties;

            return View();
        }

        [HttpPost]
        public ActionResult AddStudent(AddStudent details)
        {
            DateTime tt = DateTime.Now;
            DateTime ti = DateTime.Now;
            MC_Student dd = new MC_Student();
            if (details.Student_DateOfBirth!=null)
            {
                tt = DateTime.Now;
                ti = details.Student_DateOfBirth.Value;
            }
           
            if (!ModelState.IsValid)
            {
                return View(details);
            }
            else if (ti>tt)
            {
                ViewBag.dateError = "Please select Valid date";
                return View(details);
            }
            else
            {

               


                dd.Student_Name = details.Student_Name;
                dd.Student_RegNo = details.Student_RegNo;
                dd.Student_NicNo = details.Student_NicNo;
                dd.Student_Age = details.Student_Age;
                dd.Student_DateOfBirth = details.Student_DateOfBirth;
                dd.Student_Gender = details.Student_Gender.ToString();
                dd.Student_Faculty = details.Student_Faculty.ToString();
                dd.Student_Year = details.Student_Year;
                dd.Student_Rational = details.Student_Rational;
                dd.Student_FamilyMembers = details.Student_FamilyMembers;
                dd.Student_MotherName = details.Student_MotherName;
                dd.Student_FatherName = details.Student_FatherName;

                db.MC_Student.Add(dd);
                db.SaveChanges();

                ViewBag.Message = "Successfully save student details";
                return View();
            }
        }

        [HttpGet]
        public ActionResult DrugStoke()
        {
            return View();
        }


        [HttpPost]
        public JsonResult getstudentslist(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            AddStudent hh = new Models.AddStudent();
            List<AddStudent> hd = new List<AddStudent>();

            var students = db.MC_Student.ToList();
            students = students.Skip(jtStartIndex).Take(jtPageSize).ToList();
            int totalstudent = db.MC_Student.Count();

            try
            {
                foreach (var item in students)
                {
                    hh.Student_RegNo = item.Student_RegNo;
                    hh.Student_Name = item.Student_Name;
                    // hh.Student_Gender = item.Student_Gender;
                    hh.Student_NicNo = item.Student_NicNo;
                    hh.Student_Age = item.Student_Age;

                    hd.Add(hh);
                }


                return Json(new { Result = "OK", Records = hd, TotalRecordCount = totalstudent });
            }
            catch (Exception ex)
            {

                throw;

            }

        }

        [HttpPost]
        public JsonResult Todaypatients(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {

                List<StudentHistoryDT> ddl = new List<StudentHistoryDT>();


                List<MC_Student_History> studenthistory = db.MC_Student_History
                    .Where(c => c.Date == DateTime.Now.Date).ToList();

                studenthistory = studenthistory.Skip(jtStartIndex).Take(jtPageSize).ToList();
                int totalrecord = studenthistory.Count();



                foreach (var item in studenthistory)
                {
                    StudentHistoryDT dd = new StudentHistoryDT();
                    dd.date = item.Date;
                    dd.disease = item.Disease;
                    dd.descrition = item.Description;
                    dd.medical = item.Medical;
                    dd.drugname = db.MC_Drug_History.Where(c => c.Student_History_Pk == item.Student_History_Pk).Select(c => c.Drug_Name).ToList();
                    dd.drugqty = db.MC_Drug_History.Where(c => c.Student_History_Pk == item.Student_History_Pk).Select(c => c.Drug_Qty).ToList();

                    ddl.Add(dd);

                }

                switch (jtSorting)
                {
                    case "date ASC":
                        ddl = ddl.OrderBy(t => t.date).ToList();
                        break;

                    case "date DESC":
                        ddl = ddl.OrderByDescending(t => t.date).ToList();
                        break;

                    case "disease ASC":
                        ddl = ddl.OrderBy(t => t.disease).ToList();
                        break;

                    case "disease DESC":
                        ddl = ddl.OrderByDescending(t => t.disease).ToList();
                        break;

                    case "descrition ASC":
                        ddl = ddl.OrderBy(t => t.descrition).ToList();
                        break;

                    case "descrition DESC":
                        ddl = ddl.OrderByDescending(t => t.descrition).ToList();
                        break;

                    case "drugname ASC":
                        ddl = ddl.OrderBy(t => t.drugname).ToList();
                        break;

                    case "drugname DESC":
                        ddl = ddl.OrderByDescending(t => t.drugname).ToList();
                        break;


                    case "medical ASC":
                        ddl = ddl.OrderBy(t => t.medical).ToList();
                        break;

                    case "medical DESC":
                        ddl = ddl.OrderByDescending(t => t.medical).ToList();
                        break;

                }


                return Json(new { Result = "OK", Records = ddl, TotalRecordCount = totalrecord });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }



        [HttpGet]
        public ActionResult StudentHistory()
        {

            return View();
        }

        [HttpPost]
        public JsonResult GetstudentHistory(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {

            try
            {
                if (Session["selectid"] == null && Session["selectval"] == null)
                {
                    Session["selectid"] = "";
                    Session["selectval"] = "";
                }



                string dk = Session["selectid"].ToString();

                string bb = Session["selectval"].ToString();

                List<StudentHistoryDT> ddl = new List<StudentHistoryDT>();


                List<MC_Student> studenthistory = db.MC_Student.ToList();

                studenthistory = studenthistory.Skip(jtStartIndex).Take(jtPageSize).ToList();
                int totalrecord = db.MC_Student_History.Count();



                foreach (var item in studenthistory)
                {
                    StudentHistoryDT dd = new StudentHistoryDT();
                    dd.studentname = item.Student_Name;
                    dd.nicno = item.Student_NicNo;
                    dd.studentregno = item.Student_RegNo;
                    dd.faculty = item.Student_Faculty;
                    dd.year = item.Student_Year;
                    dd.rationanal = item.Student_Rational;
                    dd.familymembers = item.Student_FamilyMembers;
                    dd.mothername = item.Student_MotherName;
                    dd.fathernmae = item.Student_FatherName;


                    ddl.Add(dd);


                }

                switch (jtSorting)
                {
                    case "studentname ASC":
                        ddl = ddl.OrderBy(t => t.studentname).ToList();
                        break;

                    case "studentname DESC":
                        ddl = ddl.OrderByDescending(t => t.studentname).ToList();
                        break;

                    case "nicno ASC":
                        ddl = ddl.OrderBy(t => t.nicno).ToList();
                        break;

                    case "nicno DESC":
                        ddl = ddl.OrderByDescending(t => t.nicno).ToList();
                        break;

                    case "studentregno ASC":
                        ddl = ddl.OrderBy(t => t.studentregno).ToList();
                        break;

                    case "studentregno DESC":
                        ddl = ddl.OrderByDescending(t => t.studentregno).ToList();
                        break;

                    case "faculty ASC":
                        ddl = ddl.OrderBy(t => t.faculty).ToList();
                        break;

                    case "faculty DESC":
                        ddl = ddl.OrderByDescending(t => t.faculty).ToList();
                        break;


                    case " year ASC":
                        ddl = ddl.OrderBy(t => t.year).ToList();
                        break;

                    case " year DESC":
                        ddl = ddl.OrderByDescending(t => t.year).ToList();
                        break;

                }

                // var  studenthistory1 = db.MC_Student_History.Where(c => c.Student_Pk == student.Student_Pk).ToList();
                return Json(new { Result = "OK", Records = ddl, TotalRecordCount = totalrecord });
            }
            catch (Exception ex)
            {

                throw ex;

            }

        }


        [HttpPost]
        public JsonResult DeleteStudent(string nicno)
        {
            try
            {



                //MC_Student studentdelete = db.MC_Student.Where(c => c.Student_NicNo == nicno)
                //.Select(record => record).FirstOrDefault();

                MC_Student studentdelete = db.MC_Student.Where(c => c.Student_NicNo == nicno).FirstOrDefault();


                // List<MC_Student_History> deletehistory = db.MC_Student_History
                //     .Where(c => c.Student_Pk == studentdelete.Student_Pk).ToList();


                //List<MC_Drug_History> drugdel = db.MC_Drug_History.Where(c=>c.Student_History_Pk == studentdelete.)

                // foreach (var item in deletehistory)
                // {

                //     db.MC_Student_History.Remove(item);


                // }

                db.MC_Student.Remove(studentdelete);

                db.SaveChanges();

                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult medidetails(string nicno, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {

                List<StudentHistoryDT> ddl = new List<StudentHistoryDT>();


                List<MC_Student_History> studenthistory = db.MC_Student_History
                    .Where(c => c.MC_Student.Student_NicNo == nicno).ToList();

                studenthistory = studenthistory.Skip(jtStartIndex).Take(jtPageSize).ToList();
                int totalrecord = studenthistory.Count();



                foreach (var item in studenthistory)
                {
                    StudentHistoryDT dd = new StudentHistoryDT();
                    dd.date = item.Date;
                    dd.disease = item.Disease;
                    dd.descrition = item.Description;
                    dd.medical = item.Medical;
                    dd.drugname = db.MC_Drug_History.Where(c => c.Student_History_Pk == item.Student_History_Pk).Select(c => c.Drug_Name).ToList();
                    dd.drugqty = db.MC_Drug_History.Where(c => c.Student_History_Pk == item.Student_History_Pk).Select(c => c.Drug_Qty).ToList();

                    ddl.Add(dd);

                }

                switch (jtSorting)
                {
                    case "date ASC":
                        ddl = ddl.OrderBy(t => t.date).ToList();
                        break;

                    case "date DESC":
                        ddl = ddl.OrderByDescending(t => t.date).ToList();
                        break;

                    case "disease ASC":
                        ddl = ddl.OrderBy(t => t.disease).ToList();
                        break;

                    case "disease DESC":
                        ddl = ddl.OrderByDescending(t => t.disease).ToList();
                        break;

                    case "descrition ASC":
                        ddl = ddl.OrderBy(t => t.descrition).ToList();
                        break;

                    case "descrition DESC":
                        ddl = ddl.OrderByDescending(t => t.descrition).ToList();
                        break;

                    case "drugname ASC":
                        ddl = ddl.OrderBy(t => t.drugname).ToList();
                        break;

                    case "drugname DESC":
                        ddl = ddl.OrderByDescending(t => t.drugname).ToList();
                        break;


                    case "medical ASC":
                        ddl = ddl.OrderBy(t => t.medical).ToList();
                        break;

                    case "medical DESC":
                        ddl = ddl.OrderByDescending(t => t.medical).ToList();
                        break;

                }


                return Json(new { Result = "OK", Records = ddl, TotalRecordCount = totalrecord });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Deletemedi(string nicno)
        {
            try
            {


                MC_Student studentdelete = db.MC_Student.Where(c => c.Student_NicNo == nicno)
                    .Select(record => record).FirstOrDefault();


                MC_Student_History deletehistory = db.MC_Student_History.Where(c => c.Student_Pk == studentdelete.Student_Pk)
                .Select(record => record).FirstOrDefault();

                db.MC_Student_History.Remove(deletehistory);
                db.SaveChanges();

                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        [HttpGet]
        public ActionResult drugstock()
        {
            return View();
        }

        public JsonResult AddSessiondrugtdetails(string selectedtextid, string selectedvalue)
        {

            Session["selectid"] = selectedtextid;
            Session["selectval"] = selectedvalue;



            return Json(new { Result = "OK" });
        }



        [HttpPost]
        public JsonResult drugdetails(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            List<DrugStockDTO> ddl = new List<DrugStockDTO>();




            if (Session["selectid"] == null && Session["selectval"] == null)
            {
                Session["selectid"] = "";
                Session["selectval"] = "";
            }



            string dk = Session["selectid"].ToString();

            string bb = Session["selectval"].ToString();





            if (dk == "Drug Code")
            {

                var drgstk1 = db.MC_Ddrug_Stoke.Where(c => c.Drug_Code.Contains(bb)).ToList();
                var drgstk = drgstk1.Skip(jtStartIndex).Take(jtPageSize).ToList();
                int totalrecord = drgstk1.Count();


                foreach (var item in drgstk)
                {
                    DrugStockDTO aa = new DrugStockDTO();
                    aa.drugcode = item.Drug_Code;
                    aa.drugname = item.Drug_Name;
                    aa.receivedate = item.Recevive_Date;
                    aa.exipiredate = item.Expire_Date;
                    aa.quantity = item.Quantitty;
                    aa.unit = item.Unit;
                    aa.balance = db.MC_Drug_Balance.Where(c => c.Drug_Code == item.Drug_Code).Select(c => c.Balance).FirstOrDefault();

                    ddl.Add(aa);


                    switch (jtSorting)
                    {
                        case "drugname ASC":
                            ddl = ddl.OrderBy(t => t.drugname).ToList();
                            break;

                        case "drugname DESC":
                            ddl = ddl.OrderByDescending(t => t.drugname).ToList();
                            break;

                        case "drugcode ASC":
                            ddl = ddl.OrderBy(t => t.drugcode).ToList();
                            break;

                        case "drugcode DESC":
                            ddl = ddl.OrderByDescending(t => t.drugcode).ToList();
                            break;

                        case "receivedate ASC":
                            ddl = ddl.OrderBy(t => t.receivedate).ToList();
                            break;

                        case "receivedate DESC":
                            ddl = ddl.OrderByDescending(t => t.receivedate).ToList();
                            break;

                        case "exipiredate ASC":
                            ddl = ddl.OrderBy(t => t.exipiredate).ToList();
                            break;

                        case "exipiredate DESC":
                            ddl = ddl.OrderByDescending(t => t.exipiredate).ToList();
                            break;

                    }

                }

                return Json(new { Result = "OK", Records = ddl, TotalRecordCount = totalrecord });

            }

            else if (dk == "Drug Descrition")
            {

                var drgstk1 = db.MC_Ddrug_Stoke.Where(c => c.Drug_Name.Contains(bb)).ToList();
                var drgstk = drgstk1.Skip(jtStartIndex).Take(jtPageSize).ToList();
                int totalrecord = drgstk1.Count();


                foreach (var item in drgstk)
                {
                    DrugStockDTO aa = new DrugStockDTO();
                    aa.drugcode = item.Drug_Code;
                    aa.drugname = item.Drug_Name;
                    aa.receivedate = item.Recevive_Date;
                    aa.exipiredate = item.Expire_Date;
                    aa.quantity = item.Quantitty;
                    aa.unit = item.Unit;
                    aa.balance = db.MC_Drug_Balance.Where(c => c.Drug_Code == item.Drug_Code).Select(c => c.Balance).FirstOrDefault();

                    ddl.Add(aa);


                    switch (jtSorting)
                    {
                        case "drugname ASC":
                            ddl = ddl.OrderBy(t => t.drugname).ToList();
                            break;

                        case "drugname DESC":
                            ddl = ddl.OrderByDescending(t => t.drugname).ToList();
                            break;

                        case "drugcode ASC":
                            ddl = ddl.OrderBy(t => t.drugcode).ToList();
                            break;

                        case "drugcode DESC":
                            ddl = ddl.OrderByDescending(t => t.drugcode).ToList();
                            break;

                        case "receivedate ASC":
                            ddl = ddl.OrderBy(t => t.receivedate).ToList();
                            break;

                        case "receivedate DESC":
                            ddl = ddl.OrderByDescending(t => t.receivedate).ToList();
                            break;

                        case "exipiredate ASC":
                            ddl = ddl.OrderBy(t => t.exipiredate).ToList();
                            break;

                        case "exipiredate DESC":
                            ddl = ddl.OrderByDescending(t => t.exipiredate).ToList();
                            break;

                    }

                }

                return Json(new { Result = "OK", Records = ddl, TotalRecordCount = totalrecord });

            }

            else
            {

                var drgstk1 = db.MC_Ddrug_Stoke.ToList();
                var drgstk = drgstk1.Skip(jtStartIndex).Take(jtPageSize).ToList();
                int totalrecord = drgstk1.Count();


                foreach (var item in drgstk)
                {
                    DrugStockDTO aa = new DrugStockDTO();
                    aa.drugcode = item.Drug_Code;
                    aa.drugname = item.Drug_Name;
                    aa.receivedate = item.Recevive_Date;
                    aa.exipiredate = item.Expire_Date;
                    aa.quantity = item.Quantitty;
                    aa.unit = item.Unit;
                    aa.balance = db.MC_Drug_Balance.Where(c => c.Drug_Code == item.Drug_Code).Select(c => c.Balance).FirstOrDefault();

                    ddl.Add(aa);


                    switch (jtSorting)
                    {
                        case "drugname ASC":
                            ddl = ddl.OrderBy(t => t.drugname).ToList();
                            break;

                        case "drugname DESC":
                            ddl = ddl.OrderByDescending(t => t.drugname).ToList();
                            break;

                        case "drugcode ASC":
                            ddl = ddl.OrderBy(t => t.drugcode).ToList();
                            break;

                        case "drugcode DESC":
                            ddl = ddl.OrderByDescending(t => t.drugcode).ToList();
                            break;

                        case "receivedate ASC":
                            ddl = ddl.OrderBy(t => t.receivedate).ToList();
                            break;

                        case "receivedate DESC":
                            ddl = ddl.OrderByDescending(t => t.receivedate).ToList();
                            break;

                        case "exipiredate ASC":
                            ddl = ddl.OrderBy(t => t.exipiredate).ToList();
                            break;

                        case "exipiredate DESC":
                            ddl = ddl.OrderByDescending(t => t.exipiredate).ToList();
                            break;

                    }

                }

                return Json(new { Result = "OK", Records = ddl, TotalRecordCount = totalrecord });

            }



        }




        [HttpPost]
        public JsonResult Deletedrugdetils(string drugcode)
        {
            try
            {

                MC_Ddrug_Stoke drugdelate = db.MC_Ddrug_Stoke.Where(c => c.Drug_Code == drugcode).
                Select(record => record).FirstOrDefault();
                db.MC_Ddrug_Stoke.Remove(drugdelate);
                db.SaveChanges();

                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult drugdetailsaad(string code)
        {
            Session["drugcode"] = code;
            DrugStockDTO fr = new DrugStockDTO();
            if (code == null)
            {
                return View();
            }
            else
            {
                var tt = db.MC_Ddrug_Stoke.Where(c => c.Drug_Code == code).FirstOrDefault();
                var ti = db.MC_Drug_Balance.Where(c => c.Drug_Code == code).FirstOrDefault();
                fr.balance = ti.Balance;
                fr.drugcode = code;
                fr.exipiredate = tt.Expire_Date;
                fr.quantity = tt.Quantitty;
                fr.receivedate = tt.Recevive_Date;
                fr.unit = tt.Unit;
                fr.drugname = tt.Drug_Name;
            }
            return View(fr);
        }

        [HttpPost]
        public ActionResult drugdetailsaad(DrugStockDTO drug)
        {
            DateTime recivedate = DateTime.Now;
            DateTime expiredate = DateTime.Now;
            MC_Student dd = new MC_Student();
            if (drug.receivedate != null)
            {
                recivedate = drug.receivedate.Value;
                expiredate = drug.exipiredate.Value;
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            else if (expiredate<recivedate)
            {
                ViewBag.dateError = "Please select Valid date";
                return View(drug);
            }

            else {
                if (Session["drugcode"] == null)
                {
                    MC_Ddrug_Stoke aa = new MC_Ddrug_Stoke();
                    aa.Drug_Code = drug.drugcode;
                    aa.Drug_Name = drug.drugname;
                    aa.Recevive_Date = drug.receivedate;
                    aa.Expire_Date = drug.exipiredate;
                    aa.Quantitty = drug.quantity;
                    aa.Unit = drug.unit;

                    db.MC_Ddrug_Stoke.Add(aa);
                    db.SaveChanges();


                    MC_Drug_Balance bb = new MC_Drug_Balance();
                    bb.Drug_Code = drug.drugcode;
                    bb.Balance = drug.quantity;

                    db.MC_Drug_Balance.Add(bb);
                    db.SaveChanges();
                    return View("drugstock");

                }
                else
                {
                    string code = (string)Session["drugcode"];
                    DrugStockDTO fr = new DrugStockDTO();

                    var tt = db.MC_Ddrug_Stoke.Where(c => c.Drug_Code == code).FirstOrDefault();
                    var ti = db.MC_Drug_Balance.Where(c => c.Drug_Code == code).FirstOrDefault();
                    ti.Balance = ti.Balance + drug.quantity;
                    int qty= (int)(ti.Balance + drug.quantity);
                    ti.Drug_Code = drug.drugcode;

                    tt.Drug_Code = drug.drugcode;
                    tt.Expire_Date = drug.exipiredate;
                    tt.Quantitty =qty;
                    tt.Recevive_Date = drug.receivedate;
                    tt.Drug_Name = drug.drugname;
                    tt.Unit = drug.unit;
                    db.Entry(tt).State = System.Data.Entity.EntityState.Modified;
                    db.Entry(ti).State = System.Data.Entity.EntityState.Modified;

                    db.SaveChanges();

                    return View("drugstock");
                }



            }
        }





        /// <summary>
        /// Create medicacl 
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateMedicalForm()
        {
            MedicalFormDTO FORM = new MedicalFormDTO();
            Session["Medicaltable"] = null;

            return View(FORM);
        }
        [HttpPost]
        public ActionResult CreateMedicalForm(MedicalFormDTO details)
        {
            DateTime tt = DateTime.Now;
            DateTime ti = DateTime.Now;
            if (details.MedicalDate != null)
            {
                tt = DateTime.Now;
                ti = details.MedicalDate;
            }
            //MedicalFormDTO FORM = new MedicalFormDTO();
            if (!ModelState.IsValid && Session["Medicaltable"]==null)
            {
                ViewBag.medtbl = "Please Fill required data before proceeding";
                return View(details);
            }
            else if (Session["Medicaltable"] == null) {
                ViewBag.medtbl = "Sorry you can not subbmit form without medical";
                return View(details);
            }
            else if (details.studentpk==0)
            {
                ViewBag.medtbl = "Please make sure to search and fill student before proceeding";
                return View(details);
            }
            else if (ti>tt)
            {
                ViewBag.medtbl = "Can Not make future date for medical issue date";
                return View(details);


            }
            else
            {
                MC_Student_History historyrecord = new MC_Student_History();
                historyrecord.Student_Pk = details.studentpk;
                historyrecord.Medical = "False";
                historyrecord.Date = details.MedicalDate;
                historyrecord.Disease = details.Diesese;
                historyrecord.Description = details.Description;
                db.MC_Student_History.Add(historyrecord);
                db.SaveChanges();


                int pk = db.MC_Student_History.OrderByDescending(p => p.Student_History_Pk).Select(o => o.Student_History_Pk).FirstOrDefault();
                List<DrugSessionDTO> listpremed = (List<DrugSessionDTO>)Session["Medicaltable"];
                foreach (var item in listpremed)
                {
                    MC_Drug_History drughistory = new MC_Drug_History();
                    drughistory.Student_History_Pk = pk;
                    drughistory.Drug_Name = item.DrugName;
                    drughistory.Drug_Code = item.Drugcode;
                    //drughistory.Drug_Code = Convert.ToInt32(item.Drugcode);
                    drughistory.Drug_Qty = item.DrugQty;
                    db.MC_Drug_History.Add(drughistory);
                    db.SaveChanges();

                   
                    var balance = db.MC_Drug_Balance.Where(v => v.Drug_Code == item.Drugcode).FirstOrDefault();

                    if (balance.Balance != null)
                    {
                        balance.Balance = balance.Balance - item.DrugQty;
                        
                    }


                    db.SaveChanges();


                }
                Session["Medicaltable"] = null;
                ViewBag.message = "Medical form succesfully submited";
              
                MedicalFormDTO FORM = new MedicalFormDTO();
                return View(FORM);
               // return RedirectToAction("AdminPanel", "AdminHandle");

            }


        }



        /// <summary>
        /// Make session for when adding drugs into prescription table
        /// </summary>
        /// <param name="Dcode"></param>
        /// <param name="Dname"></param>
        /// <param name="Dqty"></param>
        /// <returns></returns>
        public JsonResult AddSessionMedicine(string Dname, int Dqty)
        {

            string Dcode = Dname.Split(',').LastOrDefault();
            string Dnameonly = Dname.Split(',').FirstOrDefault();

           var valid= db.MC_Ddrug_Stoke.Where(c => c.Drug_Code == Dcode).FirstOrDefault();
            if (valid!=null)
            {
                List<DrugSessionDTO> medicine = new List<DrugSessionDTO>();

                if (Session["Medicaltable"] == null)
                {
                    DrugSessionDTO list = new DrugSessionDTO();
                    list.Drugcode = Dcode;
                    list.DrugName = Dnameonly;
                    list.DrugQty = Dqty;

                    List<DrugSessionDTO> listM = new List<DrugSessionDTO>();
                    listM.Add(list);
                    Session["Medicaltable"] = listM;
                }
                else
                {
                    List<DrugSessionDTO> listpremed = (List<DrugSessionDTO>)Session["Medicaltable"];
                    DrugSessionDTO list = new DrugSessionDTO();
                    list.Drugcode = Dcode;
                    list.DrugName = Dnameonly;
                    list.DrugQty = Dqty;

                    // List<DrugSessionDTO> listM = new List<DrugSessionDTO>();
                    listpremed.Add(list);
                    Session["Medicaltable"] = listpremed;
                }
                List<DrugSessionDTO> drugrec = (List<DrugSessionDTO>)Session["Medicaltable"];

                return Json(new { Result = "OK" });
            }
            else
            {
                ViewBag.nodrug = "Sry there is no any drug match with given drug please provide correct name";
                string err =  "Sry there is no any drug match with given drug please provide correct name";
                return Json(new { Result = err });
            }

           
        }



        /// <summary>
        /// prescription table list action method
        /// </summary>
        /// <param name="jtStartIndex"></param>
        /// <param name="jtPageSize"></param>
        /// <param name="jtSorting"></param>
        /// <returns></returns>
        public JsonResult Drugformtable(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            var drugrec = (List<DrugSessionDTO>)Session["Medicaltable"];
            int drugcount = 0;
            if (drugrec == null)
            {

            }
            else
            {
                drugcount = drugrec.Count();
            }


            return Json(new { Result = "OK", Records = drugrec, TotalRecordCount = drugcount });
        }

        [HttpPost]
        public JsonResult MedicalDeletedrugdetils(string drugcode)
        {
            try
            {

                //var drugrec = (List<DrugSessionDTO>)Session["Medicaltable"];
                //drugcode.Where(c=>c)
                //Select(record => record).FirstOrDefault();
                //db.MC_Ddrug_Stoke.Remove(drugdelate);
                //db.SaveChanges();

                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }



        /// <summary>
        /// student search autocomplete method
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public JsonResult SearchStudent(string term)
        {
            List<MedicalFormDTO> names = db.MC_Student.Where(x => x.Student_Name.Contains(term) || x.Student_RegNo.Contains(term) || x.Student_NicNo.Contains(term)).Select(y => new MedicalFormDTO
            {
                Student_NicNo = y.Student_NicNo,
                Student_RegNo = y.Student_RegNo,
                Student_Name = y.Student_Name
            }).ToList();
            List<string> recor = new List<string>();
            foreach (var item in names)
            {

                string result = item.Student_Name + ",  (" + item.Student_RegNo + ")," + item.Student_NicNo;
                recor.Add(result);
            }

            return Json(recor, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Get medicine name for auto complete
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>

        public JsonResult GetMedicine(string term)
        {

            List<MedicalFormDTO> names = db.MC_Ddrug_Stoke.Where(x => (x.Drug_Name.Contains(term) || x.Drug_Code.Contains(term))).Select(y => new MedicalFormDTO
            {
                Drugcode = y.Drug_Code,
                DrugName = y.Drug_Name

            }).ToList();
            List<string> recor = new List<string>();
            foreach (var item in names)
            {

                string result = item.DrugName + "," + item.Drugcode;
                recor.Add(result);
            }

            return Json(recor, JsonRequestBehavior.AllowGet);
        }

     
        /// <summary>
        /// check quantity
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public JsonResult checkqty(string dname, int qty)
        {
            if (dname!=null)
            {
                dname = dname.Split(',').LastOrDefault();

                var drugreq = db.MC_Drug_Balance.Where(x => x.Drug_Code == dname).FirstOrDefault();
                if (drugreq != null)
                {
                    if (drugreq.Balance >= qty)
                    {
                        return Json("OK", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var msg = "Sorry you cannot assign more than balance of " + drugreq.Balance;
                        return Json(msg, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var msg = "There is no drug name or drug code match for given drug.Please fill correctly";
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var msg = "Please provide a drug name before make qunatity";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }


        }

        /// <summary>
        /// get student details for fill form
        /// </summary>
        /// <param name="Searchkey"></param>
        /// <returns></returns>

        [HttpPost]
        public JsonResult GetStudentDetails(string Searchkey)
        {
            Searchkey = Searchkey.Split(',').LastOrDefault();
           

            var searchstudents = db.MC_Student.Where(x => x.Student_NicNo == Searchkey).FirstOrDefault();
            if (searchstudents!=null)
            {
                MedicalFormDTO record = new MedicalFormDTO();
                record.studentpk = searchstudents.Student_Pk;
                record.Student_Age = searchstudents.Student_Age;
                record.Student_Faculty = searchstudents.Student_Faculty;
                record.Student_Gender = searchstudents.Student_Gender;
                record.Student_NicNo = searchstudents.Student_NicNo;
                record.Student_Year = searchstudents.Student_Year;
                record.Student_RegNo = searchstudents.Student_RegNo;


                return Json(record);

            }
            return Json("Fail");
           
        }

        [HttpGet]
        public ActionResult ordermedical()
        {
            Session["MedicalRequesttable"] = null;
            return View();
        }

        public JsonResult AddSessionorderMedicine(string Dname, int Dqty, string Dunit)
        {
            string Dcode = null;
            List<pdf> list = new List<pdf>();
            if (Session["MedicalRequesttable"] == null)
            {
                pdf listdeta = new pdf();
                Dname = Dname.Split(',').FirstOrDefault();
                Dcode = Dname.Split(',').LastOrDefault();
                listdeta.Drugcode = Dcode;
                listdeta.DrugName = Dname;
                listdeta.DrugQty = Dqty;
                listdeta.Unit = Dunit;

                list.Add(listdeta);
                Session["MedicalRequesttable"] = list;

            }



            else
            {
                List<pdf> listpremed = (List<pdf>)Session["MedicalRequesttable"];
                pdf lista = new pdf();

                lista.DrugName = Dname;
                lista.Drugcode = Dcode;
                lista.DrugQty = Dqty;
                lista.Unit = Dunit;

                // List<DrugSessionDTO> listM = new List<DrugSessionDTO>();
                listpremed.Add(lista);
                Session["MedicalRequesttable"] = listpremed;
            }

            return Json(new { Result = "OK" });

        }

        [HttpPost]
        public JsonResult medicineselect(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            int count = 0;
            var drugrec = (List<pdf>)Session["MedicalRequesttable"];
            if (drugrec==null)
            {
                count = 0;
            }
            else
            {
                count = drugrec.Count();
            }
           
            return Json(new { Result = "OK", Records = drugrec, TotalRecordCount = count });
        }

        public ActionResult pdf()
        {
            DateTime date = DateTime.Today;
            ViewAsPdf result = new ViewAsPdf("GeneratePdf")
            {

                FileName = Server.MapPath("~/Content/Files/requestfile.pdf")

            };
            return RedirectToAction("Index", "AdminHandle");
        }

        public ActionResult GeneratePdf()
        {
            string date = DateTime.Now.ToString("MM/dd/yyyy");
            ViewBag.today = date;
            var drugrequest = (List<pdf>)Session["MedicalRequesttable"];
            List<pdf> newpdf = new List<pdf>();
            newpdf = drugrequest;
            ViewBag.requesttable = drugrequest;
            Session["MedicalRequesttable"] = null;
            ViewBag.message = "quate generate succefully";
            return new ViewAsPdf("GeneratePdf", newpdf) { FileName = "Request medicine report " + date + ".pdf" };
        }

        //public ActionResult editmedical(int code)
        //{
        //    DrugStockDTO  fr= new DrugStockDTO();
        //    //ViewBag.message = (string)Session["smesg"];

        //    return View();
        //}
        JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
    }
}