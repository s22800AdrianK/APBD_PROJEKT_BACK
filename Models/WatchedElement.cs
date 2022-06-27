using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt.Models
{
    public class WatchedElement
    {
        public string? logo { get; set; }
        public string ticker { get; set; }
        public string Name { get; set; }
        public string country { get; set; }
        public string description { get; set; }
        public virtual List<Watchlist> Watchlist { get; set; }
        public virtual List<OHLC> OHLCs { get; set; }
    }
}
