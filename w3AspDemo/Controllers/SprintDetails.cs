using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading.Tasks;

namespace w3AspDemo.Controllers
{
    public class Sprint
    {
        public int id { get; set; }
        public int sequence { get; set; }
        public string name { get; set; }
        public string state { get; set; }
        public int linkedPagesCount { get; set; }
    }

    public class RapidSprints
    {
        public List<Sprint> sprints { get; set; }
        public int rapidViewId { get; set; }
    }
}