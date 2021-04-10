using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjetoBetaAutenticacao.Models
{
    public class Beneficio
    {
        [Key]
        public int BeneficioId { get; set; }
        
        public bool SimNao { get; set; }
        [Display(Name = "Benefício")]
        public string TipoBeneficio { get; set; }
        //[ForeignKey("PessoaCarente")]
        //public int PessoaCarenteId { get; set; }
        
        public virtual PessoaCarente PessoaCarente { get; set; }

    }
}