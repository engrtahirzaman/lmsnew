using LMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS.Areas.Courses.Controllers
{
    public class AssessmentController : Controller
    {
        CMSEntities db = new CMSEntities();

        // GET: Courses/Assessment
        [ValidateAntiForgeryToken]
        public ActionResult GetCourseAssessment()
        {
            //Sesssion parameter
            int StudentID = Convert.ToInt32(Session["StudentID"]);

            int SessionID = Convert.ToInt32(Request.Form["SessionID"]);

            int CourseID = Convert.ToInt32(Request.Form["CourseID"]);
            int StudentCourseID = Convert.ToInt32(Request.Form["stdCourseID"]);
            int TeacherCourseID = Convert.ToInt32(Request.Form["TeacherCourseID"]);



            ViewBag.StudentCourseID = StudentCourseID;
            ViewBag.SessionID = SessionID;
            ViewBag.CourseID = CourseID;
            ViewBag.TeacherCourseID = TeacherCourseID;
            var Course = db.Courses.Where(t => t.ID == CourseID).FirstOrDefault();
            ViewBag.CourseCodeTitle = Course.Code + " " + Course.Title;


            // DateTime currentDate = DateTime.Now;

            var assessments = db.Assessments
                .Where(t => t.TeacherCourseID == TeacherCourseID)
                .ToList();
                return View(assessments);
        }



        [ValidateAntiForgeryToken]
        public ActionResult GetCourseAssessmentMarks()
        {
            //Sesssion parameter
            int StudentID = Convert.ToInt32(Session["StudentID"]);

            int SessionID = Convert.ToInt32(Request.Form["SessionID"]);

            int CourseID = Convert.ToInt32(Request.Form["CourseID"]);
            int StudentCourseID = Convert.ToInt32(Request.Form["stdCourseID"]);
            int TeacherCourseID = Convert.ToInt32(Request.Form["TeacherCourseID"]);

            int AssessmentID = Convert.ToInt32(Request.Form["AssessmentID"]);
            var AssessmentInfo = db.Assessments.Find(AssessmentID);
            // DateTime currentDate = DateTime.Now;
            ViewBag.MaxMarks = AssessmentInfo.TotalMarks;
            ViewBag.Description = AssessmentInfo.Description;
            ViewBag.Name = AssessmentInfo.AssessmentName.Name;
            ViewBag.Weightage = AssessmentInfo.Weightage;


            ViewBag.StudentCourseID = StudentCourseID;
            ViewBag.SessionID = SessionID;
            ViewBag.CourseID = CourseID;
            ViewBag.TeacherCourseID = TeacherCourseID;
            var Course = db.Courses.Where(t => t.ID == AssessmentInfo.TeacherCours.CourseID).FirstOrDefault();
            ViewBag.CourseCodeTitle = Course.Code + " " + Course.Title;

            

            var assessmentsDetails = db.AssessmentDetails
                .SingleOrDefault(t => t.AssessmentsID == AssessmentID && t.StudentID == StudentID);
            return View(assessmentsDetails);
        }

        [ValidateAntiForgeryToken]
        public ActionResult GetCourseAssessmentConsolidated()
        {
            //Sesssion parameter
            int StudentID = Convert.ToInt32(Session["StudentID"]);

            int SessionID = Convert.ToInt32(Request.Form["SessionID"]);

            int CourseID = Convert.ToInt32(Request.Form["CourseID"]);
            int StudentCourseID = Convert.ToInt32(Request.Form["stdCourseID"]);
            int TeacherCourseID = Convert.ToInt32(Request.Form["TeacherCourseID"]);

            var assessmentsDefined = db.Assessments.Where(t => t.TeacherCourseID == TeacherCourseID && t.TeacherCours.SessionID == SessionID).OrderBy(m => m.AssessmentName.SortBy).ToList();


            ViewBag.StudentCourseID = StudentCourseID;
            ViewBag.SessionID = SessionID;
            ViewBag.CourseID = CourseID;
            ViewBag.TeacherCourseID = TeacherCourseID;
            var Course = db.Courses.Where(t => t.ID == CourseID).FirstOrDefault();
            ViewBag.CourseCodeTitle = Course.Code + " " + Course.Title;



            return View(assessmentsDefined);
        }
    }
}