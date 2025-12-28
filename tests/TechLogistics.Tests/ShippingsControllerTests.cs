using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq; // librería de simulación
using TechLogistics.API.Controllers;
using TechLogistics.Application.DTOs;
using TechLogistics.Application.Interfaces;
using TechLogistics.Domain.Entities;
using TechLogistics.Domain.Interfaces;
using Xunit;

namespace TechLogistics.Tests
{
    public class ShippingsControllerTests
    {
        // Simulamos (Mock) las dependencias externas
        private readonly Mock<IShippingRepository> _mockRepo;
        private readonly Mock<IMessageProducer> _mockProducer;
        private readonly ShippingsController _controller;

        public ShippingsControllerTests()
        {
            // Creamos los objetos falsos
            _mockRepo = new Mock<IShippingRepository>();
            _mockProducer = new Mock<IMessageProducer>();

            // Inyectamos los falsos en el controlador real
            _controller = new ShippingsController(_mockRepo.Object, _mockProducer.Object);
        }

        [Fact]
        public async Task Create_Should_Call_Repository_And_Producer_And_Return_Created()
        {
            // 1. Arrange
            var request = new CreateShippingRequest
            {
                RecipientName = "Test User",
                DestinationAddress = "Test Address"
            };

            // 2. Act
            var result = await _controller.Create(request);

            // 3. Assert

            // Verificamos que sea un resultado "201 Created"
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);

            // VERIFICACIÓN CLAVE DE SENIOR:
            // ¿Se llamó al método AddAsync del repositorio exactamente 1 vez?
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Shipping>()), Times.Once);

            // ¿Se envió el mensaje a RabbitMQ exactamente 1 vez?
            _mockProducer.Verify(p => p.SendMessage(It.IsAny<Shipping>()), Times.Once);
        }
    }
}
