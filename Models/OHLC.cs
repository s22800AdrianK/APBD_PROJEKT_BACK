using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt.Models
{
    public class OHLC
    {
        public string WatchedElementTicker { get; set; }
        public double c { get; set; }
        public double h { get; set; }
        public double l { get; set; }
        public long n { get; set; }
        public double o { get; set; }
        public long t { get; set; }
        public double v { get; set; }
        public double vw { get; set; }
        public virtual WatchedElement Watched {get; set;}
    }
}
