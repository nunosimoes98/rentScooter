using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eRecarga.Models
{
    public class Posto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set;  }

        [Required]
        public string Nome { get; set; }

        [Required]
        public int EstacaoId { get; set; }

        [ForeignKey("EstacaoId")]
        public Estacao Estacao { get; set; }
    }
}