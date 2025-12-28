using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TechLogistics.Domain.Entities;

namespace TechLogistics.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        private IConnection _connection;
        private IModel _channel;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            InitRabbitMQ();
        }

        private void InitRabbitMQ()
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri(_configuration.GetConnectionString("RabbitMQConnection"))
            };

            // Creamos la conexión persistente
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Aseguramos que la cola exista (Idempotencia)
            _channel.QueueDeclare(queue: "shippings_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);

            // Este evento se dispara cada vez que RabbitMQ nos empuja un mensaje
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                // Deserializamos el mensaje a nuestro objeto de Dominio
                var shipping = JsonSerializer.Deserialize<Shipping>(content);

                // --- AQUÍ SIMULAMOS EL TRABAJO PESADO ---
                ProcesarEnvio(shipping);

                // Confirmamos a RabbitMQ que terminamos bien (ACK)
                // Si no hacemos esto, RabbitMQ volverá a poner el mensaje en cola
                _channel.BasicAck(ea.DeliveryTag, false);
            };

            // Le decimos a RabbitMQ: "Empieza a mandarme mensajes"
            _channel.BasicConsume("shippings_queue", false, consumer);

            return Task.CompletedTask;
        }

        private void ProcesarEnvio(Shipping shipping)
        {
            _logger.LogInformation($"[NUEVO PEDIDO] Procesando envío para: {shipping.RecipientName}");
            _logger.LogInformation($" -> Generando Guía PDF: {shipping.TrackingNumber}.pdf");
            _logger.LogInformation($" -> Enviando Email a: {shipping.DestinationAddress}");

            // Simulamos que tarda 2 segundos
            Thread.Sleep(2000);

            _logger.LogInformation($"[FINALIZADO] Envío {shipping.TrackingNumber} listo para despacho.\n");
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
