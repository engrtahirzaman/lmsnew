﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Session_Expired(string ReturnUrl)
        {
            return View();
        }
    }
}