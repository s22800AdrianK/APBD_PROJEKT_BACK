using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt.Models.DTOs
{
    public class CompanyDetailed
    {
        public Logo? branding { get; set; }
        public string ticker { get; set; }
        public string name { get; set; }
        public string locale { get; set; }
        public string description { get; set; }
        public List<OHLC> oHLCs { get; set; }
    }

    public class Logo
    {
        public string logo_url { get; set; }
    }
}
