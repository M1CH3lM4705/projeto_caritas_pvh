using ProjetoBetaAutenticacao.Models;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ProjetoBetaAutenticacao.Areas.Admin.Data
{
    public class Voluntario
    {
        public Voluntario()
        {
            Ativo = true;
        }
        [Key]
        public long VoluntarioId { get; set; }

        [Display(Name = "Login")]
        [Required(ErrorMessage = "O Campo {0} é Obrigatório!")]
        [StringLength(100, ErrorMessage = "O Campo {0} pode ter no máximo {1} e minimo {2} caracteres", MinimumLength = 5)]
        [Index("UserNameIndex", IsUnique = true)]
        
        public string UserName { get; set; }

        [Required(ErrorMessage = "O Campo {0} é obigatório!")]
        [RegularExpression("^[A-Za-záàâãéèêíïóôõöúçñÁÀÂÃÉÈÍÏÓÔÕÖÚÇÑ ]+$", ErrorMessage = "Digite Somente Letras no Campo {0}")]
    
        public string Nome { get; set; }
        [Required(ErrorMessage = "O Campo {0} é obigatório!")]
        [RegularExpression("^[A-Za-záàâãéèêíïóôõöúçñÁÀÂÃÉÈÍÏÓÔÕÖÚÇÑ ]+$", ErrorMessage ="Digite Somente Letras no Campo {0}")]
        [Display(Name = "Sobrenome")]
        public string SobreNome { get; set; }
        [Display(Name = "Nome Completo")]
        public string NomeCompleto { get { return string.Format("{0} {1}", this.Nome, this.SobreNome); } }

        public bool Ativo { get; set; }

        [Required(ErrorMessage = "Selecione uma {0})")]
        [Display(Name ="Paróquia")]
        public long ParoquiaId { get; set; }
        public virtual Paroquia Paroquia { get; set; }

        [Required(ErrorMessage = "Selecione um perfil")]
        [Display(Name = "Perfil")]
        public string Role { get; set; }

        public virtual ICollection<PessoaCarente> PessoasCarentes { get; set; }

        
    }
}