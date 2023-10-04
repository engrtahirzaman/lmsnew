using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS.Controllers
{
    [App_Start.SessionAuthentication]
    public class MyDashboardController : Controller
    {
        public ActionResult RedirectToLoginPage()
        {
            return RedirectToAction("Login", "Account", new { Area = "StudentAccount" });//Redirect to login...
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}