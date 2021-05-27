namespace ProjetoBetaAutenticacao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InicialBase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Beneficios",
                c => new
                    {
                        BeneficioId = c.Int(nullable: false),
                        SimNao = c.Boolean(nullable: false),
                        TipoBeneficio = c.String(),
                    })
                .PrimaryKey(t => t.BeneficioId)
                .ForeignKey("dbo.PessoaCarente", t => t.BeneficioId, cascadeDelete: true)
                .Index(t => t.BeneficioId);
            
            CreateTable(
                "dbo.PessoaCarente",
                c => new
                    {
                        PessoaCarenteId = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 200),
                        Cpf = c.String(nullable: false),
                        Rg = c.String(maxLength: 14),
                        DataNascimento = c.DateTime(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                        ProtocoloRefugio = c.String(),
                        Genero = c.String(),
                        EstadoCivil = c.Int(nullable: false),
                        Nacionalidade = c.String(maxLength: 50),
                        Profissao = c.String(maxLength: 50),
                        Renda = c.String(),
                        Religiao = c.String(maxLength: 50),
                        VoluntarioId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.PessoaCarenteId)
                .ForeignKey("dbo.Voluntarios", t => t.VoluntarioId)
                .Index(t => t.VoluntarioId);
            
            CreateTable(
                "dbo.Contato",
                c => new
                    {
                        ContatoId = c.Int(nullable: false),
                        Celular = c.String(),
                        WhatsApp = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ContatoId)
                .ForeignKey("dbo.PessoaCarente", t => t.ContatoId, cascadeDelete: true)
                .Index(t => t.ContatoId);
            
            CreateTable(
                "dbo.Encaminhamento",
                c => new
                    {
                        EncaminhamentoId = c.Int(nullable: false, identity: true),
                        TipoEncaminhamento = c.String(nullable: false),
                        Status = c.Boolean(nullable: false),
                        DataSolicitacao = c.DateTime(nullable: false),
                        DataEntrega = c.DateTime(),
                        PessoaCarenteId = c.Int(),
                    })
                .PrimaryKey(t => t.EncaminhamentoId)
                .ForeignKey("dbo.PessoaCarente", t => t.PessoaCarenteId, cascadeDelete: true)
                .Index(t => t.PessoaCarenteId);
            
            CreateTable(
                "dbo.Endereco",
                c => new
                    {
                        EnderecoId = c.Int(nullable: false),
                        Rua = c.String(),
                        Cep = c.String(),
                        Bairro = c.String(),
                        Numero = c.String(),
                    })
                .PrimaryKey(t => t.EnderecoId)
                .ForeignKey("dbo.PessoaCarente", t => t.EnderecoId, cascadeDelete: true)
                .Index(t => t.EnderecoId);
            
            CreateTable(
                "dbo.MembrosFamiliar",
                c => new
                    {
                        ParenteId = c.Int(nullable: false, identity: true),
                        Nome = c.String(maxLength: 200),
                        Idade = c.String(maxLength: 2),
                        Cpf = c.String(),
                        Rg = c.String(maxLength: 14),
                        Parentesco = c.String(maxLength: 50),
                        PessoaCarenteId = c.Int(),
                    })
                .PrimaryKey(t => t.ParenteId)
                .ForeignKey("dbo.PessoaCarente", t => t.PessoaCarenteId, cascadeDelete: true)
                .Index(t => t.PessoaCarenteId);
            
            CreateTable(
                "dbo.PerfilSocioEconomico",
                c => new
                    {
                        PSocioEcoId = c.Int(nullable: false),
                        Escolaridade = c.Int(nullable: false),
                        OcupacaoAtual = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PSocioEcoId)
                .ForeignKey("dbo.PessoaCarente", t => t.PSocioEcoId, cascadeDelete: true)
                .Index(t => t.PSocioEcoId);
            
            CreateTable(
                "dbo.Voluntarios",
                c => new
                    {
                        VoluntarioId = c.Long(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 100),
                        Nome = c.String(nullable: false),
                        SobreNome = c.String(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                        ParoquiaId = c.Long(nullable: false),
                        Role = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.VoluntarioId)
                .ForeignKey("dbo.Paroquias", t => t.ParoquiaId, cascadeDelete: true)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.ParoquiaId);
            
            CreateTable(
                "dbo.Paroquias",
                c => new
                    {
                        ParoquiaId = c.Long(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false),
                        Endereco = c.String(nullable: false),
                        Estado = c.Int(nullable: false),
                        Cidade = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => t.ParoquiaId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PessoaCarente", "VoluntarioId", "dbo.Voluntarios");
            DropForeignKey("dbo.Voluntarios", "ParoquiaId", "dbo.Paroquias");
            DropForeignKey("dbo.PerfilSocioEconomico", "PSocioEcoId", "dbo.PessoaCarente");
            DropForeignKey("dbo.MembrosFamiliar", "PessoaCarenteId", "dbo.PessoaCarente");
            DropForeignKey("dbo.Endereco", "EnderecoId", "dbo.PessoaCarente");
            DropForeignKey("dbo.Encaminhamento", "PessoaCarenteId", "dbo.PessoaCarente");
            DropForeignKey("dbo.Contato", "ContatoId", "dbo.PessoaCarente");
            DropForeignKey("dbo.Beneficios", "BeneficioId", "dbo.PessoaCarente");
            DropIndex("dbo.Voluntarios", new[] { "ParoquiaId" });
            DropIndex("dbo.Voluntarios", "UserNameIndex");
            DropIndex("dbo.PerfilSocioEconomico", new[] { "PSocioEcoId" });
            DropIndex("dbo.MembrosFamiliar", new[] { "PessoaCarenteId" });
            DropIndex("dbo.Endereco", new[] { "EnderecoId" });
            DropIndex("dbo.Encaminhamento", new[] { "PessoaCarenteId" });
            DropIndex("dbo.Contato", new[] { "ContatoId" });
            DropIndex("dbo.PessoaCarente", new[] { "VoluntarioId" });
            DropIndex("dbo.Beneficios", new[] { "BeneficioId" });
            DropTable("dbo.Paroquias");
            DropTable("dbo.Voluntarios");
            DropTable("dbo.PerfilSocioEconomico");
            DropTable("dbo.MembrosFamiliar");
            DropTable("dbo.Endereco");
            DropTable("dbo.Encaminhamento");
            DropTable("dbo.Contato");
            DropTable("dbo.PessoaCarente");
            DropTable("dbo.Beneficios");
        }
    }
}
