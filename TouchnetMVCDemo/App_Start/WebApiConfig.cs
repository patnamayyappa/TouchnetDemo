using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Cmc.Core.ComponentModel;
using Cmc.Core.PaymentProvider.Interfaces;

namespace TouchnetMVCDemo
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );


            // Add default route using convention-based routing
            config.Routes.MapHttpRoute(
                "PaymentApi",
                routeTemplate: "Core/api/{controller}/{Action}/{operationType}");
        }

        public class ContainerConfig
        {
            public static void RegisterContainer()
            {

               

                var containerWrapper = ServiceLocator.Default as IUnderlyingContainer;
                var container = containerWrapper.Container as ILifetimeScope;
                //// Create the depenedency resolver.
                var resolver = new AutofacWebApiDependencyResolver(container);
                //// Configure Web API with the dependency resolver.
                GlobalConfiguration.Configuration.DependencyResolver = resolver;
            }
        }
    }
}
