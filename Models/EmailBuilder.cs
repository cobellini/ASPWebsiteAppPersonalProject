using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ComicWebsite.Models;
using System.Net.Mail;
using System.Net;

namespace ComicWebsite.Models
{
    public class EmailBuilder : Controller
    {
        [NonAction]
        public void SendVerificationLinkEmail(NewUser newUser, int code)
        {
            
                var fromEmail = new MailAddress("EmailAddress", "Email Subject");
                var toEmail = new MailAddress(newUser.Email);
                var fromEmailPassword = "EmailPassword";
                string subject = "Confirm your email";

                string body = "<br></br>Please input the code link to confirm your email <br></br>" + code;

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
                };

                using (var message = new MailMessage(fromEmail, toEmail)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                    smtp.Send(message);
        
        }

        public void SendPasswordReset(AccountRecovery Recover, int code)
        {

            var fromEmail = new MailAddress("EmailAddress", "Email Subject");
            var toEmail = new MailAddress(Recover.givenEmail);
            var fromEmailPassword = "EmailPassword";
            string subject = "Change your password";

            string body = "<br></br>Please input the code link to change your password<br></br>" + code;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }
        
    }
}