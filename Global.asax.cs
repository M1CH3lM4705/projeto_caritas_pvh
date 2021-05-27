using ProjetoBetaAutenticacao.Models.Extensions;
using ProjetoBetaAutenticacao.Utilidades;
using System;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ProjetoBetaAutenticacao
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            
            this.CheckRoles();
            this.CriarSuperUser();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalFilters.Filters.Add(new FormatExceptionAttribute());
        }

        void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();

            if (ex is HttpRequestValidationException)
            {
                Response.Clear();
                Response.StatusCode = 200;
                Response.ContentType = "application/json";
                Response.Write("{\"Resultado\":\"AVISO\",\"Mensagens\":[\"Somente texto sem caracateres especiais podem ser enviado.\"],\"Resultado\":}");
                Response.End();
            }
            else if (ex is HttpAntiForgeryException)
            {
                Response.Clear();
                Response.StatusCode = 200;
                Response.End();
            }
        }

        private void CheckRoles()
        {
            Utils.CheckRole("Admin");
            Utils.CheckRole("Voluntario_N1");
            Utils.CheckRole("Voluntario_N2");
        }

        private void CriarSuperUser()
        {
            Utils.CheckSuperUser("Admin");
            Utils.ChangeUserParoquia("Admin", "Caritas", "Porto Velho");
        }
    }
}
