using ProjetoBetaAutenticacao.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProjetoBetaAutenticacao.Infraestrutura.Dao
{
    public class PessoaCarenteDao : DaoBase
    {

        public void Salvar(PessoaCarente pessoaCarente)
        {
            _context.PessoasCarentes.Add(pessoaCarente);
            _context.SaveChanges();
        }

        public PessoaCarente Buscar(int pessoaCarenteId)
        {
            var buscarCarente = _context.PessoasCarentes
                .Include(p => p.Endereco)
                .Include(p => p.Contato)
                .Include(p => p.Beneficio)
                .Include(p => p.MembroFamilia)
                .Include(p => p.PerfilEconomico)
                .Include(p => p.Voluntario.Paroquia)
                .Include(p => p.Encaminhamentos)
                .Where(p => p.PessoaCarenteId == pessoaCarenteId)
                .AsNoTracking()
                .FirstOrDefault();
            return buscarCarente;
        }

        public PessoaCarente BuscarId(int? id)
        {
            var pessoa =  _context.PessoasCarentes
                .Include(e => e.Encaminhamentos)
                .Where(p => p.PessoaCarenteId == id)
                .FirstOrDefault();
            return pessoa;
        }

        public IEnumerable<PessoaCarente> BuscarPessoas(string pessoa)
        {
            pessoa.ToLower();
            var lista = _context.PessoasCarentes
                .Where(p => p.Nome.ToLower().Contains(pessoa) || pessoa == null)
                .ToList();

            return lista;
        }

        public IEnumerable<PessoaCarente> Listar()
        {
            var list = _context.PessoasCarentes.ToList();

            return list;
        }
        public bool BuscarCpf(string cpf)
        {
            var Cpf = _context.PessoasCarentes
                .Where(p => p.Cpf.Contains(cpf))
                .FirstOrDefault();

            if (Cpf.Cpf.Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public IEnumerable<PessoaCarente> ListarQuery()
        {
            var list = _context.PessoasCarentes
                //.Include(p => p.Endereco)
                //.Include(p => p.Contato)
                //.Include(p => p.Beneficio)
                //.Include(p => p.MembroFamilia)
                //.Include(p => p.PerfilEconomico)
                //.Include(p => p.Encaminhamentos)
                //.Include(p => p.Voluntario.Paroquia)
                .ToList();

            return list;
        }
        public IEnumerable<PessoaCarente> ListaOrdenada(string ordem)
        {
            var pessoas = from s in _context.PessoasCarentes select s;
            switch (ordem)
            {
                case "desc":
                    pessoas = pessoas.OrderByDescending(s => s.Nome);
                    break;
                default:
                    pessoas = pessoas.OrderBy(s => s.Nome);
                    break;
            }
            return pessoas.ToList();
        }
        public void Alterar(PessoaCarente pessoaCarente)
        {
            _context.PessoasCarentes.Attach(pessoaCarente);
            _context.Entry(pessoaCarente).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Excluir(PessoaCarente pessoaCarente)
        {
            _context.PessoasCarentes.Attach(pessoaCarente);
            _context.PessoasCarentes.Remove(pessoaCarente);
            _context.SaveChanges();
        }

    }
}