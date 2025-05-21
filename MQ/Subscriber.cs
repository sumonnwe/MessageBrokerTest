using System;
using System.Text;
using MQ;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MQ
{ 

    public class Subscriber
    {
        private readonly IMessageProcessor _messageProcessor;
        private readonly string _exchangeName;
        private readonly string _queueName;

        public Subscriber(IMessageProcessor messageProcessor, string exchangeName = "orders_exchange", string queueName = "order_queue")
        {
            _messageProcessor = messageProcessor;
            _exchangeName = exchangeName;
            _queueName = queueName;
        }

        public void StartListening()
        {
            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest" };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Fanout);
                //var queueName = channel.QueueDeclare().QueueName;
                channel.QueueBind(queue: _queueName, exchange: _exchangeName, routingKey: "");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    // Use the injected IMessageProcessor to process the message
                    _messageProcessor.ProcessMessage(message);

                    // Acknowledge the message
                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                };

                channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);

                Console.WriteLine("Listening for messages. Press [enter] to exit.");
                Console.ReadLine();
            }
        }

        /*
        public void StartListening(Action<string> onMessageReceived, string exchangeName = "orders_exchange", string queueName = "order_queue", string routingKey = "order_routing_key")
        {
            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest" };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);

                //var queueName = channel.QueueDeclare().QueueName;
                channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routingKey);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body); 

                    try
                    {
                        // Simulate processing of the message , Deserialize the message to an Order object
                        var order = JsonConvert.DeserializeObject<Order>(message);
                        Console.WriteLine($"[x] Received Order: {order.OrderId}, Customer: {order.CustomerName}, Amount: {order.Amount}");
                        
                        // Call the provided callback function with the message
                        onMessageReceived?.Invoke(message);

                        // After processing, send manual acknowledgment
                        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                        Console.WriteLine("Message acknowledged.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing message: {ex.Message}");
                        // Optionally, use BasicNack to reject the message and re-queue it
                        channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true);
                    } 

                };

                channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

                Console.WriteLine("Press [enter] to exit.");
                Console.ReadLine();
            }
        }
        */
    }
} 