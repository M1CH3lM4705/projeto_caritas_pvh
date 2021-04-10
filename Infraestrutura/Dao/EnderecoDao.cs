using ProjetoBetaAutenticacao.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProjetoBetaAutenticacao.Infraestrutura.Dao
{
    public class EnderecoDao : DaoBase
    {
        public void Salvar(Endereco endereco)
        {
            _context.Enderecos.Add(endereco);
            _context.SaveChanges();

        }

        public Endereco Buscar(int enderecoId)
        {
            var buscarEndereco = _context.Enderecos
                .Where(p => p.EnderecoId == enderecoId).FirstOrDefault();
            return buscarEndereco;
        }

        public Endereco BuscarId(int? id)
        {
            return _context.Enderecos.Find(id);
        }

        public IEnumerable<Endereco> Listar()
        {
            var list = _context.Enderecos.ToList();

            return list;
        }

        public IEnumerable<Endereco> ListarQuery()
        {
            var list = _context.Enderecos
                .Include(e => e.PessoaCarente)
                .AsNoTracking()
                .ToList();

            return list;
        }

        public void Alterar(Endereco endereco)
        {
            _context.Entry(endereco).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Excluir(Endereco endereco)
        {
            _context.Enderecos.Remove(endereco);
            _context.SaveChanges();
        }
    }
}