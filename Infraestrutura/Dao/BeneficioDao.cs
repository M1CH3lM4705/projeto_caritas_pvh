using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ProjetoBetaAutenticacao.Models;

namespace ProjetoBetaAutenticacao.Infraestrutura.Dao
{
    public class BeneficioDao : DaoBase
    {
        public void Salvar(Beneficio beneficio)
        {
            //Contato contato = new Contato();
            //Beneficio beneficio = new Beneficio();
            //PerfilSocioEconomico perfil = new PerfilSocioEconomico();
            //Endereco endereco = new Endereco();

            //contato.PessoaCarenteId = pessoaCarente.PessoaCarenteId;
            //beneficio.PessoaCarenteId = pessoaCarente.PessoaCarenteId;
            //perfil.PessoaCarenteId = pessoaCarente.PessoaCarenteId;
            //endereco.PessoaCarenteId = pessoaCarente.PessoaCarenteId;


            _context.Beneficios.Add(beneficio);
            _context.SaveChanges();

        }

        public Beneficio Buscar(int beneficioId)
        {
            var buscarBeneficio = _context.Beneficios
                .Where(p => p.BeneficioId == beneficioId).FirstOrDefault();
            return buscarBeneficio;
        }

        public Beneficio BuscarId(int? id)
        {
            return _context.Beneficios.Find(id);
        }

        public IEnumerable<Beneficio> Listar()
        {
            var list = _context.Beneficios.ToList();

            return list;
        }

        public IEnumerable<Beneficio> ListarQuery()
        {
            var list = _context.Beneficios
                .Include(b => b.PessoaCarente)
                .ToList();

            return list;
        }

        public void Alterar(Beneficio beneficio)
        {
            _context.Entry(beneficio).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Excluir(Beneficio beneficio)
        {
            _context.Beneficios.Remove(beneficio);
            _context.SaveChanges();
        }
    }
}