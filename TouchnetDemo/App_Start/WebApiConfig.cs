using System.Web.Http;

namespace TouchnetDemo
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Enable attribute routing
            config.MapHttpAttributeRoutes();

            // Add default route using convention-based routing
            config.Routes.MapHttpRoute(
                "PaymentApi",
                routeTemplate: "Core/api/{controller}/{Action}/{operationType}");
        }
    }
}