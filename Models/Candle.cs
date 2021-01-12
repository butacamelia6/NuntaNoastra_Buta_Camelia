using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace NuntaNoastra_Buta_Camelia.Models
{
    
        public class Candle
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Distribuitor { get; set; }

            [Column(TypeName = "decimal(6, 2)")]
            public decimal Price { get; set; }
            public ICollection<Order> Orders { get; set; }
            public ICollection<DistribuitorCandle> DistribuitorCandles { get; set; }

        }
    
}
