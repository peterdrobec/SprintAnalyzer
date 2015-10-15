using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using System.Web.HttpApplicationState;


namespace w3AspDemo.Controllers
{
    public class HomeController : Controller
    {
       
        public ActionResult Index()
        {
            if ((DateTime.Now - ((JiraResults)HttpContext.ApplicationInstance.Application.Get("Results")).TimeStamp).TotalMinutes < 15)
            {
                return View((JiraResults)HttpContext.ApplicationInstance.Application.Get("Results"));
            }
            else
            {
                ((JiraResults)HttpContext.ApplicationInstance.Application.Get("Results")).getSprintResults();
                return View((JiraResults)HttpContext.ApplicationInstance.Application.Get("Results"));
            }
            
        }

        public ActionResult About()
        { return View(); }

        public ActionResult Contact()
        { return View(); }
    }
}