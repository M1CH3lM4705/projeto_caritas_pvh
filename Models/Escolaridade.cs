using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjetoBetaAutenticacao.Models
{
    public enum Escolaridade
    {
        [Display(Name = "SEM ESCOLARIDADE")]
        SEM_ESCOLARIDADE,
        [Display(Name = "FUNDAMENTAL INCOMPLETO")]
        FUNDAMENTAL_INCOMPLETO,
        [Display(Name = "FUNDAMENTAL")]
        FUNDAMENTAL,
        [Display(Name = "MÉDIO INCOMPLETO")]
        MEDIO_INCOMPLETO,
        [Display(Name = "MÉDIO")]
        MEDIO,
        [Display(Name = "TÉCNICO")]
        TECNICO,
        [Display(Name = "SUPERIOR INCOMPLETO")]
        SUPERIOR_INCOMPLETO,
        [Display(Name = "ENSINO SUPERIOR")]
        SUPERIOR
        
    }
}