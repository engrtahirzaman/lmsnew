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
                            "<strong>Failed ! </strong>" + "The course has already been selected for " + db.Sessions.Find(StudentCurrentSessionID).Session1+ " (" + addedCourseNames + ")</div>";
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