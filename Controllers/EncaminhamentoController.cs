using AplicacaoEscola.Helpers;
using ProjetoBetaAutenticacao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjetoBetaAutenticacao.Infraestrutura.Dao;
using System.Threading.Tasks;
using System.Net;
using System.Diagnostics;
using PagedList;

namespace ProjetoBetaAutenticacao.Controllers
{

    public class EncaminhamentoController : Controller
    {
        private readonly EncaminhamentoDao encaminhamentoDao = new EncaminhamentoDao();
        private readonly PessoaCarenteDao pessoaDao = new PessoaCarenteDao();
        

        // GET: Encaminhamento
        [Authorize(Roles = "Admin, Voluntario_N1, Voluntario_N2")]
        public ActionResult Index(int? id, int? pagina, int? tamanhoPag)//Carrega a lista de encaminhamento da Pessoa carente
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Erro na requisição Ajax");
            }
            int numeroRegistros = (tamanhoPag ?? 5);
            int numeroPagina = (pagina ?? 1);
            ViewBag.ListaTamanhoPag = new SelectList(new int[] { numeroRegistros, 10, 15, 20 }, numeroRegistros);
            ViewBag.Id = id;
            var listaEnc = encaminhamentoDao.ListaEncaminhar(Convert.ToInt32(id));
            
            var pessoa = encaminhamentoDao.BuscarId(id);
            if (pessoa == null)
            {
                var p = pessoaDao.BuscarId(id);
                ViewBag.Nome = p.Nome;
                var lista = new List<Encaminhamento>(p.PessoaCarenteId);
                
               
                return View(lista.ToPagedList(numeroPagina, numeroRegistros));
            }
            ViewBag.Nome = pessoa.PessoaCarente.Nome;
            if (pessoa.DataEntrega != null)
            {
                var data = pessoa.DataString(encaminhamentoDao.DataMaxima(id));
                ViewBag.Data = data;
            }
            
            ViewBag.TamanhoPagina = numeroRegistros;
            ViewBag.Pagina = pagina;
            if (Request.IsAjaxRequest())
            {
                return PartialView("_ListaEncaminhamentos", listaEnc.ToPagedList(numeroPagina, numeroRegistros));
            }
            return View(listaEnc.ToPagedList(numeroPagina, numeroRegistros));
        }

        
       
        [Authorize(Roles = "Admin, Voluntario_N1, Voluntario_N2")]

        public PartialViewResult ListaEncaminhamento(int id, int? pagina, int? tamanhoPag)
        {
            var listaEnc = encaminhamentoDao.ListaEncaminhar(id);

            int numeroPagina = (pagina ?? 1);
            int numeroRegistros = (tamanhoPag ?? 5);
            ViewBag.Id = id;
            ViewBag.TamanhoPagina = numeroRegistros;

            ViewBag.Pagina = pagina;
            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Views/Encaminhamento/_ListaEncaminhamentos.cshtml", listaEnc.ToPagedList(numeroPagina, numeroRegistros));
            }
            return PartialView("~/Views/Encaminhamento/_ListaEncaminhamentos.cshtml", listaEnc.ToPagedList(numeroPagina, numeroRegistros));
        }
        
        [HttpPost]
        [Authorize(Roles = "Admin, Voluntario_N1, Voluntario_N2")]
        [ValidateAntiForgeryToken]
        public JsonResult AtualizaData(int id)
        {
            var enc = encaminhamentoDao.BuscarId(id);
            if (enc.DataEntrega != null)
            {
                var data = enc.DataString(encaminhamentoDao.DataMaxima(id));
                ViewBag.Data = data;
            }
            return Json(new { dataEntrega = ViewBag.Data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Voluntario_N1")]
        [ValidateAntiForgeryToken]
        public ActionResult ModalEncaminhamento(int id)
        {
            var encaminhamento = new Encaminhamento
            {
                PessoaCarenteId = id
            };
            return PartialView("_NovoEncaminhamento", encaminhamento);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Voluntario_N1")]
        [ValidateAntiForgeryToken]
        public ActionResult SalvarEncaminhamento(Encaminhamento encaminhamento)
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
                    var existeEnc = encaminhamentoDao.Buscar(encaminhamento.EncaminhamentoId);
                    if (existeEnc == null)
                    {
                        Encaminhamento Obj_E = new Encaminhamento
                        {
                            TipoEncaminhamento = encaminhamento.TipoEncaminhamento,
                            Status = encaminhamento.Status,
                            PessoaCarenteId = encaminhamento.PessoaCarenteId
                        };
                        if (encaminhamento.Status == true)
                        {
                            encaminhamento.DataEntrega = DateTime.Now;
                            Obj_E.DataEntrega = encaminhamento.DataEntrega;
                        }
                        existeEnc = Obj_E;
                        encaminhamentoDao.Salvar(existeEnc);
                    }
                    else
                    {
                        existeEnc.TipoEncaminhamento = encaminhamento.TipoEncaminhamento;
                        existeEnc.Status = encaminhamento.Status;
                        if (existeEnc.Status == true)
                        {
                            existeEnc.DataEntrega = DateTime.Now;
                        }
                        encaminhamentoDao.Alterar(existeEnc);
                    }
                    idSalvo = existeEnc.PessoaCarenteId.ToString();
                }
                catch (Exception)
                {
                    resultado = "ERRO";
                }
            }

            return Json(new { Resultado = resultado, Mensagens = mensagens, IdSalvo = idSalvo }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        [Authorize(Roles = "Admin, Voluntario_N1")]
        [ValidateAntiForgeryToken]
        public ActionResult FinalizarEntrega(Encaminhamento encaminhamento)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var enc = new Encaminhamento();
                    enc.FinalizarEncaminhamento(encaminhamento);
                }
                catch (Exception)
                {

                    return RedirectToAction("Index", encaminhamento.PessoaCarenteId)
                        .Erro("Ocorreu um Erro a finalizar a entrega!");
                }
            }
            return Json(new
            {
                Resultado = encaminhamento.PessoaCarenteId,
                Mensagem = "O Encaminhamento foi finalizado com sucesso!"
            },
                JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [Authorize(Roles = "Admin, Voluntario_N1")]
        [ValidateAntiForgeryToken]
        public ActionResult Alterar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var encaminhamento = encaminhamentoDao.Buscar(id);
            var n = encaminhamento.TesteData(encaminhamento.DataSolicitacao);
            Debug.WriteLine(DateTime.FromOADate(n));
            if (encaminhamento == null)
            {
                return HttpNotFound();
            }
            ViewBag.Modal = "Novo Encaminhamento";
            return PartialView("_NovoEncaminhamento", encaminhamento);

        }

        [HttpPost]
        [Authorize(Roles = "Admin, Voluntario_N1")]
        public ActionResult AlterarEncaminhamento(Encaminhamento enc)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    encaminhamentoDao.Alterar(enc);
                }
                return Json(new
                {
                    resultado = enc.PessoaCarenteId,
                    message = "O Encaminhamento foi Alterado!"
                },
                        JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new
                {
                    resultado = 0,
                    message = ex.Message
                },
                        JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Admin, Voluntario_N1")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirEncaminhamento(int EncaminhamentoId)
        {
            var resultado = false;
            var mensagem = "";

            try
            {
                var encaminhamento = encaminhamentoDao.Buscar(EncaminhamentoId);
                if (encaminhamento != null)
                {
                    encaminhamentoDao.Excluir(encaminhamento);
                    resultado = true;
                    mensagem = "O encaminhamento foi excluído com sucesso!";
                }


            }
            catch (Exception ex)
            {
                mensagem = "Ocorreu um erro ao excluir.";
                Console.WriteLine(ex);
            }
            return Json(new
            {
                Resultado = resultado,
                Mensagem = mensagem
            },
                    JsonRequestBehavior.AllowGet);
        }
    }
}