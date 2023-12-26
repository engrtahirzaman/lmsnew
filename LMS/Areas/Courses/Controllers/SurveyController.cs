using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using LMS.Models;
using static System.Collections.Specialized.BitVector32;
using System.Data.Entity;
using System.Text;
using System.Web.Helpers;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Web.UI.WebControls;
using System.Web.Razor.Parser.SyntaxTree;
using static System.Net.Mime.MediaTypeNames;
using System.Globalization;

namespace LMS.Areas.Courses.Controllers
{
    public class SurveyController : Controller
    {
        CMSEntities db = new CMSEntities();
        // GET: Courses/Survey  
        [HttpPost]
        public async Task<ActionResult> CourseSurvey()
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
            ViewBag.CourseCodeTitle = Course.Code +" "+Course.Title;
            bool isLab = false;
            if (Course.ISLab)
            {
                isLab = true;
            }

            var surveyTypeName = isLab ? "LabCourse" : "TheoryCourse";

           // DateTime currentDate = DateTime.Now;

            var surveyInitiateD = db.SurveyInitiateds
                .Where(t => t.SessionID == SessionID &&
                            t.Survey.SurveyType.Name == surveyTypeName)
                .ToList();
            if(surveyInitiateD == null || surveyInitiateD.Count < 1)
                return HttpNotFound();
            else
            return View(surveyInitiateD);
        }

        [HttpPost]
        public async Task<ActionResult> TeacherSurvey()
        {
            //Sesssion parameter
            int StudentID = Convert.ToInt32(Session["StudentID"]);


            int SessionID = Convert.ToInt32(Request.Form["SessionID"]);

            int CourseID = Convert.ToInt32(Request.Form["CourseID"]);
            int StudentCourseID = Convert.ToInt32(Request.Form["stdCourseID"]);
            int TeacherID = Convert.ToInt32(Request.Form["TeacherID"]);
            string TeacherName = Request.Form["TeacherName"];
            int TeacherCourseID = Convert.ToInt32(Request.Form["TeacherCourseID"]);


            ViewBag.StudentCourseID = StudentCourseID;
            ViewBag.SessionID = SessionID;
            ViewBag.CourseID = CourseID;
            ViewBag.TeacherID = TeacherID;
            ViewBag.TeacherName = TeacherName;
            ViewBag.TeacherCourseID = TeacherCourseID;


            var Course = db.Courses.Where(t => t.ID == CourseID).FirstOrDefault();
            ViewBag.CourseCodeTitle = Course.Code + " " + Course.Title;

            bool isLab = false;
            if (Course.ISLab)
            {
                isLab = true;
            }
           
            var surveyTypeName = isLab ? "LabEngineerFeedBack" : "InstructorFeedBack";

            var surveyInitiateD = db.SurveyInitiateds
                .Where(t => t.SessionID == SessionID &&
                            t.Survey.SurveyType.Name == surveyTypeName)
                .ToList();

            if (surveyInitiateD == null || surveyInitiateD.Count < 1)
                return HttpNotFound();
            else
                return View(surveyInitiateD);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveQuestionResponse()
        {

            int StudentID = Convert.ToInt32(Session["StudentID"]);

           
            int surveyInitiatedID = Convert.ToInt32(Request.Form["surveyInitiatedID"]);
            int surveyID = Convert.ToInt32(Request.Form["surveyID"]);
            int SurveyHeadingID = Convert.ToInt32(Request.Form["SurveyHeadingID"]);
            int sortNo = Convert.ToInt32(Request.Form["SortNo"]);
            int QuestionTypeID = Convert.ToInt32(Request.Form["QuestionTypeID"]);
            string Question = Request.Form["Question"];
            int surveyHeadingQAID = Convert.ToInt32(Request.Form["surveyHeadingQAID"]);
            int SessionID = Convert.ToInt32(Request.Form["SessionID"]);
            int CourseID = Convert.ToInt32(Request.Form["CourseID"]);
            int StudentCourseID = Convert.ToInt32(Request.Form["StudentCourseID"]);
            int TeacherCourseID = Convert.ToInt32(Request.Form["TeacherCourseID"]);

            string QuestionResponse = Request.Form["name"];
            if (QuestionResponse == null)
            {
                QuestionResponse = Request.Form["textType"];
            }

            var studentCourseRecord = db.StudentCourses.Where(m => m.StudentID == StudentID && m.CourseID == CourseID && m.SessionID == SessionID).FirstOrDefault();
            // var teachCourseRecord = db.TeacherCourses.Where(m => m.Section == studentCourseRecord.Section && m.CourseID == CourseID && m.SessionID == SessionID).FirstOrDefault();

            var surveyInitiatedRepsonse = db.SurveyInitiatedResponses.Where(t =>t.SessionID == SessionID && t.TeacherCoursesID == TeacherCourseID && t.SurveyInititatedID == surveyInitiatedID && t.StudentCourseID == StudentCourseID).FirstOrDefault();
            int surveyInitiatedRepsonseID = 0;
            if (surveyInitiatedRepsonse == null)
            {
               

                // Insert into surveyinitiatedresponse if first time
                SurveyInitiatedResponse newResponse = new SurveyInitiatedResponse
                {
                    // Populate other properties as needed
                    SurveyInititatedID = surveyInitiatedID,
                    SessionID = SessionID,
                    SurveyID = surveyID,
                    StudentCourseID = StudentCourseID, 
                    CourseID = CourseID,
                    StudentID = StudentID,
                    TeacherCoursesID = TeacherCourseID,
                    Section = studentCourseRecord.Section,
                    CrBy = StudentID,
                    CrDate = DateTime.Now,
                    CrTime = DateTime.Now.TimeOfDay
                };

                // Save the new response to the database
                db.SurveyInitiatedResponses.Add(newResponse);
                db.SaveChanges();

                // Now, update surveyInitiatedRepsonseID with the newly created ID
                surveyInitiatedRepsonseID = newResponse.ID;
            }
            else
                surveyInitiatedRepsonseID = surveyInitiatedRepsonse.ID;


            if (Question.Length > 5)
            {
                //Check if already exist than update otherwise insert

                var questionResponse = db.SurveyHeadingQAResponses
                                         .FirstOrDefault(t => t.SurveyInitiatedResponseID == surveyInitiatedRepsonseID && t.SurveyHeadingQAID == surveyHeadingQAID);

                if (questionResponse != null)
                {

                    questionResponse.Answer = QuestionResponse;
                    // Update other properties
                    questionResponse.UBy = StudentID;
                    questionResponse.UDate = DateTime.Now;
                    questionResponse.UTime = DateTime.Now.TimeOfDay;

                    // Save changes to the database
                    db.SaveChanges();
                }

                else
                {
                    // Insert code if the response does not exist
                    SurveyHeadingQAResponse newResponse = new SurveyHeadingQAResponse
                    {
                        // Populate other properties as needed
                        SurveyInitiatedResponseID = surveyInitiatedRepsonseID,
                        Question = Question,
                        SurveyHeadingQAID = surveyHeadingQAID,
                        QuestionTypeID = QuestionTypeID,
                        Answer = QuestionResponse, // Replace with the actual value or property
                        SurveyHeadingID = SurveyHeadingID,
                        SortNo = sortNo,
                        CrBy = StudentID,
                        CrDate = DateTime.Now,
                        CrTime = DateTime.Now.TimeOfDay
                    };

                    // Save the new response to the database
                    db.SurveyHeadingQAResponses.Add(newResponse);
                    db.SaveChanges();
                }

            }

            return Json(new { Data = "data if you want", Message = "Success" }, JsonRequestBehavior.AllowGet);

        }


         [HttpPost]
         [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitQuestionResponse()
        {

            try
            {

                int StudentID = Convert.ToInt32(Session["StudentID"]);

            if(StudentID == 0 || StudentID == null) {
            
                return HttpNotFound();
            }

            //Decrypt the ID
            string SrvyInitResp = Request.Form["surveyInitiatedRepsonseID"];
            int surveyInitiatedRepsonseID = Convert.ToInt32(EncryptDecrypt.DecryptText(SrvyInitResp));
            int SessionID = Convert.ToInt32(Request.Form["SessionID"]);
            int StudentCourseID = Convert.ToInt32(Request.Form["StudentCourseID"]);

            int surveyInitiatedID = Convert.ToInt32(Request.Form["surveyInitiatedID"]);
            int surveyID = Convert.ToInt32(Request.Form["surveyID"]);
            int CourseID = Convert.ToInt32(Request.Form["CourseID"]);


            var surveyInitiatedRepsonse = db.SurveyInitiatedResponses.Where(t => t.ID == surveyInitiatedRepsonseID && t.StudentCourseID == StudentCourseID && t.SessionID == SessionID && t.StudentID == StudentID).FirstOrDefault();

            if (surveyInitiatedRepsonse != null)
            {

                surveyInitiatedRepsonse.Submitted = true;
                // Update other properties
                surveyInitiatedRepsonse.SubmittedBy = StudentID;
                surveyInitiatedRepsonse.SubmittedDate = DateTime.Now;
                surveyInitiatedRepsonse.SubmittedTime = DateTime.Now.TimeOfDay;

                // Save changes to the database
                db.SaveChanges();
            }

                var result = new { success = true, message = "Form submitted successfully!" };
                return Json(result);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                var result = new { success = false, message = "Error submitting the form." };
                return Json(result);
            }

}



        public ActionResult loadUnSubmittedSurvey(int SurveryInitiatedID, int StudentCourseID, int TeacherCourseID)
        {
            var allinitiatedSurveys = db.SurveyInitiatedResponses
                .Where(t => t.StudentCourseID == StudentCourseID && t.TeacherCoursesID == TeacherCourseID)
                .ToList();
            StringBuilder htmlStringBuilder = new StringBuilder();

            if (allinitiatedSurveys.Count > 0)
            {
                foreach (var surveyInitiatedRepsonse in allinitiatedSurveys)
                {
                    int sectionCounter = 1;
                    int questionCounter = 1;
                    var solvedQuestions = db.SurveyHeadingQAResponses.Where(t => t.SurveyInitiatedResponseID == surveyInitiatedRepsonse.ID).OrderBy(m => m.SurveyHeading.SortNo).ToList();
                    var groupedQuestions = solvedQuestions.GroupBy(q => q.SurveyHeading);

                    if (solvedQuestions.Count > 0)
                    {
                        if (surveyInitiatedRepsonse.Submitted != true)
                        {
                            htmlStringBuilder.AppendLine("<style> .info {\r\n  background-color: #2196F3;\r\n  border-left: 6px solid #2196F3;\r\n}</style><div class=\"alert alert-info info\"><button type = \"button\" class=\"close\" data-dismiss=\"alert\"><i class=\"ace-icon fa fa-times\"></i><button><strong>");
                            htmlStringBuilder.AppendLine("<h3 >Following are your selected responses for  <span class=\"label label-lg label-pink\">" + surveyInitiatedRepsonse.SurveyInitiated.EvaluationCycle.Name + "</span>. When you done with all above questions, then submit by clicking the button below to record (finally submited) your response.</h3>");
                            htmlStringBuilder.AppendLine("</div>");


                            htmlStringBuilder.AppendLine("<form action=\"/Courses/Survey/SubmitQuestionResponse\" method=\"post\" class=\"form-horizontal\" enctype=\"multipart/form-data\" id=\"submitSurveyForm\" >");

                            //Encrypt the ID
                            string SrvyInitResp = EncryptDecrypt.EncryptText(surveyInitiatedRepsonse.ID.ToString());
                            string antiForgeryToken = AntiForgery.GetHtml().ToString();

                            // Hidden fields
                            htmlStringBuilder.AppendLine(antiForgeryToken);
                            htmlStringBuilder.AppendLine("<input type=\"hidden\" name=\"surveyInitiatedRepsonseID\" value=\"" + SrvyInitResp + "\" />");
                            htmlStringBuilder.AppendLine("<input type=\"hidden\" name=\"surveyID\" value=\"" + surveyInitiatedRepsonse.SurveyID + "\" />");
                            htmlStringBuilder.AppendLine("<input type=\"hidden\" name=\"SessionID\" value=\"" + surveyInitiatedRepsonse.SessionID + "\" />");
                            htmlStringBuilder.AppendLine("<input type=\"hidden\" name=\"StudentCourseID\" value=\"" + surveyInitiatedRepsonse.StudentCourseID + "\" />");
                            htmlStringBuilder.AppendLine("<input type=\"hidden\" name=\"CourseID\" value=\"" + surveyInitiatedRepsonse.CourseID + "\" />");
                            htmlStringBuilder.AppendLine("<input type=\"hidden\" name=\"TeacherCourseID\" value=\"" + TeacherCourseID + "\" />");


                            htmlStringBuilder.AppendLine("<div class=\"form-actions center\">");
                            htmlStringBuilder.AppendLine("<button type=\"button\" class=\"btn btn-purple\" onclick=\"submitForm()\">");
                            htmlStringBuilder.AppendLine("Submit the Following Form");
                            htmlStringBuilder.AppendLine("<i class=\"ace-icon fa fa-arrow-right icon-on-right bigger-110\"></i>");
                            htmlStringBuilder.AppendLine("</button>");
                            htmlStringBuilder.AppendLine("<div id =\"spinner\" style = \"display: none;\" > Loading...  </div>");
                            htmlStringBuilder.AppendLine("</div>");
                            htmlStringBuilder.AppendLine("</form>");
                        }
                        else
                        {
                            htmlStringBuilder.AppendLine("<i class=\"ace-icon fa fa-flag bigger-130 orange\"> Submitted On: ");
                            htmlStringBuilder.Append("<span class=\"label label-lg label-danger arrowed arrowed-right\">");
                                            htmlStringBuilder.Append(surveyInitiatedRepsonse.SubmittedDate.Value.ToString("dd /MMMM/yyyy", CultureInfo.InvariantCulture));
                                        if(surveyInitiatedRepsonse.SubmittedTime.HasValue)
                                        {
                                            htmlStringBuilder.Append("<text>");
                                htmlStringBuilder.Append(surveyInitiatedRepsonse.SubmittedTime);

                                htmlStringBuilder.Append("</text>");
                                        }
                            htmlStringBuilder.Append("</span>");
                               htmlStringBuilder.AppendLine("</i><h3 class=\"header  lighter blue\">Following are your selected and submitted responses for <span class=\"label label-lg label-pink\">" + surveyInitiatedRepsonse.SurveyInitiated.EvaluationCycle.Name  + "</span></h3>");
                        }


                        foreach (var group in groupedQuestions)
                        {
                            htmlStringBuilder.AppendLine($"<div class=\"panel panel-info\">");
                            htmlStringBuilder.AppendLine($"<div class=\"panel-heading\">");
                            htmlStringBuilder.AppendLine($"<h3 class=\"panel-title\">Section-{(char)('A' + sectionCounter - 1)}: {group.Key.Name}</h3>");
                            htmlStringBuilder.AppendLine("</div>");
                            htmlStringBuilder.AppendLine("<div class=\"panel-body\">");

                            foreach (var question in group)
                            {
                                htmlStringBuilder.AppendLine($"<div class=\"panel panel-info\" style=\"background-color:aliceblue\">");

                                htmlStringBuilder.AppendLine($"<p>Q-{questionCounter}: {question.Question}</p>");

                                if (question.QuestionTypeID == 1)
                                {
                                    htmlStringBuilder.AppendLine("<div class=\"rounded-box text-center\">");
                                    htmlStringBuilder.AppendLine("<div class=\"radio-group\">");
                                    htmlStringBuilder.AppendLine("<div class=\"radio-item\">");

                                    foreach (var option in question.QuestionType.QuestionTypeAnswerOptions)
                                    {
                                        if (question.Answer == option.OptionName)
                                        {
                                            htmlStringBuilder.AppendLine($"<span class=\"badge badge-success\"><h6><i class=\"ace-icon fa fa-check-square-o\"></i> {option.OptionName}</h6></span> <span>&nbsp;</span>");
                                        }
                                        else
                                        {
                                            htmlStringBuilder.AppendLine($"<span class=\"badge\"> {option.OptionName}</span><span>&nbsp;</span>");
                                        }
                                    }

                                    htmlStringBuilder.AppendLine("</div>");
                                    htmlStringBuilder.AppendLine("</div>");
                                    htmlStringBuilder.AppendLine("</div>");
                                }
                                else if (question.QuestionTypeID == 2)
                                {
                                    htmlStringBuilder.AppendLine($"<div name=\"Q-{questionCounter}\" rows=\"4\" style=\"width: 100%; background-color:white;\">{question.Answer}</div>");
                                }

                                htmlStringBuilder.AppendLine("</div>");
                                htmlStringBuilder.AppendLine("<hr />");
                                questionCounter++;
                            }

                            htmlStringBuilder.AppendLine("</div>");
                            htmlStringBuilder.AppendLine("</div>");
                            sectionCounter++;
                            questionCounter = 1; // Reset question counter for each section
                        }
                    }
                }

                    string finalHtml = htmlStringBuilder.ToString();
                    var jsonResponse = new
                    {
                        success = true,
                        message = "Survey loaded successfully.",
                        html = finalHtml
                    };
                
                    return Json(jsonResponse, JsonRequestBehavior.AllowGet);

                }
           


            var errorResponse = new
            {
                success = false,
                message = "Survey not found or no responses."
            };

            return Json(errorResponse, JsonRequestBehavior.AllowGet);


            ////if (surveyInitiatedRepsonse != null)
            //{
            //    int sectionCounter = 1;
            //    int questionCounter = 1;

            //    var solvedQuestions = db.SurveyHeadingQAResponses
            //        .Where(t => t.SurveyInitiatedResponseID == surveyInitiatedRepsonse.ID)
            //        .ToList();

            //    var groupedQuestions = solvedQuestions.GroupBy(q => q.SurveyHeading);

            //    if (solvedQuestions.Count > 0)
            //    {
            //        StringBuilder sb = new StringBuilder();

            //        // Append your HTML content to the StringBuilder
            //        sb.Append("<div>Your HTML content goes here</div>");

            //        // ... (Continue appending your HTML content using groupedQuestions)

            //        var jsonResponse = new
            //        {
            //            success = true,
            //            message = "Survey loaded successfully.",
            //            html = sb.ToString()
            //        };

            //        return Json(jsonResponse, JsonRequestBehavior.AllowGet);
            //    }
            //}


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