using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TechLogistics.Application.DTOs
{
    public class CreateShippingRequest
    {
        // DataAnnotations: Validaciones automáticas de .NET
        // Si no mandan estos datos, la API devuelve error 400 automáticamente.

        [Required(ErrorMessage = "El destinatario es obligatorio")]
        public string RecipientName { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria")]
        public string DestinationAddress { get; set; }
    }
}
