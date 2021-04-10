using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjetoBetaAutenticacao.Models
{
    [Table("PerfilSocioEconomico")]
    public class PerfilSocioEconomico
    {

        [Key]
        public int PSocioEcoId { get; set; }
        
        public Escolaridade Escolaridade { get; set; }
        [Display(Name = "Ocupação Atual")]
        [StringLength(50, ErrorMessage = "O campo {0} pode ter no maximo {1} caracteres!")]
        public string OcupacaoAtual { get; set; }
        //[ForeignKey("PessoaCarente")]
        //public int PessoaCarenteId { get; set; }
        public virtual PessoaCarente PessoaCarente { get; set; }
    }
}