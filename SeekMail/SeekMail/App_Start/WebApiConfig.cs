using System.Web.Http;

namespace SeekMail
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "MessagesApi",
                routeTemplate: "api/messages/{page}",
                defaults: new { controller = "Messages", page = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "SendMessageApi",
                routeTemplate: "api/templates/{templateId}/send",
                defaults: new {controller = "Messages"});

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
