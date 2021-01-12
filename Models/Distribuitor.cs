using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace NuntaNoastra_Buta_Camelia.Models
{
    public class Distribuitor
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Nume distribuitor")]
        [StringLength(50)]
        public string DistribuitorName { get; set; }

        [StringLength(70)]
        public string Adress { get; set; }
        public ICollection<DistribuitorCandle> DistribuitorCandles { get; set; }

    }
}
