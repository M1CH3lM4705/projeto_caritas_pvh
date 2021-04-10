using ProjetoBetaAutenticacao.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ProjetoBetaAutenticacao.Infraestrutura.Dao
{
    public class EncaminhamentoDao : DaoBase
    {
        public void Salvar(Encaminhamento encaminhamento)
        {
            _context.Encaminhamentos.Add(encaminhamento);
            _context.SaveChanges();

        }

        public Encaminhamento Buscar(int? encId)
        {
             var buscarEncaminhamento =  _context.Encaminhamentos
                .Where(e => e.EncaminhamentoId == encId).FirstOrDefault();
            return buscarEncaminhamento;
        }

        public Encaminhamento BuscarId(int? id)
        {
            var enc = _context.Encaminhamentos
                .Include(p => p.PessoaCarente)
                .Where(e => e.PessoaCarente.PessoaCarenteId == id)
                .AsNoTracking()
                .FirstOrDefault();
            return enc;
        }

        public DateTime DataMaxima(int? id)
        {
            var encaminhamento = _context.Encaminhamentos
                .Where(e => e.PessoaCarenteId == id && e.Status == true)
                .Max(e => e.DataEntrega);
            return Convert.ToDateTime(encaminhamento);
        }
        public IEnumerable<Encaminhamento> ListaEncaminhar(int id)
        {
            var lista = _context.Encaminhamentos
                .Where(e => e.PessoaCarente.PessoaCarenteId == id)
                .OrderByDescending(e => e.EncaminhamentoId)
                .ToList();
            return lista;
        }
        public IEnumerable<Encaminhamento> Listar()
        {
            var list = _context.Encaminhamentos.ToList();

            return list;
        }

        public IEnumerable<Encaminhamento> ListarQuery()
        {
            var list = _context.Encaminhamentos
                .Include(e => e.PessoaCarente)
                .AsNoTracking()
                .ToList();

            return list;
        }

        public void Alterar(Encaminhamento encaminhamento)
        {
            _context.Entry(encaminhamento).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Excluir(Encaminhamento encaminhamento)
        {
            _context.Encaminhamentos.Remove(encaminhamento);
            _context.SaveChanges();
        }
    }
}