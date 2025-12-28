using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechLogistics.Domain.Entities
{
    // Usamos 'sealed' por rendimiento si no planeamos heredar de ella.
    public sealed class Shipping
    {
        public int Id { get; set; }

        // Número de seguimiento único (Ej: TRK-998877)
        public string TrackingNumber { get; set; } = string.Empty;

        // Datos del destinatario
        public string RecipientName { get; set; } = string.Empty;
        public string DestinationAddress { get; set; } = string.Empty;

        // Fechas importantes
        public DateTime CreatedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }

        // Estado del envío (Usaremos un Enum para evitar strings mágicos)
        public ShippingStatus Status { get; set; }

        // Constructor vacío requerido por Entity Framework
        public Shipping() { }

        // Constructor para crear un envío nuevo (Validación básica)
        public Shipping(string recipient, string address)
        {
            if (string.IsNullOrWhiteSpace(recipient)) throw new ArgumentException("El destinatario es obligatorio.");

            RecipientName = recipient;
            DestinationAddress = address;
            TrackingNumber = Guid.NewGuid().ToString().Substring(0, 8).ToUpper(); // Generamos un código simple
            CreatedAt = DateTime.UtcNow;
            Status = ShippingStatus.Pending;
        }
    }

    // Enum para controlar los estados (Muy útil para Angular luego)
    public enum ShippingStatus
    {
        Pending,    // Creado, esperando procesar
        InTransit,  // En camino (RabbitMQ lo moverá aquí)
        Delivered,  // Entregado
        Cancelled
    }
}
