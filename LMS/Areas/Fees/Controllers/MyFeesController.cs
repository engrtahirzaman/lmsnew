using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LMS.Models;
using SelectPdf;

namespace LMS.Areas.Fees.Controllers
{
    [App_Start.SessionAuthentication]
    public class MyFeesController : Controller
    {
        CMSEntities db = new CMSEntities();
        DateTimeProvider _dateTimeProvider = DateTimeProvider.Instance;

        // GET: Fees/MyFees
        public async Task<ActionResult> AllFee()
        {
            int StudentID = Convert.ToInt32(Session["StudentID"]);
            var studentFees = db.StudentFees.Where(t => t.StudentID == StudentID).OrderByDescending(t => t.SessionID).ToListAsync();
            return View(await studentFees);
        }

        public string ConvertAmountToWords(decimal amount)
        {
            string[] units = { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten",
                           "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };

            string[] tens = { "", "", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

            if (amount < 20)
            {
                return units[(int)amount];
            }
            else if (amount < 100)
            {
                return tens[(int)(amount / 10)] + (amount % 10 > 0 ? " " + units[(int)(amount % 10)] : "");
            }
            else if (amount < 1000)
            {
                return units[(int)(amount / 100)] + " Hundred" + (amount % 100 > 0 ? " and " + ConvertAmountToWords(amount % 100) : "");
            }
            else if (amount < 1000000)
            {
                return ConvertAmountToWords(amount / 1000) + " Thousand" + (amount % 1000 > 0 ? " " + ConvertAmountToWords(amount % 1000) : "");
            }
            else
            {
                return "Amount is too large to convert.";
            }
        }

        public static string GetOrdinal(int number)
        {
            if (number % 100 >= 11 && number % 100 <= 13)
            {
                return number + "th";
            }

            switch (number % 10)
            {
                case 1:
                    return number + "st";
                case 2:
                    return number + "nd";
                case 3:
                    return number + "rd";
                default:
                    return number + "th";
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult PrintVoucher(int SessionID, string VoucherNo)
        {
            int StudentID = Convert.ToInt32(Session["StudentID"]);
            string CurrentSemesterNo = GetOrdinal(Methods.GetStudentCurrentSemesterNumber(StudentID));

            var student = db.Students.Find(StudentID);

            var BatchTitle = student.ProgramOffered.BatchTitle;



            //FOR FORIEGNERS FEE WILL MULTIPLY WITH 2
            int multiply = 1;
            if (student.Admission.NationalityID > 1)
            {
                multiply = 2;
            }
            string receiverName = student.Admission.FirstName + " " + student.Admission.MiddleName + " " + student.Admission.LastName;

            var studentFee = db.StudentFees.Where(t => t.VoucherNo == VoucherNo && t.IsPaid == false).ToList();

            var Total = (studentFee.Sum(t => t.Amount) * multiply);
            var TotalinWords = ConvertAmountToWords(Convert.ToDecimal(Total));


            var DueDate = "07-Nov-2023";//due date changed by fida sb....
            var IssueDate = "20-Oct-2023";

            // Create a PDF converter object
            HtmlToPdf converter = new HtmlToPdf();

            // Set the page orientation to landscape
            converter.Options.PdfPageOrientation = PdfPageOrientation.Landscape;

            // Load the template from the editor
            string template = @"
          
<html>
    <head>
        <title>Papers</title>
        <style>
            * {
                box-sizing: border-box;
            }
    
            html, body {
                width: 100%;
                height: 100%;
            }
    
            body {
                margin: 0;
                padding: 0;
                background-color: #ffffff;
            }

            .Bank
            {
              text-align: left;
            }
            
       
            .fee_in_word
            {
              
             
              border-bottom:  1px solid black;
              text-align: left;
            }

            .note
            {
              justify-content: left;
              align-items: left;
              text-align: left;
            }

            .fee_in_words
            {
              font-size: 12px;
              
            }
    
            .papers {
                display: flex;
                flex-direction: row;
                align-items: center;
                justify-content: center;
            }
    
            .fee-details {
                width: 25%;
                height: 300px;
            }
            #table2 {
                   border-collapse: collapse;
                     width: 100%;
                       border: 1px solid black; /* Add a border to the table */
                      
              }

            #table2 th, #table2 td {
                   border: 1px solid black; /* Add borders to table cells */
                    text-align: left;
                    font-size: 12px;
                      }
           
            .personal_details{
              display: flex;
              flex-direction: column;
              
            }
            .table1
            {
              font-size: small;
             
            }
            

    
            .paper {
                width: 25%;
                height: 700px;
                background-color: #ffffff;
                margin: 10px;
               
                text-align: center;
                border: black 1px solid;
            }
    
            .addmission-fee {
                font-size: 10px;
                text-decoration: underline; /* Add underline to the text */
                background-color: grey; /* Set the background color to gray */
                height: 22px;
               
            }

            .bank_header
            {
                font-size: 10px;
                background-color: lightgrey; /* Set the background color to gray */
                height: 14px;
            }

            .paper h5 {
    font-size: 10px;
    font-weight: normal; /* Reset font weight to normal */
    margin: 0; /* Remove any default margin for h5 */
            }
            .paper h4 {
              font-size: 12px;
               margin: 0; 
            }
.paper h6{
font-size: 10px;
}
            .paper h1{
              font-size: 14px;
            }

    
            .paper p {
                font-size: 12px;
            }
            .paper h2{
              font-size: 14px;
            }

    
            .headerDiv {
                display: flex;
                align-items: center;
                height: 38px;
            }
            .semester
            {
              display: flex;
              align-items: center;  
              justify-content: space-between;
              border: black 1px solid;
              height: 28px;
             
             
            }
            .footer
            {
              display: flex;
              flex-direction: row;
              justify-content: space-between;
              margin: 0;
            }
            .footer_names
            {
              padding: 0;
              margin-top: 15px;
             border-top: black solid 1px;
             font-size: 14px;
             
            }
            #sp2 {
            background-color: lightgray; /* Set the background color of sp2 to gray */
            padding: 5px 10px; /* Add padding to create some spacing around the text */
        }

    
            .logo {
                margin-right: 20px;
              
            }
    
            .logo img {
                max-width: 60px;
            }
            .due_date
            {
            
             display: flex;
              align-items: center;
              
             margin: 5px;
             justify-content: space-between;
            }
            .date
            {
              display: flex;
              align-items: center;
              justify-content: space-between;
              border: black 1px solid;
              height: 28px;
              border-top: 0cm;
              border-left: 0cm;
              border-right: 0cm;
              display: flex;
            }
            .duedate {
                      font-size: 18px; /* Adjust the font size */
                      font-weight: bolder;
                      font-family: Arial, Helvetica, sans-serif;
             }
.voucherno {
                      font-size: 12px; /* Adjust the font size */
                      font-weight: bolder;
                      font-family: Arial, Helvetica, sans-serif;
             }

        </style>
    </head>";

            template += @"
<body>
 
    <div class=""papers"">
        <div class=""paper"">
          <div class =""headerDiv"">
            <div class = ""logo"">
              <img src=""D:\PakAustria\CMS\CMS/pfiast.png"" style='max-width: 3em;'>
            </div>
            <h6>
              Pak-Austria Fachhochschule: <br> Institute of Applied Sciences and Technology,
               Khanpur Road, Mang Haripur, Pakistan
            </h6> 

           </div>

            <div class =""addmission-fee"">
                <h1>" + CurrentSemesterNo + @" SEMESTER FEE </h1>
            </div>

          <div class = ""semester"">
             <p id = ""sp1"">Session:" + db.Sessions.Find(SessionID).Session1 + @"</p>
            <p id = ""sp2"">Bank Copy</p>

          </div>

          <div class=""date"">
            <p>Bank Branch: _____________________________</p>
          </div>

          <div class=""date"">
            <p>Date: _____________________________</p>
          </div>
          <div class = ""due_date"">
            <span class=""voucherno"">Voucher No." + VoucherNo + @"</span>

          </div>
 <hr>
<div class = ""due_date"">
            <span class=""voucherno"">Issue:" + IssueDate + @"</span>
            <span class=""voucherno"">Due : " + DueDate + @"</span>

          </div>
          <hr>
          <div class = ""personal_details"">
            <table class = ""table1"">
            
              <tr>
                <td>Reg No:</td>
                <td>" + student.RegNo + @"</td>
              </tr>
              <tr>
                <td>Name:</td>
                <td>" + receiverName + @"</td>
              </tr>
              <tr>
                <td>  Mobile No:</td>
                <td>" + student.Admission.MobileNo + @"</td>
              </tr>
              <tr>
                <td>  Program:</td>
                <td>" + student.ProgramOffered.DepartmentProgram.Program.ProgramTitle + @"</td>
              </tr>
              
            </table>

          </div>

           
          <div id=""fee-details"">
                <table id = ""table2"">
                  <tr>
                    <th>Particulars</th>
                    <th>Amount (PKR)</th>
                  </tr>";

            decimal tuitionAmount = 0;
            foreach (var item in studentFee)
            {
                string formattedFee = string.Format("{0:#,0}/-", item.Amount * multiply);
                if (item.FeeHead.FeeType == "Tuition Charges")
                {
                    // Add the tuition fee to the accumulated amount
                    tuitionAmount += item.Amount;
                }
                else
                {
                    // Display other fee items
                    template += " <tr>" +
                        "<td>" + item.FeeHead.HeadName + "</td>" +
                        "<td>" + formattedFee + "</th>" +
                        "</tr>";
                }
            }
            // After iterating, check if there was any tuition fee to display
            if (tuitionAmount > 0)
            {
                template += " <tr>" +
                    "<td>Tuition Fee</td>" +
                    "<td>" + string.Format("{0:#,0}/-", tuitionAmount * multiply) + "</th>" +
                    "</tr>";
            }

            template += @"
                    
                  <tr>
<td><strong>Total:</strong</td>
<td><strong>" + string.Format("{0:#,0}/-", Total) + @"</strong></td>
</tr>
                </table>
        
            </div>

          <div class= ""fee_in_word""> 
            <h5 class= ""fee_in_words"">" + @TotalinWords + @" only.</h5>
          </div>

          <div class = ""note"">
          <h5><strong>Note:</strong></h5>
          <h5>The student must pay the bill till due date.
The deposited copy shall be uploaded to the admission portal. In case of a problem, please email it to the admissions@paf-iast.edu.pk</h5>
        
          </div>

          <div class =""bank_header"">
            <h3>BANK OF KHYBER</h3>
           </div>
          <div class=""Bank"">
            <h4>
             A/C Title: Student Admission Fee <br />
              A/C No: 0014-2007525659 <br />
              IBAN No: PK49KHYB00142007525659
            </h4>
          </div>

          <div class =""bank_header"">
            <h3>NATIONAL BANK OF PAKISTAN</h3>
           </div>

          <div class=""Bank"">
            <h4>
              A/C Title: Student Fee Account <br />
              A/C No: 2330-3168083826 <br />
              IBAN No: PK74NBPA2330003168083826
            </h4>

<div class =""bank_header"">
            <h3>Note: No IBT charges will be apply.</h3>
           </div>
          </div>
         
          <div
          class=""footer""
          >
            <div
            class=""footer_names""
            >
              Student Signature
            </div>
            <div class=""footer_names"">
              Cashier
            </div>
            <div  class=""footer_names"">
              Bank Officer
            </div>
          </div>

        </div>";

            template += @"
        <div class=""paper"">
          <div class =""headerDiv"">
               <div class = ""logo"">
              <img src=""D:\PakAustria\CMS\CMS/pfiast.png"" style='max-width: 3em;'>
            </div>
            <h6>
              Pak-Austria Fachhochschule: <br> Institute of Applied Sciences and Technology,
               Khanpur Road, Mang Haripur, Pakistan
            </h6> 

           </div>

            <div class =""addmission-fee"">
                 <h1>" + CurrentSemesterNo + @" SEMESTER FEE </h1>
            </div>

          <div class = ""semester"">
             <p id = ""sp1"">Session:" + db.Sessions.Find(SessionID).Session1 + @"</p>
            <p id = ""sp2"">Student Copy</p>

          </div>

          <div class=""date"">
            <p>Bank Branch: _____________________________</p>
          </div>

          <div class=""date"">
            <p>Date: _____________________________</p>
          </div>
<div class = ""due_date"">
            <span class=""voucherno"">Voucher No." + VoucherNo + @"</span>

          </div>
<hr>
<div class = ""due_date"">
            <span class=""voucherno"">Issue:" + IssueDate + @"</span>
            <span class=""voucherno"">Due : " + DueDate + @"</span>

          </div>
          <hr>
          <div class = ""personal_details"">
            <table class = ""table1"">
              <tr>
                <td>Reg No:</td>
                <td>" + student.RegNo + @"</td>
              </tr>
              <tr>
                <td>Name:</td>
                <td>" + receiverName + @"</td>
              </tr>
              <tr>
                <td>  Mobile No:</td>
                <td>" + student.Admission.MobileNo + @"</td>
              </tr>
<tr>
                <td>  Program:</td>
                <td>" + student.ProgramOffered.DepartmentProgram.Program.ProgramTitle + @"</td>
              </tr>
              
            </table>

          </div>

          <div id=""fee-details"">
                <table id = ""table2"">
                  <tr>
                    <th>Particulars</th>
                    <th>Amount (PKR)</th>
                  </tr>";

            decimal tuitionAmount2 = 0;
            foreach (var item in studentFee)
            {
                string formattedFee = string.Format("{0:#,0}/-", item.Amount * multiply);
                if (item.FeeHead.FeeType == "Tuition Charges")
                {
                    // Add the tuition fee to the accumulated amount
                    tuitionAmount2 += item.Amount;
                }
                else
                {
                    // Display other fee items
                    template += " <tr>" +
                        "<td>" + item.FeeHead.HeadName + "</td>" +
                        "<td>" + formattedFee + "</th>" +
                        "</tr>";
                }
            }
            // After iterating, check if there was any tuition fee to display
            if (tuitionAmount2 > 0)
            {
                template += " <tr>" +
                    "<td>Tuition Fee</td>" +
                    "<td>" + string.Format("{0:#,0}/-", tuitionAmount2 * multiply) + "</th>" +
                    "</tr>";
            }

            template += @"
                    
                  <tr>
<td><strong>Total:</strong</td>
<td><strong>" + string.Format("{0:#,0}/-", Total) + @"</strong></td>
</tr>
                </table>
        
            </div>

          <div class= ""fee_in_word""> 
             <h5 class= ""fee_in_words"">" + @TotalinWords + @" only.</h5>
          </div>

          <div class = ""note"">
          <h5><strong>Note:</strong></h5>
          <h5>The student must pay the bill till due date.
The deposited copy shall be uploaded to the admission portal. In case of a problem, please email it to the admissions@paf-iast.edu.pk</h5>
       
          </div>

          <div class =""bank_header"">
            <h3>BANK OF KHYBER</h3>
           </div>
          <div class=""Bank"">
            <h4>
             A/C Title: Student Admission Fee <br />
              A/C No: 0014-2007525659 <br />
              IBAN No: PK49KHYB00142007525659
            </h4>

          </div>

          <div class =""bank_header"">
            <h3>NATIONAL BANK OF PAKISTAN</h3>
           </div>

          <div class=""Bank"">
            <h4>
              A/C Title: Student Fee Account <br />
              A/C No: 2330-3168083826 <br />
              IBAN No: PK74NBPA2330003168083826
            </h4>

<div class =""bank_header"">
            <h3>Note: No IBT charges will be apply.</h3>
           </div>
          </div>
          
          <div
          class=""footer""
          >
            <div
            class=""footer_names""
            >
              Student Signature
            </div>
            <div class=""footer_names"">
              Cashier
            </div>
            <div  class=""footer_names"">
              Bank Officer
            </div>
          </div>

        </div>";

            template += @"
        <div class=""paper"">
          <div class =""headerDiv"">
               <div class = ""logo"">
              <img src=""D:\PakAustria\CMS\CMS/pfiast.png"" style='max-width: 3em;'>
            </div>
            <h6>
              Pak-Austria Fachhochschule: <br> Institute of Applied Sciences and Technology,
               Khanpur Road, Mang Haripur, Pakistan
            </h6> 

           </div>

            <div class =""addmission-fee"">
                 <h1>" + CurrentSemesterNo + @" SEMESTER FEE </h1>
            </div>

          <div class = ""semester"">
 <p id = ""sp1"">Session:" + db.Sessions.Find(SessionID).Session1 + @"</p>
            <p id = ""sp2"">Institute Copy</p>

          </div>

          <div class=""date"">
            <p>Bank Branch: _____________________________</p>
          </div>

          <div class=""date"">
            <p>Date: _____________________________</p>
          </div>
<div class = ""due_date"">
            <span class=""voucherno"">Voucher No." + VoucherNo + @"</span>

          </div>
 <hr>
         <div class = ""due_date"">
            <span class=""voucherno"">Issue:" + IssueDate + @"</span>
            <span class=""voucherno"">Due : " + DueDate + @"</span>

          </div>
          <hr>
          
          <div class = ""personal_details"">
            <table class = ""table1"">
             
              <tr>
                <td>Reg No:</td>
                <td>" + student.RegNo + @"</td>
              </tr>
              <tr>
                <td>Name:</td>
                <td>" + receiverName + @"</td>
              </tr>
              <tr>
                <td>  Mobile No:</td>
                <td>" + student.Admission.MobileNo + @"</td>
              </tr>
              <tr>
                <td>  Program:</td>
                <td>" + student.ProgramOffered.DepartmentProgram.Program.ProgramTitle + @"</td>
              </tr>
              
            </table>

          </div>

          <div id=""fee-details"">
                <table id = ""table2"">
                  <tr>
                    <th>Particulars</th>
                    <th>Amount (PKR)</th>
                  </tr>";

            decimal tuitionAmount3 = 0;
            foreach (var item in studentFee)
            {
                string formattedFee = string.Format("{0:#,0}/-", item.Amount * multiply);
                if (item.FeeHead.FeeType == "Tuition Charges")
                {
                    // Add the tuition fee to the accumulated amount
                    tuitionAmount3 += item.Amount;
                }
                else
                {
                    // Display other fee items
                    template += " <tr>" +
                        "<td>" + item.FeeHead.HeadName + "</td>" +
                        "<td>" + formattedFee + "</th>" +
                        "</tr>";
                }
            }
            // After iterating, check if there was any tuition fee to display
            if (tuitionAmount3 > 0)
            {
                template += " <tr>" +
                    "<td>Tuition Fee</td>" +
                    "<td>" + string.Format("{0:#,0}/-", tuitionAmount3 * multiply) + "</th>" +
                    "</tr>";
            }

            template += @"
                    
                  <tr>
<td><strong>Total:</strong</td>
<td><strong>" + string.Format("{0:#,0}/-", Total) + @"</strong></td>
</tr>
                </table>
        
            </div>

          <div class= ""fee_in_word""> 
             <h5 class= ""fee_in_words"">" + @TotalinWords + @" only.</h5>
          </div>

          <div class = ""note"">
          <h5><strong>Note:</strong></h5>
          <h5>The student must pay the bill till due date.
The deposited copy shall be uploaded to the admission portal. In case of a problem, please email it to the admissions@paf-iast.edu.pk</h5>
         
          </div>

          <div class =""bank_header"">
            <h3>BANK OF KHYBER</h3>
           </div>
          <div class=""Bank"">
            <h4>
             A/C Title: Student Admission Fee <br />
              A/C No: 0014-2007525659 <br />
              IBAN No: PK49KHYB00142007525659
            </h4>

          </div>

          <div class =""bank_header"">
            <h3>NATIONAL BANK OF PAKISTAN</h3>
           </div>

          <div class=""Bank"">
           <h4>
              A/C Title: Student Fee Account <br />
              A/C No: 2330-3168083826 <br />
              IBAN No: PK74NBPA2330003168083826
            </h4>

<div class =""bank_header"">
            <h3>Note: No IBT charges will be apply.</h3>
           </div>
          </div>
          
          <div
          class=""footer""
          >
            <div
            class=""footer_names""
            >
              Student Signature
            </div>
            <div class=""footer_names"">
              Cashier
            </div>
            <div  class=""footer_names"">
              Bank Officer
            </div>
          </div>

        </div>
 
  </div>
</body>
</html>

";

            // Convert the HTML template to a PDF document
            SelectPdf.PdfDocument doc = converter.ConvertHtmlString(template);

            // Save the PDF to a memory stream
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            doc.Save(memoryStream);
            doc.Close();

            // Set the response headers for displaying the PDF in the browser
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "inline;filename=FeeVoucher.pdf");
            Response.BinaryWrite(memoryStream.ToArray());
            Response.End();

            return File(memoryStream, "application/pdf", "FeeVoucher.pdf");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UploadFee(HttpPostedFileBase file)
        {
            var VoucherNo = Request.Form["VoucherNo"];
            var BankVoucherNo = Request.Form["BankVoucherNo"];
            var BankName = Request.Form["BankName"];
            var BranchCode = Request.Form["BranchCode"];
            var BankBranch = Request.Form["BankBranch"];
            var PaymentDate = Request.Form["PaymentDate"];

            if ((file != null) && (BankVoucherNo != null) && (BankName != null) && (BranchCode != null) && (BankBranch != null) && (PaymentDate != null)
                && (string.Equals(Path.GetExtension(file.FileName), ".jpg", StringComparison.OrdinalIgnoreCase)
       || string.Equals(Path.GetExtension(file.FileName), ".png", StringComparison.OrdinalIgnoreCase)
       || string.Equals(Path.GetExtension(file.FileName), ".jpeg", StringComparison.OrdinalIgnoreCase)))
            {
                var studentFees = db.StudentFees.Where(t => t.VoucherNo == VoucherNo).ToList();

                var filename = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/FeeVouchers/"), VoucherNo + Path.GetExtension(file.FileName));

                if (!Directory.Exists(Path.GetDirectoryName(path)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }
                file.SaveAs(path);

                foreach (var item in studentFees)
                {
                    item.BankVoucherNo = BankVoucherNo;
                    item.BankName = BankName;
                    item.BranchCode = BranchCode;
                    item.BankBranch = BankBranch;
                    item.IsPaid = true;
                    item.IsFake = false;
                    item.PaymentDate = _dateTimeProvider.CurrentDate;
                    item.PaymentTime = _dateTimeProvider.CurrentTime;
                    item.Attachment = VoucherNo + Path.GetExtension(file.FileName);
                    item.UDate = _dateTimeProvider.CurrentDate;
                }
                await db.SaveChangesAsync();

                TempData["Success"] = "<div class=\"alert alert-success alert-dismissible fade in\" role=\"alert\">\r\n  " +
                    "<a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\" style=\"font-size:30px\">&times;</a>" +
                    "<strong>Excellent! </strong>" +
                    "We have received your deposited fee information, which will be verified by the PAF-IAST Account Section. Upon successful verification, your fee will be received. Thank you for your cooperation." +
                    "</div>";

                return RedirectToAction("AllFee", "MyFees");
            }
            else
            {
                TempData["Failed"] = "All fields are required in proper format.";
                return RedirectToAction("AllFee", "MyFees");
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