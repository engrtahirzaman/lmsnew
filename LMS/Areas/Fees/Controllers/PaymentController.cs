using LMS.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace LMS.Areas.Fees.Controllers
{
    public class PaymentController : Controller
    {
        CMSEntities db = new CMSEntities();
        DateTimeProvider _dateTimeProvider = DateTimeProvider.Instance;

        //Payment Generation Api......start...
        public class ChallanSubCategory
        {
            public int Amount_Head_ID { get; set; }
            public string Amount_Head_Title { get; set; }
            public decimal Amount_Head_Value { get; set; }
        }

        public class ClinkPaymentResponseModel
        {
            public string Clink_ID { get; set; }
            public string Error_Msg { get; set; }
            public string Message { get; set; }
        }

        public class TokenResponseModel
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public int expires_in { get; set; }
        }

        private async Task<string> GetAccessTokenAsync()
        {


            using (HttpClient client = new HttpClient())
            {
                //string tokenEndpoint = "https://sandboxapi.kaizenhive.com/Token";
                string tokenEndpoint = "https://kaizenhive.com/Sandboxapi/Token";
                //string tokenEndpoint = "https://kaizenhive.com/Live/Token";
                var parameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", "PAFCMS01"),
                //password for sandbox...
                //new KeyValuePair<string, string>("password", "MaKKMTTU_gx7f8oLbyqYhGn+nF/ZxWNxSIOtC9C4JeZ8uxgdYOcY=$$%%#@@@E")
                //password for live
                new KeyValuePair<string, string>("password", "MaKKMTTU_UIiVMTRfOPGGsGJJf??JJLIfUY_CdfLNGRTVFDghhjuiopp??8****E") 
            };

                HttpResponseMessage response = await client.PostAsync(tokenEndpoint, new FormUrlEncodedContent(parameters));
                if (response.IsSuccessStatusCode)
                {
                    string jsonContent = await response.Content.ReadAsStringAsync();
                    var tokenResponse = JsonConvert.DeserializeObject<TokenResponseModel>(jsonContent);
                    return tokenResponse.access_token;
                }

                // Handle error if needed
                return null;
            }
        }

        private async Task<ClinkPaymentResponseModel> GenerateClinkPaymentAsync(string accessToken, string clientID, string challanID, 
            decimal totalAmount, string challanCategory, string dueDate, 
            string applicantCNIC, string applicantName, string applicantFatherName, string applicantCellNo, 
            string applicantEmail, string applicantRoomNumber, List<ChallanSubCategory> challanSubCategories)
        {
            using (HttpClient client = new HttpClient())
            {
                //string clinkEndpoint = "http://sandboxapi.kaizenhive.com/api/CLINK/CLIENT/PAFCMS/Generate_ClinkID";
                string clinkEndpoint = "https://kaizenhive.com/Sandboxapi/api/CLINK/CLIENT/PAFCMS/Generate_ClinkID";
                //string clinkEndpoint = "https://kaizenhive.com/Live/api/CLINK/CLIENT/PAFCMS/Generate_ClinkID";

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Prepare the request data
                var requestData = new
                {
                    //access_token = accessToken,
                    Client_ID = clientID,
                    Challan_ID = challanID,
                    Total_Amount = totalAmount,
                    Master_Category = "102",                    
                    Challan_Category = challanCategory,
                    Due_Date = dueDate,
                    Consumer_ID = 1,
                    Applicant_CNIC = applicantCNIC,
                    Applicant_Name = applicantName,
                    Applicant_Father_Name = applicantFatherName,
                    Applicant_CellNo = applicantCellNo,
                    Applicant_Email = applicantEmail,
                    Applicant_RoomNumber = applicantRoomNumber,
                    Challan_Amount_Heads = challanSubCategories
                };

                // Serialize the request data to JSON
                string jsonData = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(clinkEndpoint, content);
                if (response.IsSuccessStatusCode)
                {
                    string jsonContent = await response.Content.ReadAsStringAsync();
                    var clinkResponse = JsonConvert.DeserializeObject<ClinkPaymentResponseModel>(jsonContent);
                    return clinkResponse;
                }

                // Handle error if needed
                return null;
            }
        }

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<System.Web.Mvc.ActionResult> PayAction()
        {
            ApiException apiException = new ApiException();

            //to avoice SSL
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

            var VoucherNo = Request.Form["VoucherNo"];
            var studentFees = db.StudentFees.Where(t => t.VoucherNo == VoucherNo).ToList();


            var checkForAlreadRerferenceID = db.StudentFees.Where(t => t.VoucherNo == VoucherNo && t.OnlinePaymentReferenceID != null).Select(t => t.OnlinePaymentReferenceID)?.FirstOrDefault();
            if (checkForAlreadRerferenceID != null)
            {
                TempData["Success"] = "<style>.success {border-left: 6px solid #04AA6D;}</style><div class=\"alert alert-success success alert-dismissible fade in\" role=\"alert\">\r\n  " +
                                "<a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\" style=\"font-size:30px\">&times;</a>" +
                                "<strong>Reference/Consumer/CLINK/1Bill ID! </strong>" +
                                "Your Online Reference ID: <strong>100806" + checkForAlreadRerferenceID.ToString() + "</strong> against Voucher No. " + VoucherNo + " has been generated successfully." +
                                "</div>";
                return RedirectToAction("AllFee", "MyFees");
            }

            string accessToken = await GetAccessTokenAsync();

            if (accessToken != null)
            {
                string ApplicantName = studentFees.FirstOrDefault().Student.Admission.FirstName.ToString(); //+ ApplicantData.FirstOrDefault().MiddleName.ToString() + ApplicantData.FirstOrDefault().LastName.ToString();
                // Prepare the request parameters
                string clientID = "1";
                string challanID = "10001";
                string dueDate = studentFees.FirstOrDefault()?.DueDate.Value.ToString("yyyy-MM-ddd");
                decimal totalAmount = studentFees.Sum(t => t.Amount);
                string challanCategory = "10101";
                string applicantCNIC = studentFees.FirstOrDefault().Student.Admission.CNICNo.ToString();
                string applicantName = ApplicantName;
                string Applicant_Father_Name = studentFees.FirstOrDefault().Student.Admission.FatherName.ToString();
                string Applicant_CellNo = studentFees.FirstOrDefault().Student.Admission.MobileNo.ToString();
                string Applicant_Email = studentFees.FirstOrDefault().Student.Admission.EmailAddress.ToString();
                string Applicant_RoomNumber = "";
                // Add other parameters as needed...

                List<ChallanSubCategory> challanSubCategories = new List<ChallanSubCategory>();
                foreach (var item in studentFees)
                {
                    challanSubCategories.Add(new ChallanSubCategory { Amount_Head_ID = Convert.ToInt32(item.FeeHeadID), Amount_Head_Title = item.FeeHead.HeadName, Amount_Head_Value = item.Amount });
                }


                // Call the Clink Payment API and get the response
                ClinkPaymentResponseModel clinkResponse = await GenerateClinkPaymentAsync(accessToken, clientID, challanID, totalAmount, challanCategory, dueDate, applicantCNIC, applicantName, Applicant_Father_Name, Applicant_CellNo, Applicant_Email, Applicant_RoomNumber, challanSubCategories);

                if (clinkResponse != null)
                {
                    if (clinkResponse.Error_Msg != "")
                    {
                        TempData["Failed"] = "<style>.danger {border-left: 6px solid #f44336;}</style><div class=\"alert alert-danger danger alert-dismissible fade in\" role=\"alert\">\r\n  " +
                           "<a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\" style=\"font-size:30px\">&times;</a>" +
                           "<strong>Failed! </strong>" +
                           "Something went wrong with system, Please contact to PAFIAST Technical Team. Error_Details: " + clinkResponse.Error_Msg +
                           "</div>";

                        //api exception saving...
                        apiException.api = "ClinkID Token Generation API";
                        apiException.errorDetails = "RegNo is " + studentFees.FirstOrDefault().Student.RegNo + " and Voucher No." + VoucherNo + " " + clinkResponse.Error_Msg;
                        apiException.date = DateTime.Now;
                        db.ApiExceptions.Add(apiException);
                        db.SaveChanges();
                        return RedirectToAction("AllFee", "MyFees");
                    }
                    else
                    {
                        var Vouhcers = db.StudentFees.Where(t => t.VoucherNo == VoucherNo).ToList();
                        // Process the Clink Payment API response here
                        // For example, you can store Clink_ID or display error messages
                        foreach (var item in Vouhcers)
                        {
                            item.OnlinePaymentReferenceID = clinkResponse.Clink_ID;
                            item.BankVoucherNo = clinkResponse.Clink_ID;
                            item.BranchCode = "1-Bill";
                            item.BankName = "1-Bill";
                            item.BankBranch = "1-Bill";
                            item.ReferenceIDCrDate = DateTime.Now;
                            db.SaveChanges();
                        }

                        TempData["Success"] = "<style>.success {border-left: 6px solid #04AA6D;}</style><div class=\"alert alert-success success alert-dismissible fade in\" role=\"alert\">\r\n  " +
                                "<a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\" style=\"font-size:30px\">&times;</a>" +
                                "<strong>Excellent! </strong>" +
                                "Your Online 1-Bill ID: <strong>100806" + clinkResponse.Clink_ID + "</strong> against Voucher No. " + VoucherNo + " has been generated successfully." +
                                "</div>";

                        //api exception saving...
                        apiException.api = "ClinkID Token Generation API";
                        apiException.errorDetails = "RegNo is " + studentFees.FirstOrDefault().Student.RegNo + " and Voucher No." + VoucherNo + " ClinkID Generated Successfully";
                        apiException.date = DateTime.Now;
                        db.ApiExceptions.Add(apiException);
                        db.SaveChanges();
                        return RedirectToAction("AllFee", "MyFees");
                    }
                }
                else
                {
                    // Handle API error
                }
            }
            else
            {
                // Handle token generation error
                TempData["Failed"] = "<style>.danger {border-left: 6px solid #f44336;}</style><div class=\"alert alert-danger danger alert-dismissible fade in\" role=\"alert\">\r\n  " +
                          "<a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\" style=\"font-size:30px\">&times;</a>" +
                          "<strong>Failed! </strong>" +
                          "Something went wrong with system, Please contact to PAFIAST Technical Team. Error_Details: Token Not Generated. " +
                          "</div>";

                //api exception saving...
                apiException.api = "ClinkID Token Generation API";
                apiException.errorDetails = "RegNo is " + studentFees.FirstOrDefault().Student.RegNo + " and Voucher No." + VoucherNo + " Token Not Generated";
                apiException.date = DateTime.Now;
                db.ApiExceptions.Add(apiException);
                db.SaveChanges();
                return RedirectToAction("AllFee", "MyFees");

            }
            return Json(JsonRequestBehavior.AllowGet);
        }
        //Payment Generation Api......end...



        //Payment Receiving Api......starts..
        public async Task<System.Web.Mvc.ActionResult> checkApi()
        {

            using (HttpClient client = new HttpClient())
            {
                string token = Hash_CalculateHash("100124010316200001", "tahir");
                string tokenEndpoint = "https://localhost:44350/Fees/Payment/PayApi";
                var parameters = new List<KeyValuePair<string, string>>
            {

                new KeyValuePair<string, string>("Username", "tahir"),
                new KeyValuePair<string, string>("Password", "tahir"),
                new KeyValuePair<string, string>("Challan_ID", "107"),
                new KeyValuePair<string, string>("CLINK_ID", "100124010316200001"),
                new KeyValuePair<string, string>("Status", "v1"),
                new KeyValuePair<string, string>("Token", token),
                new KeyValuePair<string, string>("Transferred_Datetime", DateTime.Today.ToString()),
                new KeyValuePair<string, string>("Challan_Category", "108"),
            };

                HttpResponseMessage response = await client.PostAsync(tokenEndpoint, new FormUrlEncodedContent(parameters));
                if (response.IsSuccessStatusCode)
                {
                    string jsonContent = await response.Content.ReadAsStringAsync();
                    var tokenResponse = JsonConvert.DeserializeObject<PaymentRequest>(jsonContent);
                    //return tokenResponse.;
                }

                // Handle error if needed
                return null;
            }
        }
        public class PaymentRequest
        {
            public string Username { get; set; }
            //public string Password { get; set; }
            //public string Challan_ID { get; set; }
            public string Challan_Category { get; set; }
            public string CLINK_ID { get; set; }
            public string Transferred_Datetime { get; set; }
            public string Status { get; set; }
            public string Token { get; set; }
        }

        public class PaymentResponse
        {
            public string AckStatus { get; set; }
            public string ErrorDetail { get; set; }
        }
        public bool TokenCheckMatch(string hash, string clinkId, string privateKey)
        {
            try
            {
                var parts = hash.Split(':');

                var salt = Convert.FromBase64String(parts[0]);

                var bytes = KeyDerivation.Pbkdf2(clinkId + privateKey, salt, KeyDerivationPrf.HMACSHA512, 10000, 16);

                return parts[1].Equals(Convert.ToBase64String(bytes));
            }
            catch
            {
                return false;
            }
        }

        public string Hash_CalculateHash(string CLINK_ID, string private_Key)
        {
            var salt = Hash_GenerateSalt(16); //we are using a salt of 16 bytes or 164 bits

            var bytes = KeyDerivation.Pbkdf2(CLINK_ID + private_Key, salt, KeyDerivationPrf.HMACSHA512, 10000, 16);

            return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(bytes)}";
        }
        private static byte[] Hash_GenerateSalt(int length)
        {
            var salt = new byte[length];

            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(salt);
            }

            return salt;
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult PayApi([FromBody] PaymentRequest request)
        {
            //to avoice SSL
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

            // for return JSON
            string AckStatus = "";
            string ErrorDetail = "";

            // Access the attributes using dynamic object properties
            string username = request.Username;
            //string password = request.Password;
            string challanCategory = request.Challan_Category;
            string clinkId = request.CLINK_ID;
            string transferredDatetime = request.Transferred_Datetime;
            string status = request.Status;
            string token = request.Token;

            var vendor = db.OnlinePaymentVenders.FirstOrDefault(m => m.UserName == username);

            //check if record is found against userName -- if record exists
            if (vendor != null)
            {
                //Check if token is valid
                if (token.Contains(":") && token.Contains("="))
                {
                    // Check if Token is valid
                    if (TokenCheckMatch(token, clinkId, vendor.Password))
                    {
                        if (status == "v1")
                        {
                            switch (challanCategory)
                            {
                                case "108":
                                    // Update the student fee record and set isVerified to true
                                    var studentFees = db.StudentFees.Where(a => a.OnlinePaymentReferenceID == clinkId).ToList();

                                    if (studentFees.Any())
                                    {
                                        using (var transaction = db.Database.BeginTransaction())
                                        {
                                            try
                                            {
                                                foreach (var item in studentFees)
                                                {
                                                    item.IsPaid = true;
                                                    item.PaymentDate = Convert.ToDateTime(transferredDatetime);
                                                    item.PaymentTime = _dateTimeProvider.CurrentTime;

                                                    item.IsVerified = true;
                                                    item.VerifiedDate = _dateTimeProvider.CurrentDate;
                                                    item.VerifiedTime = _dateTimeProvider.CurrentTime;
                                                    item.VerifiedBy = vendor.ID;

                                                    db.Entry(item).State = EntityState.Modified;
                                                }

                                                db.SaveChanges();
                                                transaction.Commit();

                                                AckStatus = "successful";
                                                ErrorDetail = "";
                                            }
                                            catch (Exception)
                                            {
                                                transaction.Rollback();
                                                AckStatus = "";
                                                ErrorDetail = "01 – PAF-IAST-CMS database error";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        AckStatus = "";
                                        ErrorDetail = "09 – CLINK ID doesnot exist.";
                                    }
                                    break;
                                // case "108": 
                                //will implement the isVerified for other Categories
                                //     break;
                                default:
                                    AckStatus = "";
                                    ErrorDetail = "03 - Category not provided or invalid.";
                                    break;
                            }
                        }
                        else
                        {
                            AckStatus = "";
                            ErrorDetail = "00 – Verification is not endorsed";
                        }
                    }
                    else
                    {
                        AckStatus = "";
                        ErrorDetail = "06 – token and password/psid do not match";
                    }
                }
                else
                {
                    AckStatus = "";
                    ErrorDetail = "08 – Token is invalid";
                }
            }
            else
            {
                AckStatus = "";
                ErrorDetail = "07 – UserName is invalid";
            }

            var response = new PaymentResponse
            {
                AckStatus = AckStatus,
                ErrorDetail = ErrorDetail
            };

            // Serialize the response object to JSON
            var jsonResponse = JsonConvert.SerializeObject(response);

            // Output the JSON data to the console
            Debug.WriteLine(jsonResponse);

            ApiException apiException = new ApiException();
            //api exception saving...
            apiException.api = "PAFIAST Pay Api";
            apiException.errorDetails = "CLINKID " + clinkId + ", challanCategory: " + challanCategory + ", AckStatus: " + AckStatus + ", Error: " + ErrorDetail;
            apiException.date = DateTime.Now;
            db.ApiExceptions.Add(apiException);
            db.SaveChanges();
            return Json(new { AckStatus = response.AckStatus, ErrorDetail = response.ErrorDetail });
        }

        //Payment Receiving Api......end
    }
}