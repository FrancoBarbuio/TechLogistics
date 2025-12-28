using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration; // Para leer appsettings
using RabbitMQ.Client;
using System.Text.Json;
using TechLogistics.Application.Interfaces;

namespace TechLogistics.Infrastructure.Messaging
{
    public class RabbitMQProducer : IMessageProducer
    {
        private readonly IConfiguration _configuration;

        public RabbitMQProducer(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendMessage<T>(T message)
        {
            try
            {
                Console.WriteLine($"[RabbitMQ] Intentando conectar a: {_configuration.GetConnectionString("RabbitMQConnection")}");

                var factory = new ConnectionFactory
                {
                    Uri = new Uri(_configuration.GetConnectionString("RabbitMQConnection"))
                };

                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.QueueDeclare(queue: "shippings_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

                var json = System.Text.Json.JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(json);

                channel.BasicPublish(exchange: "", routingKey: "shippings_queue", basicProperties: null, body: body);

                Console.WriteLine("[RabbitMQ] ¡Mensaje enviado con éxito!");
            }
            catch (Exception ex)
            {
                // ¡ESTO NOS DIRÁ EL ERROR REAL!
                Console.WriteLine($"[RabbitMQ ERROR GRAVE]: {ex.Message}");
                throw; // Relanzamos el error para que la API no se quede callada
            }
        }
    }
}
