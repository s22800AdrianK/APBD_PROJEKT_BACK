using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt.Models.DTOs
{
    public class ClientGET
    {
        [Required]
        public int idUser { get; set; }
        public string ticker { get; set; }
    }
}
