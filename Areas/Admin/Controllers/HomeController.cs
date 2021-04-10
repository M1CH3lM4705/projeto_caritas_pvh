using ProjetoBetaAutenticacao.Models.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjetoBetaAutenticacao.Areas.Admin.Controllers
{
    [HandleError(View = "AcessoNegado")]
    [Authorize(Roles="Admin")]
    public class HomeController : Controller
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}