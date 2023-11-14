using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LMS.Controllers
{
    public class GetFilesController : Controller
    {
        // GET: GetFiles
        [HttpGet]
        public ActionResult GetFeeVoucher(string encryptedImageName)
        {
            
            //string imageName = (string)EncryptDecrypt.DecryptText(encryptedImageName);

            string folderPath = Server.MapPath("~/FeeVouchers/"); 

            string imagePath = Path.Combine(folderPath, encryptedImageName);

            if (System.IO.File.Exists(imagePath))
            {
                byte[] imageData = System.IO.File.ReadAllBytes(imagePath);

                if (imageData.Length > 0)
                {
                    string contentType = "image/jpeg";

                    return File(imageData, contentType);
                }
            }

            return HttpNotFound();
        }

    }
}