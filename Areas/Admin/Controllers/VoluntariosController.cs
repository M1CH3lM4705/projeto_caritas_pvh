using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;
using ProjetoBetaAutenticacao.Areas.Admin.Data;
using ProjetoBetaAutenticacao.Infraestrutura;
using ProjetoBetaAutenticacao.Models;
using ProjetoBetaAutenticacao.Models.Filters;
using ProjetoBetaAutenticacao.Utilidades;

namespace ProjetoBetaAutenticacao.Areas.Admin.Controllers
{
    [AccessDeniedAuthorize(Roles = "Admin")]
    public class VoluntariosController : Controller
    {
        private readonly CaritasContext db = new CaritasContext();
        private readonly ApplicationDbContext context = new ApplicationDbContext();

        // GET: Admin/Voluntarios
        public ActionResult Index(string pesquisa, int? pagina, int? tamanhoPag)
        {
            var voluntario = db.Voluntarios
                .Include(v => v.Paroquia)
                .OrderByDescending(p => p.ParoquiaId);
            int numeroRegistros = (tamanhoPag ?? 5);
            int numeroPagina = (pagina ?? 1);
            ViewBag.ListaTamanhoPag = new SelectList(new int[] { numeroRegistros, 10, 15, 20 }, numeroRegistros);
            ViewBag.Pesquia = pesquisa;
            ViewBag.Pagina = pagina;
            if (!string.IsNullOrEmpty(pesquisa))
            {
                voluntario = db.Voluntarios.Where(p => p.Nome.Contains(pesquisa)).OrderBy(p => p.Nome);
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_ListaVoluntario", voluntario.ToPagedList(numeroPagina, numeroRegistros));

            }
            return View(voluntario.ToPagedList(numeroPagina, numeroRegistros));

        }

        // GET: Admin/Voluntarios/Details/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Detalhes(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Voluntario voluntario = db.Voluntarios.Find(id);
            if (voluntario == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Detalhes",voluntario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FormVoluntario(int? id)
        {
            Voluntario voluntario = new Voluntario();
            var paroquia = db.Paroquias.ToList();
            ViewBag.ParoquiaId = new SelectList(paroquia, "ParoquiaId", "Nome");

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var list = roleManager.Roles.ToList();
            //list.Add(new IdentityRole {  Name = "Selecione um perfil!" });
            //list = list.OrderBy(c => c.Name).ToList();
            ViewBag.Roles = list;
            if (id != 0)
            {
                voluntario = db.Voluntarios
                    .Find(id);
                if (voluntario == null)
                {
                    return HttpNotFound();
                }

                ViewBag.ParoquiaId = new SelectList(paroquia, "ParoquiaId", "Nome", voluntario.ParoquiaId);

                //var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                list = context.Roles.ToList();

                ViewBag.Roles = new SelectList(list, "Name", "Name", voluntario.Role);
            }
            return PartialView("_Create", voluntario);
        }

        // POST: Admin/Voluntarios/Create
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SalvarVoluntario([Bind(Include = "VoluntarioId,UserName,Nome,SobreNome,ParoquiaId,Role")] Voluntario voluntario)
        {
            var resultado = "OK";
            var mensagens = new List<string>();
            var idSalvo = string.Empty;
            var cidade = string.Empty;
            if (db.Voluntarios.Any(p => p.UserName == voluntario.UserName))
            {
                ModelState.AddModelError("UserName", $"{voluntario.UserName} Ja cadastrado!");
            }


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
                    var existeVol = db.Voluntarios
                        .Where(v => v.VoluntarioId == voluntario.VoluntarioId).
                        AsNoTracking()
                        .FirstOrDefault();

                    if (existeVol == null)
                    {
                        existeVol = voluntario;
                        db.Voluntarios.Add(existeVol);
                        var par = db.Paroquias.Find(existeVol.ParoquiaId);
                        var idCidade = par.Cidade;
                        Municipio municipio = new Municipio();
                        cidade = municipio.BuscarCidade(idCidade).Nome;
                        Utils.CreateUser(existeVol.UserName, par.Nome, cidade);
                        Utils.AddRoleToUser(existeVol.UserName, existeVol.Role);
                    }
                    else
                    {
                        existeVol.Nome = voluntario.Nome;
                        existeVol.UserName = voluntario.UserName;
                        existeVol.SobreNome = voluntario.SobreNome;
                        existeVol.ParoquiaId = voluntario.ParoquiaId;
                        existeVol.Role = voluntario.Role;
                        
                        var db2 = new CaritasContext();
                        var oldUser = db2.Voluntarios.Find(voluntario.VoluntarioId);
                        var par = db2.Paroquias.Find(voluntario.ParoquiaId);
                        //oldUser.Paroquia = par;

                        db2.Dispose();
                        db.Entry(existeVol).State = EntityState.Modified;
                        try
                        {
                            if (oldUser != null && oldUser.UserName != voluntario.UserName)
                            {
                                //Metodo que altera os campos username na tabela aspNetUsers
                                Utils.ChangeUserName(oldUser.UserName, voluntario.UserName);
                            }
                            if (oldUser != null && oldUser.UserName != voluntario.UserName && oldUser.ParoquiaId != voluntario.ParoquiaId)
                            {
                                //Verifica se usuario é diferente e, se paroquia também, para então alterar o nome da paroquia na tabela aspNetUsers
                                Utils.ChangeUserParoquia(voluntario.UserName, par.Nome, cidade);
                            }
                            if (oldUser != null && oldUser.UserName == voluntario.UserName && oldUser.ParoquiaId != voluntario.ParoquiaId)
                            {
                                //Verifica se usuario é igual e, se paroquia e diferente, para então alterar o nome da paroquia na tabela aspNetUsers
                                Utils.ChangeUserParoquia(oldUser.UserName, par.Nome, par.Cidade);
                            }
                            if (oldUser != null && oldUser != null && oldUser.UserName != voluntario.UserName && oldUser.Role != voluntario.Role)
                            {
                                //Verifica se usuario é diferente e, se a Role e diferente, para então alterar o nome da Role na tabela aspNetUsersRoles
                                Utils.UpdateRoleToUser(voluntario.UserName, oldUser.Role, voluntario.Role);
                            }
                            if (oldUser != null && oldUser.UserName == voluntario.UserName && oldUser.Role != voluntario.Role)
                            {
                                Utils.UpdateRoleToUser(oldUser.UserName, oldUser.Role, voluntario.Role);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                    }
                    db.SaveChanges();
                    idSalvo = existeVol.VoluntarioId.ToString();
                }
                catch
                {
                    resultado = "ERRO";
                }
            }
            return Json(new { Resultado = resultado, Mensagens = mensagens, IdSalvo = idSalvo});
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VoluntarioId,UserName,Nome,SobreNome,ParoquiaId,Paroquia,Role")] Voluntario voluntario)
        {
            if (ModelState.IsValid)
            {
                var db2 = new CaritasContext();
                var oldUser = db2.Voluntarios.Find(voluntario.VoluntarioId);
                var par = db2.Paroquias.Find(voluntario.ParoquiaId);
                
                //oldUser.Paroquia = par;

                db2.Dispose();
                db.Entry(voluntario).State = EntityState.Modified;

                try
                {
                    if (oldUser != null && oldUser.UserName != voluntario.UserName)
                    {
                        //Metodo que altera os campos username na tabela aspNetUsers
                        Utils.ChangeUserName(oldUser.UserName, voluntario.UserName);
                    }
                    if (oldUser != null && oldUser.UserName != voluntario.UserName && oldUser.ParoquiaId != voluntario.ParoquiaId)
                    {
                        //Verifica se usuario é diferente e, se paroquia também, para então alterar o nome da paroquia na tabela aspNetUsers
                        Utils.ChangeUserParoquia(voluntario.UserName, par.Nome, par.Cidade);
                    }
                    if (oldUser != null && oldUser.UserName == voluntario.UserName && oldUser.ParoquiaId != voluntario.ParoquiaId)
                    {
                        //Verifica se usuario é igual e, se paroquia e diferente, para então alterar o nome da paroquia na tabela aspNetUsers
                        Utils.ChangeUserParoquia(oldUser.UserName, par.Nome, par.Cidade);
                    }
                    if (oldUser != null && oldUser != null && oldUser.UserName != voluntario.UserName && oldUser.Role != voluntario.Role)
                    {
                        //Verifica se usuario é diferente e, se a Role e diferente, para então alterar o nome da Role na tabela aspNetUsersRoles
                        Utils.UpdateRoleToUser(voluntario.UserName, oldUser.Role, voluntario.Role);
                    }
                    if (oldUser != null && oldUser.UserName == voluntario.UserName && oldUser.Role != voluntario.Role)
                    {
                        Utils.UpdateRoleToUser(oldUser.UserName, oldUser.Role, voluntario.Role);
                    }
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);

                }

                return RedirectToAction("Index");
            }
            var paroquia = db.Paroquias.ToList();
            ViewBag.ParoquiaId = paroquia;

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var list = roleManager.Roles.ToList();
            ViewBag.Roles = list;
            return View(voluntario);

        }

        // GET: Admin/Voluntarios/Delete/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletarVoluntario(int voluntarioId)
        {
            bool result = false;
            var mensagem = "";
            Voluntario voluntario = db.Voluntarios.Where(v => v.VoluntarioId == voluntarioId).First();
            if (voluntario != null)
            {
                try
                {
                    Utils.AtivarDesativarUser(voluntario.UserName);
                    if (voluntario.Ativo == true)
                    {
                        voluntario.Ativo = false;
                        mensagem = "Agente Caritas Desativado com sucesso!";
                    }
                    else
                    {
                        voluntario.Ativo = true;
                        mensagem = "Agente Caritas Ativado com sucesso!";
                    }
                    db.Voluntarios.Attach(voluntario);
                    db.Entry(voluntario).Property(v => v.Ativo).IsModified = true;
                    db.SaveChanges();
                    result = true; 
                    
                }
                catch (Exception)
                {
                    mensagem = "Ocorreu um erro ao Desativar.";
                }
            }
            return Json(new { Resultado = result, Mensagem = mensagem }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AlterarSenha(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Voluntario voluntario = db.Voluntarios.Find(id);
            VoluntarioSenha vSenha = new VoluntarioSenha
            {
                VoluntarioId = voluntario.VoluntarioId
            };


            if (vSenha == null)
            {
                return HttpNotFound();
            }
            return View(vSenha);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AlterarSenha(int id, string senha)
        {
            var userLocal = db.Voluntarios.Where(v => v.VoluntarioId == id).FirstOrDefault();
            var userAsp = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var user = userAsp.FindByName(userLocal.UserName);
            if (senha == string.Empty)
            {
                ModelState.AddModelError("", $"Está senha ja esta cadastrada, por favor insira outra.");
            }
            if (ModelState.IsValid)
            {
                Utils.ChangePassword(user.UserName, senha);
                return RedirectToAction("Index");
            }
            return View(id);
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
