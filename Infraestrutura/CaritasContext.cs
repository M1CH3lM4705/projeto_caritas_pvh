using ProjetoBetaAutenticacao.Areas.Admin.Data;
using ProjetoBetaAutenticacao.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;

namespace ProjetoBetaAutenticacao.Infraestrutura
{
    public class CaritasContext : DbContext
    {
        public CaritasContext() : base("DefaultContext")
        {

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        //public override int SaveChanges()
        //{
        //    try
        //    {
        //        return base.SaveChanges();
        //    }
        //    catch (DbEntityValidationException e)
        //    {

        //        var newException = new FormattedDbEntityValidationException(e);
        //        throw newException;
        //    }
            
        //}
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

           
            //modelBuilder.Entity<Beneficio>()
            //    .HasKey(b => b.PessoaCarenteId);
            modelBuilder.Entity<PessoaCarente>()
                .HasRequired(p => p.Beneficio)
                .WithRequiredPrincipal(b => b.PessoaCarente)
                .WillCascadeOnDelete(true);


            //modelBuilder.Entity<Contato>()
            //    .HasKey(c => c.PessoaCarenteId);
            modelBuilder.Entity<PessoaCarente>()
                .HasRequired(p => p.Contato)
                .WithRequiredPrincipal(c => c.PessoaCarente)
                .WillCascadeOnDelete(true);

            //modelBuilder.Entity<Endereco>()
            //    .HasKey(e => e.PessoaCarenteId);
            modelBuilder.Entity<PessoaCarente>()
                .HasRequired(p => p.Endereco)
                .WithRequiredPrincipal(e => e.PessoaCarente)
                .WillCascadeOnDelete(true);


            //modelBuilder.Entity<PerfilSocioEconomico>()
            //    .HasKey(p => p.PessoaCarenteId);
            modelBuilder.Entity<PessoaCarente>()
                .HasRequired(p => p.PerfilEconomico)
                .WithRequiredPrincipal(p => p.PessoaCarente)
                .WillCascadeOnDelete(true);

            //modelBuilder.Entity<PessoaCarente>()
            //    .HasOptional(x => x.Voluntario)
            //    .WithMany(p => p.PessoasCarentes)
            //    .HasForeignKey(p => p.VoluntarioId);

            modelBuilder.Entity<MembroFamilia>()
                .HasOptional(p => p.PessoaCarente)
                .WithMany(m => m.MembroFamilia)
                .HasForeignKey(m => m.PessoaCarenteId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Encaminhamento>()
                .HasOptional(p => p.PessoaCarente)
                .WithMany(m => m.Encaminhamentos)
                .HasForeignKey(m => m.PessoaCarenteId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<PessoaCarente>()
                .HasRequired(p => p.Voluntario)
                .WithMany(m => m.PessoasCarentes)
                .HasForeignKey(m => m.VoluntarioId)
                .WillCascadeOnDelete(false);

        }

        public virtual DbSet<Voluntario> Voluntarios { get; set; }
        public virtual DbSet<Paroquia> Paroquias { get; set; }
        public virtual DbSet<PessoaCarente> PessoasCarentes { get; set; }
        public virtual DbSet<Contato> Contatos { get; set; }
        public virtual DbSet<Endereco> Enderecos { get; set; }
        public virtual DbSet<MembroFamilia> MembroFamilias { get; set; }
        public virtual DbSet<PerfilSocioEconomico> PerfilSocioEconomicos { get; set; }
        public virtual DbSet<Beneficio> Beneficios { get; set; }
        public virtual DbSet<Encaminhamento> Encaminhamentos { get; set; }
    }
}