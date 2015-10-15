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
            results.getSprintResults();
           
            Application.Add("Results", results);

          // var auth = w3AspDemo.Controllers.JiraMethods.jiraAuthentication();
           //var res = w3AspDemo.Controllers.JiraMethods.deserializeFilterResults(@"http://dev-aus-jira-01.swdev.local/rest/api/2/search?jql=project = ""Unified IT Manager"" AND Sprint is EMPTY AND fixVersion is EMPTY AND status = Open AND issuetype != Epic", auth);
            var res = w3AspDemo.Controllers.JiraMethods.deserializeFilterResults("peter.drobec", "pe_dro123", @"project = ""Unified IT Manager"" and Sprint = ""Sprint 1 SNI - Legend""");
            w3AspDemo.Controllers.PdfCreator.PdfPrinter(res);

        }
    }
}
