using LMS.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LMS.Models;
using System.Data.Entity;

namespace LMS.Areas.StudentAccount.Controllers
{
    [App_Start.SessionAuthentication]
    public class StudentProfileController : Controller
    {
        CMSEntities db = new CMSEntities();
        // GET: StudentAccount/StudentProfile

        [HttpGet]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateProfile()
        {
            int StudentID = Convert.ToInt32(Session["StudentID"]);
            var model = db.Students.Find(StudentID);

            //Gender List
            ViewBag.Gender = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Selected = false, Text = "Male", Value = "Male"},
                new SelectListItem { Selected = false, Text = "Female", Value = "Female"},
            }, "Value", "Text", model.Admission.Gender);


            //Blood Group
            ViewBag.BloodGroup = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Selected = false, Text = "A+", Value = "A+" },
                new SelectListItem { Selected = false, Text = "A-", Value = "A-" },
                new SelectListItem { Selected = false, Text = "B+", Value = "B+" },
                new SelectListItem { Selected = false, Text = "B-", Value = "B-" },
                new SelectListItem { Selected = false, Text = "AB+", Value = "AB+" },
                new SelectListItem { Selected = false, Text = "AB-", Value = "AB-" },
                new SelectListItem { Selected = false, Text = "O+", Value = "O+" },
                new SelectListItem { Selected = false, Text = "O-", Value = "O-" },
            }, "Value", "Text", model.Admission.BloodGroup);

            ViewBag.ReligionID = new SelectList(db.Religions, "ID", "Name", model.Admission.ReligionID);
            ViewBag.NationalityID = new SelectList(db.Nationalities, "ID", "NationalityName", model.Admission.NationalityID);
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ID", "Name", model.Admission.ProvinceID);
            ViewBag.DistrictID = new SelectList(db.Districts, "ID", "Name", model.Admission.DistrictID);
            ViewBag.DomicileID = new SelectList(db.Districts, "ID", "Name", model.Admission.DomicileID);
            ViewBag.SessionID = new SelectList(db.Sessions.Where(t => t.IsActive == true).ToList(), "ID", "Session1", model.Admission.SessionID);
            //Following  #### only one line is addition. 
            ViewBag.DefaultSessionID = db.Sessions.Where(t => t.IsActive == true).Select(t => t.ID).FirstOrDefault();
            ViewBag.TitleID = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Selected = false, Text = "Mr.", Value = "1"},
                new SelectListItem { Selected = false, Text = "Ms.", Value = "2"},
            }, "Value", "Text", model.Admission.TitleID);

            return View(model);
        }

        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.MimeType == mimeType)
                {
                    return codec;
                }
            }
            return null;
        }
        private void saveImageInLowDimensionsAndSize(string path, int acceptable_maxWidth, int acceptable_maxHeight, int acceptable_maxFileSizeKB, HttpPostedFileBase file)
        {
            // Load the original image using System.Drawing.Image
            using (var originalImage = System.Drawing.Image.FromStream(file.InputStream))
            {
                // Calculate the new dimensions while maintaining the aspect ratio
                int newWidth, newHeight;
                if (originalImage.Width > acceptable_maxWidth || originalImage.Height > acceptable_maxHeight)
                {
                    double aspectRatio = (double)originalImage.Width / originalImage.Height;

                    newWidth = originalImage.Width;
                    newHeight = originalImage.Height;

                    if (newWidth > acceptable_maxWidth)
                    {
                        newWidth = acceptable_maxWidth;
                        newHeight = (int)(newWidth / aspectRatio);
                    }

                    if (newHeight > acceptable_maxHeight)
                    {
                        newHeight = acceptable_maxHeight;
                        newWidth = (int)(newHeight * aspectRatio);
                    }
                }
                else
                {
                    // No resizing required
                    newWidth = originalImage.Width;
                    newHeight = originalImage.Height;
                }

                // Create a new bitmap with the resized dimensions
                using (var resizedImage = new Bitmap(newWidth, newHeight))
                {
                    // Resize the image using Graphics object
                    using (var graphics = Graphics.FromImage(resizedImage))
                    {
                        graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        graphics.DrawImage(originalImage, 0, 0, newWidth, newHeight);
                    }

                    // Compress the image by adjusting the JPEG compression quality
                    EncoderParameters encoderParameters = new EncoderParameters(1);
                    encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 60L); // Adjust the quality value as needed (0-100)

                    // Save the resized and compressed image to a MemoryStream
                    using (var compressedStream = new MemoryStream())
                    {
                        resizedImage.Save(compressedStream, GetEncoderInfo("image/jpeg"), encoderParameters);

                        // Check if the compressed image size is within the desired limit
                        if (compressedStream.Length / 1024 <= acceptable_maxFileSizeKB)
                        {
                            System.IO.File.WriteAllBytes(path, compressedStream.ToArray());
                        }
                        else
                        {
                            // The compressed image exceeds the desired size limit
                            // You can handle this case accordingly (e.g., display an error message)
                        }
                    }
                }
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateProfile(Student model, HttpPostedFileBase file, HttpPostedFileBase cnic, HttpPostedFileBase fathercnic)
        {
            int StudentID = Convert.ToInt32(Session["StudentID"]);

            bool IsImageMeetRequirments = true;

            if (file != null)
            {


                var postedFileExtension = Path.GetExtension(file.FileName);
                if (string.Equals(postedFileExtension, ".jpg", StringComparison.OrdinalIgnoreCase)
           || string.Equals(postedFileExtension, ".png", StringComparison.OrdinalIgnoreCase)
           || string.Equals(postedFileExtension, ".jpeg", StringComparison.OrdinalIgnoreCase))
                {

                    byte[] imageBytes;
                    using (var memoryStream1 = new MemoryStream())
                    {
                        file.InputStream.CopyTo(memoryStream1);

                        imageBytes = memoryStream1.ToArray();
                    }



                    // Create HttpClientHandler with SSL certificate validation disabled
                    var handler = new HttpClientHandler()
                    {
                        ServerCertificateCustomValidationCallback = (message, certificate, chain, errors) => true
                    };                   

                    model.Admission.Photo = imageBytes;
                }
                else
                {
                    ModelState.AddModelError("Picture", "Your picture is not in a correct fomate!");
                }
            }

            //father cnic upload...
            if (fathercnic != null)
            {
                var fathercnicExtention = Path.GetExtension(fathercnic.FileName);
                if (string.Equals(fathercnicExtention, ".jpg", StringComparison.OrdinalIgnoreCase)
           || string.Equals(fathercnicExtention, ".png", StringComparison.OrdinalIgnoreCase)
           || string.Equals(fathercnicExtention, ".jpeg", StringComparison.OrdinalIgnoreCase))
                {

                    if (model.Admission.FatherCNICCopy != null)
                    {
                        string exCNIC = Path.Combine(Server.MapPath("~/ApplicantFatherCNIC/"), model.Admission.FatherCNICCopy);
                        if (System.IO.File.Exists(exCNIC))
                        {
                            System.IO.File.Delete(exCNIC);
                        }
                    }

                    var filename = Path.GetFileName(fathercnic.FileName);
                    var path = Path.Combine(Server.MapPath("~/ApplicantFatherCNIC/"), StudentID + "-FatherCNIC" + Path.GetExtension(fathercnic.FileName));
                    saveImageInLowDimensionsAndSize(path, 800, 1000, 1000, fathercnic);
                    model.Admission.FatherCNICCopy = StudentID + "-FatherCNIC" + Path.GetExtension(fathercnic.FileName);
                }
                //Following  #### else
                //else
                //{
                //    ModelState.AddModelError("FatherCNICCopy", "Please upload your Father/Guardian CNIC copy in correct format.");
                //}
            }
            //cnic/formB upload...
            if (cnic != null)
            {
                var cnicExtention = Path.GetExtension(cnic.FileName);
                if (string.Equals(cnicExtention, ".jpg", StringComparison.OrdinalIgnoreCase)
           || string.Equals(cnicExtention, ".png", StringComparison.OrdinalIgnoreCase)
           || string.Equals(cnicExtention, ".jpeg", StringComparison.OrdinalIgnoreCase))
                {

                    if (model.Admission.CNICCopy != null)
                    {
                        string exCNIC = Path.Combine(Server.MapPath("~/ApplicantCNICFormB/"), model.Admission.CNICCopy);
                        if (System.IO.File.Exists(exCNIC))
                        {
                            System.IO.File.Delete(exCNIC);
                        }
                    }

                    var filename = Path.GetFileName(cnic.FileName);
                    var path = Path.Combine(Server.MapPath("~/ApplicantCNICFormB/"), StudentID + "-CNICFORMB" + Path.GetExtension(cnic.FileName));
                    saveImageInLowDimensionsAndSize(path, 700, 800, 500, cnic);
                    model.Admission.CNICCopy = StudentID + "-CNICFORMB" + Path.GetExtension(cnic.FileName);
                }

                //Following  #### else
                //else
                //{
                //    ModelState.AddModelError("CNICCopy", "Please upload your CNIC/Form-B copy in correct format.");
                //}
            }



            if (model.Admission.Photo == null)
            {
                ModelState.AddModelError("Picture", "Please upload your Picture!");
            }
            //Following  #### Two else if
            //else if (model.Admission.FatherCNICCopy == null)
            //{
            //    ModelState.AddModelError("FatherCNICCopy", "Please upload your Father/Guardian CNIC copy.");
            //}
            //else if (model.Admission.CNICCopy == null)
            //{
            //    ModelState.AddModelError("CNICCopy", "Please upload your CNIC/Form-B copy.");
            //}            
            else if (model.Admission.CNICNo.Count() != 13)
            {
                ModelState.AddModelError("CNIC", "Please type valid CNIC No.");
            }
            else if (IsImageMeetRequirments)
            {
                var admission = model.Admission;
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.Entry(admission).State = EntityState.Modified;
                await db.SaveChangesAsync();

                TempData["Message"] = "<div class=\"alert alert-success alert-dismissible fade in\" role=\"alert\">\r\n  " +
                        "<a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\" style=\"font-size:30px\">&times;</a>" +
                        "<strong>Done ! </strong>" +
                    "Your information has been successfully submitted. Thank you." +
                    "</div>";
            }
            else
            {
                TempData["Message"] = "<div class=\"alert alert-danger alert-dismissible fade in\" role=\"alert\">\r\n" +
                    "<a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\" style=\"font-size:30px\">&times;</a>" +
                    "<strong>Sorry! </strong>" +
                    "Your picture does not meet the requirements." +
                    "<a href = \"/Account/Applicant/PhotoRequirements\" title = \"Rules for Photo\" target = \"_blank\" > Click Here! </ a >" +
                    "</div>";
            }
            //Gender List
            ViewBag.Gender = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Selected = false, Text = "Male", Value = "Male"},
                new SelectListItem { Selected = false, Text = "Female", Value = "Female"},
            }, "Value", "Text", model.Admission.Gender);

            //BloodGroup
            ViewBag.BloodGroup = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Selected = false, Text = "A+", Value = "A+"},
                new SelectListItem { Selected = false, Text = "A-", Value = "A-"},
                new SelectListItem { Selected = false, Text = "B+", Value = "B+"},
                new SelectListItem { Selected = false, Text = "B-", Value = "B-"},
                new SelectListItem { Selected = false, Text = "AB+", Value = "AB+"},
                new SelectListItem { Selected = false, Text = "AB-", Value = "AB-"},
                new SelectListItem { Selected = false, Text = "O+", Value = "O+"},
                new SelectListItem { Selected = false, Text = "O-", Value = "O-"},
            }, "Value", "Text", model.Admission.BloodGroup);

            ViewBag.ReligionID = new SelectList(db.Religions, "ID", "Name", model.Admission.ReligionID);
            ViewBag.NationalityID = new SelectList(db.Nationalities, "ID", "NationalityName", model.Admission.NationalityID);
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ID", "Name", model.Admission.ProvinceID);
            ViewBag.DistrictID = new SelectList(db.Districts, "ID", "Name", model.Admission.DistrictID);
            ViewBag.DomicileID = new SelectList(db.Districts, "ID", "Name", model.Admission.DomicileID);
            ViewBag.SessionID = new SelectList(db.Sessions.Where(t => t.IsActive == true).ToList(), "ID", "Session1", model.Admission.SessionID);
            ViewBag.TitleID = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Selected = false, Text = "Mr.", Value = "1"},
                new SelectListItem { Selected = false, Text = "Ms.", Value = "2"},
            }, "Value", "Text", model.Admission.TitleID);

            return View(model);

        }



        [HttpGet]
        public async Task<ActionResult> ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(string OldPassword, string NewPassword, string ConfirmPassword)
        {
            int StudentID = Convert.ToInt32(Session["StudentID"]);
            var student = db.Students.Find(StudentID);

            if (student.Password != OldPassword)
            {
                TempData["OldPassword"] = OldPassword;
                TempData["NewPassword"] = NewPassword;
                TempData["ConfirmPassword"] = ConfirmPassword;
                TempData["Failed"] = "Your old password does not match the current password. Please try again.";
            }
            else if (NewPassword != ConfirmPassword)
            {
                TempData["OldPassword"] = OldPassword;
                TempData["NewPassword"] = NewPassword;
                TempData["ConfirmPassword"] = ConfirmPassword;
                TempData["Failed"] = "Your new password and confirm password do not match. Please try again.";
            }
            else
            {
                student.Password = NewPassword;
                db.SaveChanges();
                TempData["Success"] = "Done, your password has been successfully changed. Thank you!";
            }

            return View();
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