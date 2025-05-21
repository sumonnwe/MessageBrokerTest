// RabbitMqPublisher.cs
using System.Text;
using RabbitMQ.Client;
using RabbitMQPublisherService;

namespace RabbitMQPublisherService
{

    public class RabbitMqPublisher : IMessagePublisher, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _exchangeName = "orders_exchange";

        public RabbitMqPublisher()
        {
            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest" };

            // Create RabbitMQ connection and channel
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declare the exchange
            _channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Fanout);
        }

        public void PublishMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            // Publish the message to the exchange
            _channel.BasicPublish(
                exchange: _exchangeName,
                routingKey: "",
                basicProperties: null,
                body: body
            );

            Console.WriteLine($"[RabbitMqPublisher] Published message: {message}");
        }

        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
        }
    }
}