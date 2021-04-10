using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetoBetaAutenticacao.Infraestrutura.Dao
{
    public class DaoBase
    {
        protected readonly CaritasContext _context;

        public DaoBase()
        {
            _context = new CaritasContext();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}