using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjetoBetaAutenticacao.Infraestrutura.Dao;


namespace ProjetoBetaAutenticacao.Models
{
    [Table("Encaminhamento")]
    public class Encaminhamento
    {
        public Encaminhamento()
        {
            DataSolicitacao = DateTime.Now;
        }
        public Encaminhamento(int id)
        {
            PessoaCarenteId = id;
        }
        [Key]
        public int EncaminhamentoId { get; set; }
        
        
        [Required(ErrorMessage = "Por favor informe o ENCAMINHAMENTO!")]
        [Display(Name = "Tipo de Encaminhamento")]
        public string TipoEncaminhamento { get; set; }

        [DefaultValue("false")]
        [Display(Name = "Marque o 'CHECKBOX' para confirmar a Entrega")]
        public bool Status { get; set; }
        [Display(Name = "Data da Solicitação")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataSolicitacao { get; set; }

        [Display(Name = "Data da Entrega")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DataEntrega { get; set; }

        [ForeignKey("PessoaCarente")]
        public int? PessoaCarenteId { get; set; }
        public virtual PessoaCarente PessoaCarente { get; set; }

        public void FinalizarEncaminhamento(Encaminhamento encaminhamento)
        {
            var objEnc = new Encaminhamento();
            if(encaminhamento.Status == false)
            {
                objEnc.TipoEncaminhamento = encaminhamento.TipoEncaminhamento;
                objEnc.DataSolicitacao = encaminhamento.DataSolicitacao;
                objEnc.Status = true;
                objEnc.DataEntrega = DateTime.Now;
                objEnc.EncaminhamentoId = encaminhamento.EncaminhamentoId;
                objEnc.PessoaCarenteId = encaminhamento.PessoaCarenteId;

                new EncaminhamentoDao().Alterar(objEnc);
            }
            
            //DateTime proximaData;
            //int dias = 0;
            //if(encaminhamento.status == true)
            //{
            //    var dataEntrega = Convert.ToDateTime(encaminhamento.DataEntrega);
            //   proximaData = VerificarData(dataEntrega);
            //    TimeSpan data = dataEntrega.Subtract(proximaData);
            //    dias = data.Days;
 
            //}
            //return dias;
        }

        public int TesteData(DateTime data)
        {
            TimeSpan final = data.Subtract(DateTime.FromOADate(30));
            int numero = final.Days;
            return numero;
        }

        public string DataString(DateTime data)
        {
            return VerificarData(data).ToString("dd/MM/yyyy");
        }

        //Contabiliza a data a cada 30 dias removendo sabados e domingos, retornando a proxima entrega
        public DateTime VerificarData(DateTime data)
        {
            int dias;
           
            if(data.Month == DateTime.ParseExact("Fevereiro", "MMMM", CultureInfo.CurrentCulture).Month)
            {
                dias = 28;
                if (data.DayOfWeek == DayOfWeek.Saturday)
                {
                    data = data.AddDays(2);
                    dias -= 1;
                }
                else if (data.DayOfWeek == DayOfWeek.Sunday)
                {
                    data = data.AddDays(1);
                    dias -= 1;
                }
                data = data.AddDays(dias / 5 * 7);
                int extraDias = dias % 5;
                if ((int)data.DayOfWeek + extraDias > 5) extraDias += 2;
                return data.AddDays(extraDias);
            }
            else
            {
                dias = 30;
                if (data.DayOfWeek == DayOfWeek.Saturday)
                {
                    data = data.AddDays(2);
                    dias -= 1;
                }
                else if (data.DayOfWeek == DayOfWeek.Sunday)
                {
                    data = data.AddDays(1);
                    dias -= 1;
                }
                data = data.AddDays(dias / 5 * 7);
                int extraDias = dias % 5;
                if ((int)data.DayOfWeek + extraDias > 5) extraDias += 2;
                return data.AddDays(extraDias);
            }
  
        }
    }
}