using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComicWebsite.Controllers
{
    public class AccountManagementController : Controller
    {
        // GET: AccountManagement
        [HttpGet]
        public ActionResult AccountIndex()
        {
            return View();
        }
    }
}