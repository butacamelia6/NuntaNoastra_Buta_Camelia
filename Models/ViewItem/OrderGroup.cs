using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace NuntaNoastra_Buta_Camelia.Models.ViewItem
{
    
        public class OrderGroup
        {
            [DataType(DataType.Date)]
            public DateTime? OrderDate { get; set; }
            public int CandleCount { get; set; }

        }
    
}
