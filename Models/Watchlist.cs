using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt.Models
{
    public class Watchlist
    {
        public int IdUser { get; set; }
        public string ticker { get; set; }

        public virtual Client Client { get; set; }
        public virtual WatchedElement WatchedElement { get; set; }
    }
}
