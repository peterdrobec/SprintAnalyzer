using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace w3AspDemo.Controllers
{
    public class JiraTasksToJSONController : ApiController
    {
        // GET api/jirataskstojson
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/jirataskstojson/5
        [System.Web.Http.HttpGet]
        public Object GetTickets([FromUri]string name, [FromUri]string password, [FromUri]string jql)
        {
            try
            {
                var res = w3AspDemo.Controllers.JiraMethods.deserializeFilterResults(name, password, jql);
                var response = from jt in res.issues
                               select new { jt.key, jt.fields.issuetype.name, jt.fields.summary, jt.fields.customfield_10008 };
                return response;
            }
            catch (Exception ex)
            {
                return ("Bad Request with parameters: username="+name+" password:"+password+" jql:"+jql+" "+ ex.Message);
            }
        }

        // POST api/jirataskstojson
        public void Post([FromBody]string value)
        {
        }

        // PUT api/jirataskstojson/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/jirataskstojson/5
        public void Delete(int id)
        {
        }
    }
}
