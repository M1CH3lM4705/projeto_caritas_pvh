using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjetoBetaAutenticacao.Areas.Admin.Data
{
    public class Paroquia
    {
        [Key]
        public long ParoquiaId { get; set; }

        [Required(ErrorMessage = "O Campo Nome da Paroquia é Obrigatório!")]
        [StringLength(100, ErrorMessage ="O campo pode ter no máximo {1} e no minimo {2} caracteres")]
        [Display(Name = "Paróquia")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O Campo Email é Obrigatório!")]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "O Campo Endereço é Obrigatório!")]
        [Display(Name = "Endereço")]
        public string Endereco { get; set; }

        [Required(ErrorMessage = "O Campo {0} é obrigatorio!")]
        public Estado Estado { get; set; }

        [StringLength(200)]
        [Required(ErrorMessage = "O Campo {0} é obrigatorio!")]
        public string Cidade { get; set; }

        public virtual ICollection<Voluntario> Voluntarios { get; set; }

    }
}