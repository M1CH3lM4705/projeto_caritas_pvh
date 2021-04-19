using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ProjetoBetaAutenticacao
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "Autenticacao",
                url: "Autenticacao/{action}/",
                defaults: new { controller = "Autenticacao", action = "Login" }
            );

            routes.MapRoute(
                name: "Encaminhamentos",
                url: "PessoaCarente/Encaminhamento/Index/{id}",
                defaults: new { controller = "Encaminhamento", action = "Index" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] {"ProjetoBetaAutenticacao.Controllers"}
            );
        }
    }
}
