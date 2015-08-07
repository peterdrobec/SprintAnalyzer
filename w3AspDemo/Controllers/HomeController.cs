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
            //JiraResults results = new JiraResults();
            JiraResults results = (JiraResults)HttpContext.ApplicationInstance.Application.Get("Results");
            if ((DateTime.Now - results.TimeStamp).TotalMinutes < 15)
            {
                return View(results);
            }
            else
            {
                results.getResults();
                return View(results);
            }
        }

        public ActionResult About()
        { return View(); }

        public ActionResult Contact()
        { return View(); }
    }
}