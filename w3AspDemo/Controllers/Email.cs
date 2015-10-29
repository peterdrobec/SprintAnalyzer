using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;

namespace w3AspDemo.Controllers
{
    public static class Email
    {
        public static void sendEmail(Result r)
        {
            MailMessage m = new MailMessage("sprinthealtanalyzer@solarwinds.com", "peter.drobec@solarwinds.com");
            m.Subject = "Daily Priority Check";
            m.Body = r.ArrayData;
            //m.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "10.110.108.21";
            smtp.UseDefaultCredentials = false;
            smtp.Send(m);
        }
    }
}