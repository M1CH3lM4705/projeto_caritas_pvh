using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace ProjetoBetaAutenticacao.Models.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetParoquia(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Paroquia");

            return (claim != null) ? claim.Value : string.Empty;
        }
    }
}