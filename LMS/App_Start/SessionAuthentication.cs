using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Security;

namespace LMS.App_Start
{
    public class SessionAuthentication : ActionFilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            if (string.IsNullOrEmpty(Convert.ToString(filterContext.HttpContext.Session["StudentID"])))
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
            {
                //Redirecting the user to the Login View of Account Controller
                string redirectOnSuccess = filterContext.HttpContext.Request.Url.PathAndQuery;
                string redirectUrl = string.Format("?ReturnUrl={0}", redirectOnSuccess);
                string loginUrl = FormsAuthentication.LoginUrl + redirectUrl;
                if (HttpContext.Current.Request.IsAuthenticated)
                {
                    FormsAuthentication.SignOut();
                }
                RedirectResult rr = new RedirectResult(loginUrl);
                filterContext.Result = rr;
            }
        }

        public void OnAuthenticationChallengee(AuthenticationChallengeContext filterContext)
        {
            if (HttpContext.Current.Session["StudentID"] == null)
            {
                string redirectOnSuccess = filterContext.HttpContext.Request.Url.PathAndQuery;
                string redirectUrl = string.Format("?ReturnUrl={0}", redirectOnSuccess);
                string loginUrl = FormsAuthentication.LoginUrl + redirectUrl;
                if (HttpContext.Current.Request.IsAuthenticated)
                {
                    FormsAuthentication.SignOut();
                }
                RedirectResult rr = new RedirectResult(loginUrl);
                filterContext.Result = rr;
            }
        }
    }
}