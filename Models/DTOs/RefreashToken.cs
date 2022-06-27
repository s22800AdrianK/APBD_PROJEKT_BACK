using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt.Models.DTOs
{
    public class RefreashToken
    {
        [Required]
        public string token { get; set; }
    }
}
