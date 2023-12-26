using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CaptchaMvc.HtmlHelpers;
using LMS.Models;
using static System.Collections.Specialized.BitVector32;

namespace LMS.Areas.StudentAccount.Controllers
{
    
    public class AccountController : Controller
    {
        CMSEntities db = new CMSEntities();
        
        // GET: StudentAccount/Account
        [HttpGet]
        public async Task<ActionResult> Login()
        {
            ViewBag.Success = TempData["Success"];
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(UserLoginModel model, string returnUrl)
        {

            if (this.IsCaptchaValid("Captcha is Invalid"))
            {
                //if (response.Success && ModelState.IsValid)
                if (ModelState.IsValid)
                {

                    //checking if record exists,active,not expelled and not blocked...
                    bool IsDefaulter = db.FinanceUnpaidDataForLMControls.Any(t => t.RegNo.ToLower() == model.RegNo.ToLower());

                    //checking if studentfee records are due 
                    DateTime currentDate = DateTime.Now;
                    var hasValidRecord = db.StudentFees.Where(t =>t.Student.RegNo == model.RegNo).ToList();

                    var hasDue = hasValidRecord.Any(t =>t.IsVerified == false && (t.DueDate < currentDate.Date || t.DueDate == null));

                    if(hasValidRecord.Count() == 0)
                    {
                        TempData["Failed"] = "<style>.danger {background-color: #ffdddd; border-left: 6px solid #f44336;}</style><div class=\"alert alert-danger danger alert-dismissible fade in\" role=\"alert\">\r\n  " +
                         "<a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\" style=\"font-size:30px\">&times;</a>" +
                         "<strong>Outstanding Dues! </strong><br>Dear Student,<br> Please note that you have pending dues. To access the Learning Management System (LMS), ensure your dues are cleared. " +
                           "Please visit Finance Section (Management Block, 1st Floor) and clear these dues promptly to avoid academic or administrative issues." +
                           "</div>";
                        return View(model);
                    }
                    else if (IsDefaulter || hasDue)
                    {
                        TempData["Failed"] = "<style>.danger {background-color: #ffdddd; border-left: 6px solid #f44336;}</style><div class=\"alert alert-danger danger alert-dismissible fade in\" role=\"alert\">\r\n  " +
                          "<a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\" style=\"font-size:30px\">&times;</a>" +
                          "<strong>Outstanding Dues! </strong><br>Dear Student,<br> Please note that you have pending dues. To access the Learning Management System (LMS), ensure your dues are cleared. " +
                            "Please visit Finance Section (Management Block, 1st Floor) and clear these dues promptly to avoid academic or administrative issues." +
                            "</div>";
                        return View(model);

                       
                    }
                    else {

                        //checking if record exists,active,not expelled and not blocked...
                        bool IsValidUser = db.Students.Any(t => t.RegNo.ToLower() == model.RegNo.ToLower()
                    && t.Password == model.Password);
                        if (IsValidUser)
                        {
                            Session["StudentID"] = db.Students.Where(t => t.RegNo == model.RegNo && t.Password == model.Password).FirstOrDefault().ID;
                            Session["Username"] = db.Students.Where(t => t.RegNo == model.RegNo && t.Password == model.Password).FirstOrDefault()?.Admission.FirstName;

                            // Check if returnUrl is not null and valid addresss
                            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                            {
                                // Redirect to the specified ReturnUrl (page C)
                                return Redirect(returnUrl);
                            }

                            //return RedirectToAction("Index", "MyDashboard", new { Area = "" });
                            //return RedirectToAction("UpdateProfile", "StudentProfile", new { Area = "StudentAccount" });
                            return RedirectToAction("Index", "MyDashboard", new { Area = "" });
                        }

                    }

                }
                ModelState.AddModelError("InvalidMessage", "The provided credentials are incorrect. Please try again.");
            }
            return View(model);
        }

        public async Task<ActionResult> Logout()
        {
            Session.Abandon(); // it will clear the session at the end of request
            //FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account", new { Area = "StudentAccount" });
        }

        [HttpPost]
        public JsonResult RetrievePassword(string regno)
        {
            var Message = "";
            if (db.Students.Any(t => t.RegNo == regno) == true)
            {
                var model = db.Students.Where(t => t.RegNo == regno).FirstOrDefault();

                //Sending email
                string ToEmail = model.Admission.EmailAddress;
                var Subject = "PAF-IAST Password Retrieve.";
                string BodyMessage = "Dear " + db.Titles.Find(model.Admission.TitleID).Title1 + model.Admission.FirstName + " " + model.Admission.LastName + "<br />" +
                "Your password is: <strong>" + model.Password + " </strong><br />" +
                "Please do not share this password with anyone. <br />" +
                    "Thank You. <br /><br /><br />" +
                    "Kind Regards, <br />" +
                    "PAF-IAST Software Cell Team.";
                BodyMessage += "<br /><br /><br />";
                BodyMessage += "Website: https://paf-iast.edu.pk/ | Email: info@paf-iast.edu.pk | Contact No. 0995-111 723 278 | (111-PAF-AST)";

                if (EmailSender.SendMail(ToEmail, Subject, BodyMessage) != null)
                {
                    Message = "<span class='text-success'>Your password is sent to " +
                    model.Admission.EmailAddress + ". Please do not share your password with anyone. Thank You.</span>";
                    return Json(new { Status = "true", Message = Message });
                }
                else
                {
                    Message = "<span class='text-danger'>Sorry, Something went wrong on email outgoing. Please contact PAF-IAST Software Cell via lms@paf-iast.edu.pk</span>";
                    return Json(new { Status = "true", Message = Message });
                }

            }
            else
            {
                Message = "<span class='text-danger'>We do not have the provided Reg No. " + regno + ". Please try again with a valid registration no. Thank you.</span>";
                return Json(new { Status = "false", Message = Message });
            }

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