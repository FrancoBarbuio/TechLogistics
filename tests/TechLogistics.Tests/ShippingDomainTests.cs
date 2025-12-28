using TechLogistics.Domain.Entities;
using Xunit;

namespace TechLogistics.Tests
{
    public class ShippingDomainTests
    {
        [Fact] // Fact significa "Esto es una prueba simple"
        public void Create_Shipping_Should_Generate_TrackingNumber_And_PendingStatus()
        {
            // 1. Arrange (Preparar)
            var recipient = "Juan Perez";
            var address = "Calle Falsa 123";

            // 2. Act (Actuar)
            var shipping = new Shipping(recipient, address);

            // 3. Assert (Verificar)
            Assert.NotNull(shipping.TrackingNumber); // Debe tener número
            Assert.Equal(8, shipping.TrackingNumber.Length); // Debe ser de 8 caracteres
            Assert.Equal(ShippingStatus.Pending, shipping.Status); // Debe nacer como Pending
            Assert.Equal(recipient, shipping.RecipientName);
        }

        [Fact]
        public void Create_Shipping_EmptyRecipient_Should_Throw_Exception()
        {
            // Assert & Act (Verificamos que lance el error)
            Assert.Throws<ArgumentException>(() => new Shipping("", "Calle 123"));
        }
    }
}