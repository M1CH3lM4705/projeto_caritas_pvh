using ProjetoBetaAutenticacao.Areas.Admin.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjetoBetaAutenticacao.Models
{
    [Table("PessoaCarente")]
    public class PessoaCarente
    {
        public PessoaCarente()
        {
            //Contato = new Contato();
            //Endereco = new Endereco();
            //Beneficio = new Beneficio();
            //PerfilEconomico = new PerfilSocioEconomico();
            MembroFamilia = new List<MembroFamilia>();
            Encaminhamentos = new List<Encaminhamento>();
            DataCadastro = DateTime.Now;
        }
        [Key]
        public int PessoaCarenteId { get; set; }
        [Required(ErrorMessage = "O campo {0} precisa ser preenchido!")]
        [StringLength(200, ErrorMessage = "O campo {0} poder to no máximo {1} caracteres")]
        [Display(Name = "Nome Completo")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O campo {0} precisa ser preenchido! ")]
        [Display(Name = "CPF")]
        public string Cpf { get; set; }
        
        [Display(Name = "RG")]
        [StringLength(14, ErrorMessage = "O campo {0} pode ter no maximo {1} caracteres!")]
        [RegularExpression(@"^[0-9]{5,14}$", ErrorMessage = "Digite somente numeros")]
        public string Rg { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        [Display(Name = "Data de Nascimento")]
        [DataType(DataType.Date, ErrorMessage = "Data em Formato inválido")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataNascimento { get; set; }

        [Display(Name = "Data do Cadastro")]
        public DateTime DataCadastro { get; set; }

        [Display(Name = "Idade")]
        public string Idade { get { return string.Format("{0}", DateTime.Now.Year - this.DataNascimento.Year); }  }

        [Display(Name = "Protocolo de Refugio")]
        public string ProtocoloRefugio { get; set; }
        [Display(Name = "Gênero")]
        public string Genero { get; set; }

        [Display(Name = "Estado Civil")]
        public EstadoCivil EstadoCivil { get; set; }

        [StringLength(50, ErrorMessage = "O campo {0} pode ter no maximo {1} caracteres!")]
        public string Nacionalidade { get; set; }

        [Display(Name = "Profissão")]
        [StringLength(50, ErrorMessage = "O campo {0} pode ter no maximo {1} caracteres!")]
        public string Profissao { get; set; }
        public string Renda { get; set; }

        [Display(Name ="Religião")]
        [StringLength(50, ErrorMessage = "O campo {0} pode ter no maximo {1} caracteres!")]
        public string Religiao { get; set; }
        public virtual Contato Contato { get; set; }
        public virtual Endereco Endereco { get; set; }
        public virtual Beneficio Beneficio { get; set; }
        public virtual PerfilSocioEconomico PerfilEconomico { get; set; }
        public virtual ICollection<MembroFamilia> MembroFamilia { get; set; }
        [ForeignKey("Voluntario")]
        public long VoluntarioId { get; set; }
        public virtual Voluntario Voluntario { get; set; }
        public ICollection<Encaminhamento> Encaminhamentos { get; set; }
    }
}