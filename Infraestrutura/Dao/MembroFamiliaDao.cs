using ProjetoBetaAutenticacao.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProjetoBetaAutenticacao.Infraestrutura.Dao
{
    public class MembroFamiliaDao : DaoBase
    {
        public void Salvar(MembroFamilia membroFamilia)
        {
            _context.MembroFamilias.Add(membroFamilia);
            _context.SaveChanges();
        }

        public void Alterar(MembroFamilia membro)
        {
            _context.Entry(membro).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}