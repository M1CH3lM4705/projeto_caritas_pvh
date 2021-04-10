using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjetoBetaAutenticacao.Models
{
    [Table("Contato")]
    public class Contato
    {
        [Key]
        public int ContatoId { get; set; }
        
        public string Celular { get; set; }
        public bool WhatsApp { get; set; }
        //[ForeignKey("PessoaCarente")]
        //public int PessoaCarenteId { get; set; }
        public virtual PessoaCarente PessoaCarente { get; set; }
    }
}