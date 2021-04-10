using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjetoBetaAutenticacao.Models
{
    [Table("MembrosFamiliar")]
    public class MembroFamilia
    {
        [Key]
        public int ParenteId { get; set; }
        

        [StringLength(200, ErrorMessage = "O campo {0} poder to no máximo {1} caracteres")]
        public string Nome { get; set; }
        [MaxLength(2)]
        [RegularExpression(@"^[0-9]{1,2}$", ErrorMessage = "Idade incorreta")]
        public string Idade { get; set; }

        [Display(Name = "CPF")]
        [RegularExpression(@"^\d{3}\.?\d{3}\.?\d{3}\-?\d{2}$", ErrorMessage = "Somente números no campo CPF")]
        public string Cpf { get; set; }

        [Display(Name = "RG")]
        [StringLength(14, ErrorMessage = "O campo {0} pode ter um tamanho maximo de {1}!")]
        [RegularExpression(@"^[0-9]{3,14}$", ErrorMessage = "Digite somente numeros no campo {0}")]
        public string Rg { get; set; }

        [StringLength(50, ErrorMessage = "O campo {0} poder to no máximo {1} caracteres")]
        public string Parentesco { get; set; }
        
        [ForeignKey("PessoaCarente")]
        public int? PessoaCarenteId { get; set; }
        public virtual PessoaCarente PessoaCarente { get; set; }
    }
}