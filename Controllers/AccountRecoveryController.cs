using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ComicWebsite.Models;

namespace ComicWebsite.Controllers
{
    public class AccountRecoveryController : Controller
    {

        [HttpGet]
        public ActionResult PasswordResetEmail()
        {

            return View();
        }

        [HttpPost]
        public ActionResult PasswordResetEmail(AccountRecovery pwrecover, EmailBuilder emailer)
        {

            if (ModelState.IsValid)
            {
                if (!pwrecover.checkEmail())
                {
                    ModelState.AddModelError("givenEmail", "Email Doesnt exist");
                    return View(pwrecover);

                }
                else
                {
                    pwrecover.generateCode();
                    emailer.SendPasswordReset(pwrecover, pwrecover.verificationCode);
                    TempData["verificationCode"] = pwrecover.verificationCode;
                    TempData["Email"] = pwrecover.givenEmail;
                    TempData["UserObject"] = pwrecover;
                    return RedirectToAction("PasswordResetCode");

                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult PasswordResetCode()
        {
           
            return View();
        }

        [HttpPost]
        public ActionResult PasswordResetCode(AccountRecovery accRecover)
        {
            /* ONCE INPUTTED CODE, CHECK IF CODES ARE THE SAME VALUE. IF IT DOESNT, THROW ERROR MESSAGE, IF IT DOES, REDIRECT TO NEW PASSWORD PAGE.. */
            //ModelState.AddModelError("givenEmail", "That code doesn't match up what we sent."); 

            int verificode = (int)TempData["verificationCode"];
            TempData.Keep("verificationCode");

            if (accRecover.givenCode == verificode)
            {
                return View("PasswordResetNewPassword");
                
            }
            else
            {
                ModelState.AddModelError("givenCode", "Invalid code given, please refresh your inbox and try again..");
              
                return View();
            }
            
            //return View(pwrecover);
        }

        [HttpGet]
        public ActionResult PasswordResetNewPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PasswordResetNewPassword(AccountRecovery accRecover)
        {
            accRecover.givenEmail = (string)TempData["Email"];
            TempData.Keep("Email");
            

            if (!accRecover.ExistingPassword())
            {
                ModelState.AddModelError("newPassword", "That is your old password");
                return View();
            } else if (accRecover.newPassword != accRecover.confirmednewPassword)
            {
                ModelState.AddModelError("newPassword", "Please confirm your new password");
                return View();
            }
            else
            {
                accRecover.ChangePassword();
                ModelState.AddModelError("newPassword", "Done");
            }
                

            










            /*CHECK IF BOTH PASSWORDS GIVEN ARE THE SAME, IF THEY ARE, UPDATE DB WITH NEW PASSWORD.. */

            //ModelState.AddModelError("newPassword", "That is your old password"); 
            //ModelState.AddModelError("confirmednewPassword", "Emails do not match up"); 
            return View();
        }






    }
}