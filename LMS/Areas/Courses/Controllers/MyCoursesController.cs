using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LMS.Models;
using SelectPdf;

namespace LMS.Areas.Courses.Controllers
{
    [App_Start.SessionAuthentication]
    public class MyCoursesController : Controller
    {
        CMSEntities db = new CMSEntities();

        DateTimeProvider _dateTimeProvider = DateTimeProvider.Instance;

        // GET: Courses/MyCourses
        public async Task<ActionResult> AllCourses()
        {
            int StudentID = Convert.ToInt32(Session["StudentID"]);
            var studentCourses = db.StudentCourses.Where(t => t.StudentID == StudentID).OrderByDescending(t => t.SessionID).ToListAsync();
            return View(await studentCourses);
        }

        [HttpGet]
        public async Task<ActionResult> ApplyCourses()
        {
            int StudentID = Convert.ToInt32(Session["StudentID"]);
            int lastSemesterNumber = Methods.GetStudentCurrentSemesterNumber(StudentID);
            int batchID = Methods.GetStudentBatchID(StudentID);

            int StudentCurrentSessionID = Methods.GetStudentCurrentSessionID(StudentID);

            var newCourses = db.BatchWiseCourses.Where(t => t.BatchID == batchID && t.SemesterNo == lastSemesterNumber).ToList();

            ViewBag.NewCourses = newCourses;

            var studentCourseSelections = db.StudentCourseSelections.Where(t => t.StudentID == StudentID).Include(t => t.Session).OrderByDescending(t => t.SessionID).ToList();
            return View(studentCourseSelections);
        }


        [HttpPost]
        public ActionResult ApplyCourses(List<int> Courses)
        {          

            if (Courses != null)
            {

                int StudentID = Convert.ToInt32(Session["StudentID"]);
                int StudentCurrentSessionID = Methods.GetStudentCurrentSessionID(StudentID);

                _dateTimeProvider.Update();//get current date and time...

                int currentSemester  = Methods.GetStudentCurrentSemesterNumber(StudentID);
                var std  = db.Students.Find(StudentID);
                int stdProgramLevel = std.ProgramOffered.DepartmentProgram.Program.ProgramLevel_ID;

                //First Check if a student is in the First Semester of Bachelor, he is not allowed to register the courses through LMS. It shall be directly assigned by HoD or Admin.
                if (stdProgramLevel == 1 && currentSemester == 1)
                {
                    TempData["Failed"] = "<div class=\"alert alert-danger alert-dismissible fade in\" role=\"alert\">\r\n  " +
                               "<a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\" style=\"font-size:30px\">&times;</a>" +
                               "<strong>Failed ! </strong> Dear " + std.Admission.Title.Title1 +" "+ std.Admission.FirstName +" "+ std.Admission.LastName + ", the students of First Semester of the Bachelor Programs are not allowed to register courses using LMS." +
                               " Therefore, contact your HoD, or CMS office for further clearification/course registration.</div>";
                }
                else
                {

                    StudentCourseSelection studentCourseSelection = new StudentCourseSelection();

                    string addedCourseNames = string.Empty;
                    foreach (int courseID in Courses)
                    {

                        if (!db.StudentCourseSelections.Any(t => t.StudentID.Equals(StudentID) && t.SessionID.Equals(StudentCurrentSessionID) && t.CourseID.Equals(courseID)))
                        {
                            studentCourseSelection.StudentID = StudentID;
                            studentCourseSelection.SessionID = StudentCurrentSessionID;
                            studentCourseSelection.CourseID = courseID;
                            studentCourseSelection.CourseRequestedDate = _dateTimeProvider.CurrentDate;
                            studentCourseSelection.CourseRequestedTime = _dateTimeProvider.CurrentTime;
                            studentCourseSelection.CrDate = _dateTimeProvider.CurrentDate;
                            db.StudentCourseSelections.Add(studentCourseSelection);
                            db.SaveChanges();


                            TempData["Sucess"] = "<div class=\"alert alert-success alert-dismissible fade in\" role=\"alert\">\r\n  " +
                                "<a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\" style=\"font-size:30px\">&times;</a>" +
                                "<strong>Done! </strong>" + " You have successfully applied. Now, click on the 'Print' button to obtain a printout of the course registration form, sign the form, and submit it to your advisor.</strong>." +
                             "</div>";
                        }
                        else
                        {
                            Course existingCourse = db.Courses.FirstOrDefault(c => c.ID == courseID);
                            if (existingCourse != null)
                            {
                                if (!string.IsNullOrEmpty(addedCourseNames))
                                {
                                    addedCourseNames += ", ";
                                }
                                addedCourseNames += existingCourse.Title;
                            }

                            TempData["Failed"] = "<div class=\"alert alert-danger alert-dismissible fade in\" role=\"alert\">\r\n  " +
                                "<a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\" style=\"font-size:30px\">&times;</a>" +
                                "<strong>Failed ! </strong>" + "The course has already been selected for " + db.Sessions.Find(StudentCurrentSessionID).Session1 + " (" + addedCourseNames + ")</div>";
                        }

                    }
                }
            }
            return RedirectToAction("ApplyCourses");
        }



        [HttpGet]
        public async Task<ActionResult> PrintCourses(string session)
        {
            int StudentID = Convert.ToInt32(Session["StudentID"]);
            var studentCourseSelections = db.StudentCourseSelections.Where(t => t.StudentID == StudentID && t.Session.Session1.Equals(session)).OrderByDescending(t => t.SessionID).ToList();
            return View(studentCourseSelections);
        }


        [HttpPost]
        public ActionResult DropCourse()
        {
            int SessionID = Convert.ToInt32(Request.Form["SessionID"]);
            int CourseID = Convert.ToInt32(Request.Form["CourseID"]);

            if (SessionID != null && CourseID != null)
            {

                int StudentID = Convert.ToInt32(Session["StudentID"]);

                _dateTimeProvider.Update();//get current date and time...

                StudentCourseDropped studentCourseDropped = new StudentCourseDropped();

                if (!db.StudentCourseDroppeds.Any(t => t.StudentID.Equals(StudentID) && t.SessionID.Equals(SessionID) && t.CourseID.Equals(CourseID)))
                {
                    studentCourseDropped.StudentID = StudentID;
                    studentCourseDropped.SessionID = SessionID;
                    studentCourseDropped.CourseID = CourseID;
                    studentCourseDropped.CourseRequestedDate = _dateTimeProvider.CurrentDate;
                    studentCourseDropped.CourseRequestedTime = _dateTimeProvider.CurrentTime;
                    studentCourseDropped.CrDate = _dateTimeProvider.CurrentDate;
                    db.StudentCourseDroppeds.Add(studentCourseDropped);
                    db.SaveChanges();

                    TempData["Sucess"] = "<div class=\"alert alert-success alert-dismissible fade in\" role=\"alert\">\r\n  " +
                        "<a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\" style=\"font-size:30px\">&times;</a>" +
                        "<strong>Done! </strong>" + " You have successfully dropped this course. Please wait for the Head of Department's (HOD) approvals.</strong>." +
                     "</div>";
                }
                else
                {
                    TempData["Failed"] = "<div class=\"alert alert-danger alert-dismissible fade in\" role=\"alert\">\r\n  " +
                        "<a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\" style=\"font-size:30px\">&times;</a>" +
                        "<strong>Failed! </strong>" + " This course has already been dropped..</strong>." +
                     "</div>";
                }
            }
            return RedirectToAction("AllCourses");
        }


        [HttpPost]
        public ActionResult WithDrawCourse()
        {
            int SessionID = Convert.ToInt32(Request.Form["SessionID"]);
            int CourseID = Convert.ToInt32(Request.Form["CourseID"]);

            if (SessionID != null && CourseID != null)
            {

                int StudentID = Convert.ToInt32(Session["StudentID"]);

                _dateTimeProvider.Update();//get current date and time...

                StudentCourseWithDrawn studentCourseWithDrawn = new StudentCourseWithDrawn();

                if (!db.StudentCourseWithDrawns.Any(t => t.StudentID.Equals(StudentID) && t.SessionID.Equals(SessionID) && t.CourseID.Equals(CourseID)))
                {
                    studentCourseWithDrawn.StudentID = StudentID;
                    studentCourseWithDrawn.SessionID = SessionID;
                    studentCourseWithDrawn.CourseID = CourseID;
                    studentCourseWithDrawn.CourseRequestedDate = _dateTimeProvider.CurrentDate;
                    studentCourseWithDrawn.CourseRequestedTime = _dateTimeProvider.CurrentTime;
                    studentCourseWithDrawn.CrDate = _dateTimeProvider.CurrentDate;
                    db.StudentCourseWithDrawns.Add(studentCourseWithDrawn);
                    db.SaveChanges();

                    TempData["Sucess"] = "<div class=\"alert alert-success alert-dismissible fade in\" role=\"alert\">\r\n  " +
                        "<a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\" style=\"font-size:30px\">&times;</a>" +
                        "<strong>Done! </strong>" + " You have successfully withdrawn this course. Please wait for the Head of Department's (HOD) approvals.</strong>." +
                     "</div>";
                }
                else
                {
                    TempData["Failed"] = "<div class=\"alert alert-danger alert-dismissible fade in\" role=\"alert\">\r\n  " +
                        "<a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\" style=\"font-size:30px\">&times;</a>" +
                        "<strong>Failed! </strong>" + " This course has already been withdrawn..</strong>." +
                     "</div>";
                }
            }
            return RedirectToAction("AllCourses");
        }


        /// <summary>
        /// Returns the individual student data of the attendance for specific student, course, section, and session
        /// </summary>
        /// <param name="studentID"></param>
        /// <param name="courseID"></param>
        /// <param name="section"></param>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetCourseAttendance()
        {
            int SessionID = Convert.ToInt32(Request.Form["SessionID"]);
            int CourseID = Convert.ToInt32(Request.Form["CourseID"]);
            string Section = Request.Form["Section"];

            int StudentID = Convert.ToInt32(Session["StudentID"]);
            if (StudentID == null)
            {
                return HttpNotFound();
            }
            //ViewBag.AccessByInstructor = true;

            //if (!string.IsNullOrEmpty(Request.Form["TeacherID"]))
            //{
            //    FormID = Convert.ToInt32(Request.Form["TeacherID"]);
            //    ViewBag.AccessByInstructor = false;
            //    // Now you can safely convert it to an integer if it's not empty
            //}

            ////check if the details are valid...
            //var teacherCourse = db.TeacherCourses.FirstOrDefault(t => t.EmpFormID == FormID && t.SessionID == SessionID && t.CourseID == t.CourseID && t.Section == Section);

            //if (teacherCourse == null)
            //{
            //    // The record doesn't exist or is not valid
            //    return HttpNotFound();
            //}

            //ViewBag.Session = teacherCourse.Session.Session1;
            //ViewBag.SessionID = SessionID;
            //ViewBag.Course = teacherCourse.Course.Title + " (" + teacherCourse.Course.CrHr + "/" + teacherCourse.Course.ContHr + ") ";
            //ViewBag.Section = Section;
            //ViewBag.Instructor = teacherCourse.EmpForm.Title.Title1 + " " + teacherCourse.EmpForm.Name;
            var studentAttendances = db.StudentAttendances.Where(t => t.StudentID == StudentID && t.SessionID == SessionID && t.CourseID == CourseID && t.Section == Section).OrderByDescending(i => i.Date)
                                        .ToList();


            ViewBag.Total = studentAttendances.Count;
            ViewBag.Present = studentAttendances.Count(t => t.IsPresent);
            ViewBag.Absent = studentAttendances.Count(t => t.IsPresent == false);
            ViewBag.Percentage = (double)ViewBag.Present / ViewBag.Total * 100;

            if(studentAttendances.Count <1) {
                return RedirectToAction("NoAttendnaceFound");
            }

            //return View(await studentAttendances);

            return View(studentAttendances);
        }

        public ActionResult NoAttendnaceFound()
        {
            return View();
        }



        [HttpPost]
        public ActionResult GetCourseCard()
        {
            int SessionID = Convert.ToInt32(Request.Form["SessionID"]);
            int CourseID = Convert.ToInt32(Request.Form["CourseID"]);
            string Section = Request.Form["Section"];

            int StudentID = Convert.ToInt32(Session["StudentID"]);
            if (StudentID == null)
            {
                return HttpNotFound();
            }

            var std = db.Students.FirstOrDefault(t => t.ID == StudentID);

            ViewBag.CurrentSemester = Methods.GetStudentCurrentSemesterNumber(StudentID);
            ViewBag.Batch = std.ProgramOffered.BatchTitle;
            ViewBag.Group = std.StudentGroup;
           


            ViewBag.SessionID = SessionID;
            ViewBag.Section = Section;
            var course = db.Courses.Find(CourseID);
            return View(course);
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