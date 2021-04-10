using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjetoBetaAutenticacao.Areas.Admin.Data
{
    [NotMapped]
    public class VoluntarioSenha : Voluntario
    {
        [Required(ErrorMessage = "O Campo {0} é obrigatorio")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "O/A {0} deve ter no mínimo {2} caracteres.", MinimumLength = 6)]
        public string Senha { get; set; }

        
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Senha")]
        [Compare("Senha", ErrorMessage = "A Senha e a Senha de confirmação não são idênticas. ")]
        public string ConfirmarSenha { get; set; }
    }
}