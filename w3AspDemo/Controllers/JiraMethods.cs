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
    public static class JiraMethods
    {
        public static CookieContainer jiraAuthentication()
        {
            CookieContainer cookies = new CookieContainer();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://dev-aus-jira-01.swdev.local/rest/api/2/dashboard?os_username=peter.drobec&os_password=pe_dro123");
            request.CookieContainer = cookies;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Console.WriteLine(cookies.Count.ToString());
            return cookies;
        }

        //Method for deserialization of JSON for particular jira issue
        public static JiraTicket deserializeTicket(string url, CookieContainer cookies)
        {
            string ticketJson;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.CookieContainer = cookies;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            ticketJson = readHttpResponse(response);

            JiraTicket ticket = JsonConvert.DeserializeObject<JiraTicket>(ticketJson, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return ticket;
        }

        public static FilterResults deserializeFilterResults(string url, CookieContainer cookies)
        {
            string resultJson;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.CookieContainer = cookies;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            resultJson = readHttpResponse(response);

            FilterResults results = JsonConvert.DeserializeObject<FilterResults>(resultJson, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return results;

        }

        //method returns start and end date of running sprints
        public static void getStartAndEndDate(CookieContainer cookies, out DateTime start, out DateTime end)
        {
            string resultJson;
            List<string> sprintId = new List<string>();
            DateTime tempDt1;
            DateTime tempDt2;
            start = System.DateTime.Now;
            end = System.DateTime.Now;

            //getting list of sprints for given rapidboard id = 360
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://dev-aus-jira-01.swdev.local/rest/greenhopper/1.0/sprintquery/360");
            request.CookieContainer = cookies;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            resultJson = readHttpResponse(response);

            JObject o = JObject.Parse(resultJson);
            JArray sprints = (JArray)o["sprints"];

            //selecting active sprints Ids
            var activeSprintIds =
                from f in sprints
                where (string)f["state"] == "ACTIVE"
                select (string)f["id"];

            string resultJson2;
            List<string> sprintDetails = new List<string>();

            //getting json with details of all active sprints
            foreach (var id in activeSprintIds)
            {
                HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create("http://dev-aus-jira-01.swdev.local/rest/greenhopper/1.0/sprint/" + id + "/edit/model");
                request2.CookieContainer = cookies;
                HttpWebResponse response2 = (HttpWebResponse)request2.GetResponse();
                resultJson2 = readHttpResponse(response2);
                sprintDetails.Add(resultJson2);
            }


            foreach (var dt in sprintDetails)
            {
                JObject ob = JObject.Parse(dt);
                DateTime.TryParse((string)ob["sprint"]["startDate"], out tempDt1);
                if (tempDt1 < start) start = tempDt1;
                DateTime.TryParse((string)ob["sprint"]["endDate"], out tempDt2);
                if (tempDt2 > end) end = tempDt2;
            }

        }

        public static string readHttpResponse(HttpWebResponse response)
        {
            string resultJson;
            using (var s = new StreamReader(response.GetResponseStream()))
            {
                resultJson = s.ReadToEnd();
            }
            return resultJson;
        }

        //this method calculates count of opened issues grouped by priority
        public static Result getPriorityStats(List<JiraTicket> jtickets)
        {
            Result x = new Result("Priority", "Count");
            var priorityGroups =
                from ticket in jtickets
                where ticket.fields.issuetype.name == "Bug"
                group ticket by ticket.fields.priority.name into priorityGroup
                select priorityGroup;

            
               
                foreach (var pg in priorityGroups)
                {
                   //var p = new Tuple<string, string>(pg.Key, pg.Count<JiraTicket>().ToString());
                    x.addItem(pg.Key, pg.Count<JiraTicket>().ToString());
                }
                x.closeArray();
                return x;
        }

        //this method calculates count of issues in individual status
        public static Result getStatusStats(List<JiraTicket> jtickets)
        {
            Result x = new Result("Status", "Count");
            var statGroups =
                from ticket in jtickets
                where ticket.fields.issuetype.name == "Bug"
                group ticket by ticket.fields.status.name into statGroup
                select statGroup;

            
                foreach (var sg in statGroups)
                {
                    x.addItem(sg.Key, sg.Count<JiraTicket>().ToString()); 
                }
                x.closeArray();
                return x;
        }

        //this method returns count of issues opened in individual days
        public static Result getBugsPerDayStats(List<JiraTicket> jtickets)
        {
            Result x = new Result("Day", "Count");
            var dayGroups =
                from ticket in jtickets
                where ticket.fields.issuetype.name == "Bug"
                group ticket by DateTime.Parse(ticket.fields.created).ToString("yyyy/MM/dd") into dayGroup
                select dayGroup;

            foreach (var dg in dayGroups)
                {
                    x.addItem(dg.Key, dg.Count<JiraTicket>().ToString());    
                }
            x.closeArray();
            return x;
        }

        //this method returns list of items with undefined priority and creator name
        public static Result getUndefinedBugs(List<JiraTicket> jtickets)
        {
            Result x = new Result();
            var undefined =
               from ticket in jtickets
               where ticket.fields.priority.name == "Undefined"
               select new { ticket.key, ticket.fields.creator.displayName };

          
                foreach (var un in undefined)
                {
                    x.ArrayData += System.Environment.NewLine;
                    x.ArrayData += un.key + " " + un.displayName + " http://dev-aus-jira-01.swdev.local/browse/"+un.key;
                }
            return x;
          }        
    }
}