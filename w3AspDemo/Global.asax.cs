using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Http;
using Newtonsoft.Json;

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
            GlobalConfiguration.Configure(WebApiConfig.Register);

            //var json = config.Formatters.JsonFormatter;
            //json.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            
            w3AspDemo.Controllers.JiraResults results = new w3AspDemo.Controllers.JiraResults();
            results.getSprintResults();
           
            Application.Add("Results", results);

          
            var res = w3AspDemo.Controllers.JiraMethods.deserializeFilterResults("peter.drobec", "pe_dro123", @"project = ""Unified IT Manager"" AND fixVersion = ServiceNow AND status = Open AND (""Scrum Team"" = Legend OR ""Scrum Team"" is EMPTY) ORDER BY Rank");
            w3AspDemo.Controllers.PdfCreator.PdfPrinter(res);

        }
    }
}
