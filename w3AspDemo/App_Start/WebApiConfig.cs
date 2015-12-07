using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net.Http.Headers;


namespace w3AspDemo
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
			config.MapHttpAttributeRoutes();
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

        //    config.Routes.MapHttpRoute(
        //        name: "DefaultApi",
        //        routeTemplate: "api/{controller}/{action}/{name}",
        //        defaults: null
        //    );

        //    config.Routes.MapHttpRoute(
        //        name: "DefaultApi4",
        //        routeTemplate: "api/{controller}/{action}",
        //        defaults: null
        //    );

            config.Routes.MapHttpRoute(

            name: "DefaultApi2",

            routeTemplate: "api/{controller}/{action}/{id}",

            defaults: new { id = RouteParameter.Optional }

        );

            config.Routes.MapHttpRoute(
                name: "DefaultApi3",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

        }
    }
}
