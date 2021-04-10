using ProjetoBetaAutenticacao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjetoBetaAutenticacao.Controllers
{
    public class MembrosFamiliarController : Controller
    {
        // GET: MembrosFamiliar
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PartialMembroFamiliar()
        {
            var membro = new MembroFamilia();
            return PartialView("_MembroFamilia", membro);
        }
    }
}