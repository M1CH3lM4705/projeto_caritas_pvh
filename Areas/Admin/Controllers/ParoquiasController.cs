using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using PagedList;
using ProjetoBetaAutenticacao.Areas.Admin.Data;
using ProjetoBetaAutenticacao.Infraestrutura;
using ProjetoBetaAutenticacao.Models.Filters;

namespace ProjetoBetaAutenticacao.Areas.Admin.Controllers
{
    [AccessDeniedAuthorize(Roles = "Admin")]
    public class ParoquiasController : Controller
    {
        private readonly CaritasContext db = new CaritasContext();

        // GET: Admin/Paroquias
        public ActionResult Index(string pesquisa, int? pagina, int? tamanhoPag)
        {
            var paroquia = db.Paroquias.OrderByDescending(p => p.ParoquiaId);
            int numeroRegistros = (tamanhoPag ?? 5);
            int numeroPagina = (pagina ?? 1);
            ViewBag.ListaTamanhoPag = new SelectList(new int[] { numeroRegistros, 10, 15, 20 }, numeroRegistros);
            ViewBag.Pesquia = pesquisa;
            ViewBag.Pagina = pagina;
            if (!string.IsNullOrEmpty(pesquisa))
            {
                paroquia = db.Paroquias.Where(p => p.Nome.Contains(pesquisa)).OrderBy(p => p.Nome);
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_ListaParoquias", paroquia.ToPagedList(numeroPagina, numeroRegistros));

            }
            return View(paroquia.ToPagedList(numeroPagina, numeroRegistros));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Detalhes(long? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paroquia paroquia = db.Paroquias.Find(id);
            if (paroquia == null)
            {
                return HttpNotFound();
            }
            var idM = paroquia.Cidade;
            Municipio municipio = new Municipio();
            paroquia.Cidade = municipio.BuscarCidade(idM).Nome;
            return PartialView("_Detalhes",paroquia);
        }

        // GET: Admin/Paroquias/Create
        //Metodo de Criar e alterar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FormParoquia(int? id)
        {
            Paroquia paroquia = new Paroquia();
            if (id != 0)
            {
                paroquia = db.Paroquias.Find(id);
                if(paroquia == null)
                {
                    return HttpNotFound();
                }
                Municipio municipio = new Municipio();
                var idM = paroquia.Cidade;
                ViewBag.Cidade = municipio.BuscarCidade(idM);
            }
            
            return PartialView("_Create", paroquia);
        }

        // POST: Admin/Paroquias/Create
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SalvarParoquia(Paroquia paroquia)
        {
            var resultado = "OK";
            var mensagens = new List<string>();
            var idSalvo = string.Empty;
            
            if (!ModelState.IsValid)
            {
                resultado = "AVISO";
                mensagens = ModelState
                    .Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage)
                    .ToList();
            }
            else
            {
                try
                {
                    var existeParoquia = db.Paroquias
                        .Where(p => p.ParoquiaId == paroquia.ParoquiaId)
                        .AsNoTracking()
                        .FirstOrDefault();
                    if (existeParoquia == null)
                    {
                        if (db.Paroquias.Any(p => p.Nome == paroquia.Nome))
                        {
                            ModelState.AddModelError("Nome", $"Paróquia {paroquia.Nome} Ja registrada!");
                        }
                        existeParoquia = paroquia;
                        db.Paroquias.Add(existeParoquia);
                    }
                    else
                    {
                        existeParoquia.ParoquiaId = paroquia.ParoquiaId;
                        existeParoquia.Nome = paroquia.Nome;
                        existeParoquia.Email = paroquia.Email;
                        existeParoquia.Estado = paroquia.Estado;
                        existeParoquia.Cidade = paroquia.Cidade;
                        db.Entry(existeParoquia).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    idSalvo = existeParoquia.ParoquiaId.ToString();
                }
                catch (Exception)
                {
                    resultado = "ERRO";
                }
            }

            return Json(new {Resultado = resultado, Mensagens = mensagens, Idsalvo = idSalvo }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeletarParoquia(int paroquiaId)
        {
            bool result = false;
            var mensagem = "";
            Paroquia paroquia = db.Paroquias.Where(v => v.ParoquiaId == paroquiaId).SingleOrDefault();
            if (paroquia != null)
            {
                try
                {
                    //foreach(var v in paroquia.Voluntarios)
                    //{
                    //    if(v.ParoquiaId == paroquia.ParoquiaId)
                    //    {
                    //        v.Ativo = false;
                    //        db.Voluntarios.Attach(v);
                    //        db.Entry(v).Property(x => x.Ativo).IsModified = true;
                    //    }
                    //}
                    db.Paroquias.Remove(paroquia);
                    db.SaveChanges();
                    result = true;
                    mensagem = "Paroquia foi excluída com sucesso";
                }
                catch (Exception ex)
                {
                    mensagem = "Ocoreu um erro ao excluir";
                }
            }
            return Json(new { Resultado = result, Mensagem = mensagem  }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ValidarParoquia(string nomeParoquia)
        {
            var name = db.Paroquias.Where(v => v.Nome == nomeParoquia).Count() == 0;
            return Json(name, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
