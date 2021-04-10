using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjetoBetaAutenticacao.Models
{
    [Table("Endereco")]
    public class Endereco
    {
        [Key]
        public int EnderecoId { get; set; }


        [Display(Name = "RUA")]
        public string Rua { get; set; }
        [Display(Name = "CEP")]
        public string Cep { get; set; }

        [Display(Name = "BAIRRO")]
        public string Bairro { get; set; }
        [Display(Name = "NÚMERO")]
        public string Numero { get; set; }
        //[ForeignKey("PessoaCarente")]
        //public int PessoaCarenteId { get; set; }
        public virtual PessoaCarente PessoaCarente { get; set; }
    }
}