using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eRecarga.Models
{
    public class Reserva
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Estação")]
        public int EstacaoId { get; set; }

        [Display(Name = "Estação")]
        [ForeignKey("EstacaoId")]
        public Estacao Estacao { get; set; }

        [Required]
        public DateTime Data { get; set; }

        [Display(Name = "Código")]
        public String CodigoServico { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

    }
}