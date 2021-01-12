using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NuntaNoastra_Buta_Camelia.Models
{
    public class DistribuitorCandle
    {
        public int DistribuitorID { get; set; }
        public int CandleID { get; set; }
        public Distribuitor Distribuitor { get; set; }
        public Candle Candle { get; set; }
    }
}
