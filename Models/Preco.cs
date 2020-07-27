using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace eRecarga.Models
{
    public class Preco
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set;  }
        [Required]
        public string Nome { get; set; }
        [Required]
        public double Valor { get; set; }
        
        [Required]
        [Display(Name = "Hora de Início")]
        public TimeSpan HoraInicio { get; set; }
        
        [Required]
        [Display(Name = "Hora de Fim")]
        public TimeSpan HoraFim { get; set; }


        [Required]
        [Display(Name = "Estação")]
        public int EstacaoId { get; set; }

        [Display(Name = "Estação")]
        [ForeignKey("EstacaoId")]
        public Estacao Estacao { get; set; }
    }
}