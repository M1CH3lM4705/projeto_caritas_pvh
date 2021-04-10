using ProjetoBetaAutenticacao.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProjetoBetaAutenticacao.Infraestrutura.Dao
{
    public class PerfilSocioEconomicoDao : DaoBase
    {
        public void Salvar(PerfilSocioEconomico perfil)
        {
            _context.PerfilSocioEconomicos.Add(perfil);
            _context.SaveChanges();

        }

        public PerfilSocioEconomico Buscar(int perfilId)
        {
            var buscarPerfil = _context.PerfilSocioEconomicos
                .Where(p => p.PSocioEcoId == perfilId).FirstOrDefault();
            return buscarPerfil;
        }

        public PerfilSocioEconomico BuscarId(int? id)
        {
            return _context.PerfilSocioEconomicos.Find(id);
        }

        public IEnumerable<PerfilSocioEconomico> Listar()
        {
            var list = _context.PerfilSocioEconomicos.ToList();

            return list;
        }

        public IEnumerable<PerfilSocioEconomico> ListarQuery()
        {
            var list = _context.PerfilSocioEconomicos
                .Include(p => p.PessoaCarente)
                .ToList();

            return list;
        }

        public void Alterar(PerfilSocioEconomico perfil)
        {
            _context.Entry(perfil).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Excluir(PerfilSocioEconomico perfil)
        {
            _context.PerfilSocioEconomicos.Remove(perfil);
            _context.SaveChanges();
        }
    }
}