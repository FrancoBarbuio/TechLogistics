using Microsoft.AspNetCore.Mvc;
using TechLogistics.Application.DTOs;
using TechLogistics.Domain.Entities;
using TechLogistics.Domain.Interfaces;
using TechLogistics.Application.Interfaces; 

namespace TechLogistics.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShippingsController : ControllerBase
    {
        private readonly IShippingRepository _repository;
        private readonly IMessageProducer _messageProducer; 

        // Inyectamos AMBOS servicios
        public ShippingsController(IShippingRepository repository, IMessageProducer messageProducer)
        {
            _repository = repository;
            _messageProducer = messageProducer;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateShippingRequest request)
        {
            // 1. Guardar en BD (Estado: Pending)
            var shipping = new Shipping(request.RecipientName, request.DestinationAddress);
            await _repository.AddAsync(shipping);

            // 2. ENVIAR MENSAJE A RABBITMQ 
            // Enviamos el objeto completo para que el Worker sepa qué procesar
            _messageProducer.SendMessage(shipping);

            // 3. Responder al usuario
            return CreatedAtAction(nameof(GetById), new { id = shipping.Id }, shipping);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var shipping = await _repository.GetByIdAsync(id);
            if (shipping == null) return NotFound();
            return Ok(shipping);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _repository.GetAllAsync());
        }
    }
}