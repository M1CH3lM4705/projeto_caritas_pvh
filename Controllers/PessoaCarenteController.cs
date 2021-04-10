using AplicacaoEscola.Helpers;
using Microsoft.AspNet.Identity;
using PagedList;
using ProjetoBetaAutenticacao.Infraestrutura;
using ProjetoBetaAutenticacao.Infraestrutura.Dao;
using ProjetoBetaAutenticacao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ProjetoBetaAutenticacao.Controllers
{

    public class PessoaCarenteController : Controller
    {
        private readonly PessoaCarenteDao pCarenteDao = new PessoaCarenteDao();
        private readonly BeneficioDao beneficioDao = new BeneficioDao();
        private readonly ContatoDao contatoDao = new ContatoDao();
        private readonly EnderecoDao enderecoDao = new EnderecoDao();
        private readonly PerfilSocioEconomicoDao perfilSocio = new PerfilSocioEconomicoDao();
        private readonly MembroFamiliaDao mbDao = new MembroFamiliaDao();
        readonly CaritasContext db = new CaritasContext();


        [Authorize(Roles = "Admin, Voluntario_N1, Voluntario_N2")]
        // GET: PessoaCarente
        public ActionResult Index(string pesquisa, int? pagina, int? tamanhoPag)
        {
            var pessoa = pCarenteDao.ListarQuery().OrderByDescending(p => p.PessoaCarenteId);
            int numeroRegistros = (tamanhoPag ?? 5);
            int numeroPagina = (pagina ?? 1);
            ViewBag.ListaTamanhoPag = new SelectList(new int[] { numeroRegistros, 10, 15, 20 }, numeroRegistros);
            ViewBag.TamanhoPagina = numeroRegistros;
            ViewBag.Pesquisa = pesquisa;
            ViewBag.Pagina = pagina;
            //ViewBag.Ordem = ordem;
            if (!string.IsNullOrEmpty(pesquisa))
            {
                pessoa = pCarenteDao.BuscarPessoas(pesquisa).OrderBy(p => p.Nome);
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_ListaPessoas", pessoa.ToPagedList(numeroPagina, numeroRegistros));
            }
            return View(pessoa.ToPagedList(numeroPagina, numeroRegistros));
        }      


        [Authorize(Roles = "Admin, Voluntario_N1, Voluntario_N2")]

        public ActionResult DetalhesPesssoa(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var pessoa = pCarenteDao.BuscarId(id);
            if (pessoa == null)
            {
                return HttpNotFound();
            }
            return View(pessoa);
        }

        
        [HttpPost]
        [Authorize(Roles = "Admin, Voluntario_N1")]
        [ValidateAntiForgeryToken]
        public ActionResult PessoaNovo(string name)
        {
            if (name == null || name != User.Identity.GetUserName())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var voluntario = db.Voluntarios.Where(v => v.UserName == name).FirstOrDefault();
            if (voluntario == null)
            {
                return HttpNotFound();
            }
            var pessoa = new PessoaCarente
            {
                MembroFamilia = new List<MembroFamilia>
                {
                    new MembroFamilia()
                }
            };
            pessoa.VoluntarioId = voluntario.VoluntarioId;

            return PartialView("_CadastroPessoa", pessoa);


        }

        [HttpPost]
        [Authorize(Roles = "Admin, Voluntario_N1")]
        [ValidateAntiForgeryToken]
        public ActionResult AlterarPessoa(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var pessoa = pCarenteDao.BuscarId(id);
            if (pessoa == null)
            {
                return HttpNotFound();
            }
            var pCarente = pCarenteDao.Buscar(pessoa.PessoaCarenteId);


            return PartialView("_CadastroPessoa", pCarente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Voluntario_N1")]
        public ActionResult SalvarPessoa(PessoaCarente pessoaCarente)
        {
            //var cpf = pCarenteDao.BuscarCpf(pessoaCarente.Cpf);
            //if (cpf)
            //{
            //    ModelState.AddModelError("", $"Cpf Já Cadastrado");
            //}

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
                    var existePessoa = pCarenteDao.Buscar(pessoaCarente.PessoaCarenteId);

                    if (existePessoa == null)
                    {
                        existePessoa = pessoaCarente;
                        pCarenteDao.Salvar(existePessoa);
                    }
                    else
                    {
                        existePessoa.Nome = pessoaCarente.Nome;
                        existePessoa.Nacionalidade = pessoaCarente.Nacionalidade;
                        existePessoa.DataNascimento = pessoaCarente.DataNascimento;
                        existePessoa.EstadoCivil = pessoaCarente.EstadoCivil;
                        existePessoa.Cpf = pessoaCarente.Cpf;
                        existePessoa.Rg = pessoaCarente.Rg;
                        existePessoa.Religiao = pessoaCarente.Religiao;
                        existePessoa.Renda = pessoaCarente.Renda;
                        existePessoa.Profissao = pessoaCarente.Profissao;
                        existePessoa.Genero = pessoaCarente.Genero;
                        existePessoa.ProtocoloRefugio = pessoaCarente.ProtocoloRefugio;
                        existePessoa.VoluntarioId = pessoaCarente.VoluntarioId;

                        var beneficio = new Beneficio
                        {
                            TipoBeneficio = pessoaCarente.Beneficio.TipoBeneficio,
                            SimNao = pessoaCarente.Beneficio.SimNao,
                            BeneficioId = pessoaCarente.PessoaCarenteId
                        };

                        var perfil = new PerfilSocioEconomico
                        {
                            Escolaridade = pessoaCarente.PerfilEconomico.Escolaridade,
                            OcupacaoAtual = pessoaCarente.PerfilEconomico.OcupacaoAtual,

                            PSocioEcoId = pessoaCarente.PessoaCarenteId
                        };
                        var contato = new Contato
                        {
                            Celular = pessoaCarente.Contato.Celular,
                            WhatsApp = pessoaCarente.Contato.WhatsApp,

                            ContatoId = pessoaCarente.PessoaCarenteId
                        };
                        var endereco = new Endereco
                        {
                            Cep = pessoaCarente.Endereco.Cep,
                            Rua = pessoaCarente.Endereco.Rua,
                            Numero = pessoaCarente.Endereco.Numero,
                            Bairro = pessoaCarente.Endereco.Bairro,
                            EnderecoId = pessoaCarente.PessoaCarenteId
                        };

                        foreach (var item in pessoaCarente.MembroFamilia)
                        {

                            var membro = new MembroFamilia
                            {
                                ParenteId = item.ParenteId,
                                Nome = item.Nome,
                                Cpf = item.Cpf,
                                Rg = item.Rg,
                                Idade = item.Idade,
                                Parentesco = item.Parentesco,
                                PessoaCarenteId = item.PessoaCarenteId
                            };
                            if (item.ParenteId != 0)
                            {
                                mbDao.Alterar(membro);
                            }
                            else
                            {
                                membro.PessoaCarenteId = pessoaCarente.PessoaCarenteId;
                                mbDao.Salvar(membro);
                            }

                        }
                        //existePessoa.Beneficio = beneficio;
                        //existePessoa.PerfilEconomico = perfil;
                        //existePessoa.Contato = contato;
                        //existePessoa.Endereco = endereco;
                        pCarenteDao.Alterar(existePessoa);
                        beneficioDao.Alterar(beneficio);
                        contatoDao.Alterar(contato);
                        enderecoDao.Alterar(endereco);
                        perfilSocio.Alterar(perfil);

                    }
                    idSalvo = existePessoa.PessoaCarenteId.ToString();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    resultado = "Erro";

                }

            }
            return Json(new { Resultado = resultado, IdSalvo = idSalvo, Mensagens = mensagens }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        [Authorize(Roles = "Admin, Voluntario_N1")]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirPessoa(int pessoaId)
        {

            var resultado = false;
            var mensagem = "";
            try
            {
                var pessoa = pCarenteDao.Buscar(pessoaId);
                if (pessoa != null)
                {
                    pCarenteDao.Excluir(pessoa);
                    resultado = true;
                    mensagem = "A pessoa foi excluída com sucesso!";
                }

            }
            catch (Exception ex)
            {
                mensagem = "Ocorreu um erro ao excluir";
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