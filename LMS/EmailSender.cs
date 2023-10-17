using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace LMS
{
    public class EmailSender : Controller
    {
        public static string SendMail(string ToEmail, string Subject, string BodyMessage)
        {

            try
            {
                // Sending email
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("donotreply@paf-iast.edu.pk");
                //mail.ReplyToList.Add(new MailAddress("admission@paf-iast.edu.pk"));
                mail.To.Add(ToEmail);
                //mail.Bcc.Add("lms@paf-iast.edu.pk");
                mail.Subject = Subject;
                mail.Body = BodyMessage;
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient("smtp.office365.com", 587);
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("donotreply@paf-iast.edu.pk", "PAF@2024");
                smtp.EnableSsl = true;
                smtp.Send(mail);

                return "Sent";
            }
            catch (Exception ex)
            {
                return null;
                // Log the exception for debugging or error tracking
                // Example: log.LogError(ex, "Error occurred while sending email.");

                try
                {
                    // Retry sending email with alternate settings or provider

                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress("donot_reply@paf-iast.edu.pk");
                    mail.To.Add(ToEmail);
                    mail.Bcc.Add("lms@paf-iast.edu.pk");
                    mail.Subject = Subject;
                    mail.Body = BodyMessage;
                    mail.IsBodyHtml = true;

                    SmtpClient smtp = new SmtpClient("mail.paf-iast.edu.pk", 465);
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential("donot_reply@paf-iast.edu.pk", "PAF@2023");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);

                    return "Sent";
                }
                catch (Exception innerEx)
                {
                    // Log the inner exception for debugging or error tracking
                    // Example: log.LogError(innerEx, "Error occurred while retrying sending email.");

                    // Handle the exception or return an error message
                    return "Failed to send email. Please try again later.";
                }
            }

        }
    }
}
