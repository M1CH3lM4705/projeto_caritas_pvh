using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProjetoBetaAutenticacao.Startup))]
namespace ProjetoBetaAutenticacao
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
