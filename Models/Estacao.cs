using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eRecarga.Models
{
    public class Estacao
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set;}
        [Required]
        public string Nome { get; set; }
        [Required] 
        public TimeSpan HoraAbertura { get; set; }
        [Required] 
        public TimeSpan HoraFecho { get; set; }
        [Required]
        public double Longitude { get; set; }
        [Required] 
        public double Latitude { get; set; }

        [ForeignKey("Distrito")]
        public int DistritoId { get; set; }
        public virtual Distrito Distrito { get; set; }

        public IList<Posto> Postos { get; set; }
        public IList<Preco> Precos { get; set; }
    }
}