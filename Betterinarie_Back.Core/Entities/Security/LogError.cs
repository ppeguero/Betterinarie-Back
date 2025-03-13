using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betterinarie_Back.Core.Entities.Security
{
    public class LogError
    {
        [Key]
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Mensaje { get; set; }
        public string StackTrace { get; set; }
        public string? UsuarioId { get; set; }
    }
}
