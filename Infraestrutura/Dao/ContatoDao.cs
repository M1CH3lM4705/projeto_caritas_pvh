using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ProjetoBetaAutenticacao.Models;

namespace ProjetoBetaAutenticacao.Infraestrutura.Dao
{
    public class ContatoDao : DaoBase
    {
        public void Salvar(Contato contato)
        {
            _context.Contatos.Add(contato);
            _context.SaveChanges();

        }

        public Contato Buscar(int contatoId)
        {
            var buscarContato = _context.Contatos
                .Where(c => c.ContatoId == contatoId).FirstOrDefault();
            return buscarContato;
        }

        public Contato BuscarId(int? id)
        {
            return _context.Contatos.Where(c => c.PessoaCarente.PessoaCarenteId == id)
                .AsNoTracking()
                .FirstOrDefault();
        }

        public IEnumerable<Contato> Listar()
        {
            var list = _context.Contatos.ToList();

            return list;
        }

        public IEnumerable<Contato> ListarQuery()
        {
            var list = _context.Contatos
                .Include(c => c.PessoaCarente)
                .AsNoTracking()
                .ToList();

            return list;
        }

        public void Alterar(Contato contato)
        {
            _context.Entry(contato).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Excluir(Contato contato)
        {
            _context.Contatos.Remove(contato);
            _context.SaveChanges();
        }
    }
}