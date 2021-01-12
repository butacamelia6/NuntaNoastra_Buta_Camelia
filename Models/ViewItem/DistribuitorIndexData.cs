using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NuntaNoastra_Buta_Camelia.Models;

namespace NuntaNoastra_Buta_Camelia.ViewItem
{
    public class DistribuitorIndexData
    {
        public IEnumerable<Distribuitor> Distribuitors { get; set; }
        public IEnumerable<Candle> Candles { get; set; }
        public IEnumerable<Order> Orders { get; set; }
    }
}

