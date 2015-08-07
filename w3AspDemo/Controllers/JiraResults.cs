using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using System.Timers;
using System.Web;


namespace w3AspDemo.Controllers
{
    public class JiraResults
    {
        public Result PriorityStats { get; set; }
        public Result StatusStats { get; set; }
        public Result BugsPerDayStats { get; set; }
        public Result UndefinedBugs { get; set; }

        public DateTime TimeStamp { get; set; }

        public void getResults()
        {
            CookieContainer cookies = new CookieContainer();
            FilterResults results = new FilterResults();
            string url;
            DateTime sprintStartDate = DateTime.Now;
            DateTime sprintEndDate = DateTime.Now;

            cookies = JiraMethods.jiraAuthentication();
            JiraMethods.getStartAndEndDate(cookies, out sprintStartDate, out sprintEndDate);
            url = "http://dev-aus-jira-01.swdev.local/rest/api/2/search?jql=project+%3D+%22Unified+IT+Manager%22+AND+created>" + "'" + sprintStartDate.ToString("yyyy-MM-dd") + "'" + "AND+created<=" + "'" + sprintEndDate.ToString("yyyy-MM-dd") + "'";
            results = JiraMethods.deserializeFilterResults(url, cookies);

            this.PriorityStats = JiraMethods.getPriorityStats(results.issues);
            this.StatusStats =  JiraMethods.getStatusStats(results.issues);
            this.BugsPerDayStats = JiraMethods.getBugsPerDayStats(results.issues);
            this.UndefinedBugs = JiraMethods.getUndefinedBugs(results.issues);
            this.TimeStamp = DateTime.Now;
        }
    }

    public class Result
    {
        public int Total { get; set; }
        public string ArrayData {get; set;}

        public Result(string a, string b)
        {
            this.Total = 0;
            this.ArrayData = "[['"+a+"' , '"+b+"']";
            
        }
        public void addItem(string x, string y)
        {
            this.ArrayData += ", ['" + x + "' , " + y + " ]";
            this.Total += 1;
        }

      
        public void closeArray()
        {
            this.ArrayData += "]";
        }
    }
}