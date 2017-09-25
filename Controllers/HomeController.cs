using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ComicWebsite.Models;
using System.Net.Mail;
using System.Net;

namespace ComicWebsite.Controllers
{
    public class HomeController : Controller
    {

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult About()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            
            return View();
        }

        [HttpGet]
        public ActionResult EmailConfirmation()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUpForm(NewUser newUser, EmailBuilder mailBuilder)
        {
            if (string.IsNullOrEmpty(newUser.Email))
            {
                
                ModelState.AddModelError("Email", "Please enter a valid Email");
            }
            if (string.IsNullOrEmpty(newUser.Password))
            {
                ModelState.AddModelError("Password", "Please enter a valid Password");
            }
            if (string.IsNullOrEmpty(newUser.Username))
            {
                ModelState.AddModelError("Username", "Please enter a valid Password");
            }

            if (ModelState.IsValid)
            {


                if (newUser.checkUser() == true)
                {

                    ModelState.AddModelError("ErrorMessage", "User already exists");
                }
                else
                {
                    newUser.generateSignUpCode();                    
                    mailBuilder.SendVerificationLinkEmail(newUser, newUser.generatedCode);
                    TempData["verificationCode"] = newUser.generatedCode;
                    TempData["NewUser"] = newUser;
                   
                    return RedirectToAction("EmailConfirmation");

                }

            }

            return View("SignUp", newUser);

        }

        [HttpPost]
        public ActionResult LoginForm(UserLogin loginUser)
        {
            PasswordHash passhash = new PasswordHash();
            if (string.IsNullOrEmpty(loginUser.Email))
            {
                ModelState.AddModelError("Email", "Please enter a valid Email");
            }
            if (string.IsNullOrEmpty(loginUser.Password))
            {
                ModelState.AddModelError("Password", "Please enter a valid Password");
            }

            if (ModelState.IsValid)
            {
                if (loginUser.LoginUserCheck())
                {
                    Session["Logged"] = true;
                    return RedirectToAction("Index");
                    
                }
                else
                {
                    ModelState.AddModelError("ErrorMessage", "Invalid User");
                    
                   


                }
            }
           
            return View("Login", loginUser);
        }

        [HttpPost]
        public ActionResult EmailConfirmation(NewUser newUser)
        {
            int verificode = (int)TempData["verificationCode"];
            TempData.Keep("verificationCode");

            if (newUser.givenCode == verificode)
            {
               newUser = (NewUser)TempData["NewUser"];
               newUser.InsertNewUser();

               return RedirectToAction("Index");

            }
            else
            { 
               return View(newUser);
            }           
        }
    }
}