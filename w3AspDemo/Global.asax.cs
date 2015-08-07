using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace w3AspDemo
{
    public class MvcApplication : System.Web.HttpApplication
    {
      
        protected void Application_Start()
        {
           // HttpApplicationState appState;
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
            w3AspDemo.Controllers.JiraResults results = new w3AspDemo.Controllers.JiraResults();
            results.getResults();
           
            Application.Add("Results", results);

        }

        //public w3AspDemo.Controllers.JiraResults getResults()
        //{
        //    return this.Results;
        //}
    }
}
