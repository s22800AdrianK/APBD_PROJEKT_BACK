using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt.Models
{
    public class Client
    {
        public int IdUser { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string RefreshToken { get; set; }
        public string Email { get; set; }
        public virtual List<Watchlist> Watchlist { get; set; }

    }
}
