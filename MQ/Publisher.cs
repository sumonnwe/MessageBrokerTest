using System;
using System.Text;
using MQ;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace MQ
{ 
    public class Publisher
    {
        public void PublishOrder(Order order, string exchangeName = "orders_exchange", string queueName = "order_queue",  string routingKey = "order_routing_key")
        {
            // Create a connection to the RabbitMQ server
            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest" }; // Change HostName if needed

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                // Declare the exchange (Fanout is used here for broadcasting)
                channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);
                channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                channel.QueueBind(queueName, exchangeName, routingKey);

                // Serialize the order object to JSON
                var message = JsonConvert.SerializeObject(order);
                var messageBody = Encoding.UTF8.GetBytes(message);

                // Publish the message
                channel.BasicPublish(
                    exchange: exchangeName, 
                    routingKey: routingKey, // Blank for Fanout
                    basicProperties: null,
                    body: messageBody);

                Console.WriteLine($"[x] Sent Order: {message}");
            }
        }
    }
}