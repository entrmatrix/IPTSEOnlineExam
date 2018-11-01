using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IPTSEOnlineExam.AuthData
{
    public class RefreshDetectFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            filterContext.HttpContext.Response.SetCookie(new HttpCookie("RefreshFilter", filterContext.HttpContext.Request.Url.ToString()));
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var cookie = filterContext.HttpContext.Request.Cookies["RefreshFilter"];
            filterContext.RouteData.Values["IsRefreshed"] = cookie != null &&
                                                            cookie.Value == filterContext.HttpContext.Request.Url.ToString();
        }
    }
}