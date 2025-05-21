using System;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using MQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MQ.DTOs;
using MQ.Handler.Interfaces;
using MQ.Message;

namespace ConsumerService
{ 
    public class RabbitMQConsumerService : IHostedService
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly RabbitMQSettings _rabbitMQSettings;
        private IMessageSubscriber<Order> _order;
        private IMessageSubscriber<User> _user;
        private IMessageSubscriber<EventMessage> _event;
        private IMessageSubscriber<CommandMessage> _command;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMQConsumerService(
        IOptions<RabbitMQSettings> options,
        IConnectionFactory connectionFactory)
        {
            _rabbitMQSettings = options.Value;
            _connectionFactory = connectionFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var connection = _connectionFactory.CreateConnection();
            _channel = connection.CreateModel();
            // Declare the exchange and queue, then bind them
            //_channel.ExchangeDeclare(_rabbitMQSettings.ExchangeName, ExchangeType.Direct, durable: true);
            //_channel.QueueDeclare(_rabbitMQSettings.QueueName, durable: true, exclusive: false, autoDelete: false);
            //_channel.QueueBind(_rabbitMQSettings.QueueName, _rabbitMQSettings.ExchangeName, _rabbitMQSettings.RoutingKey);

            foreach (var queue in _rabbitMQSettings.Queue)
            {
                var queueName = queue.QueueName;
                var exchangeName = queue.ExchangeName;
                var routingKey = queue.RoutingKey;


                _channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, durable: true);
                _channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false);
                _channel.QueueBind(queueName, exchangeName, routingKey);

            }
            // Set up consumer
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var messageString = Encoding.UTF8.GetString(body);


                //To Subscribe, you can use with your proper class not with condition common check
                if (messageString.Contains("OrderId"))
                {
                    var message = JsonSerializer.Deserialize<Order>(messageString); 
                    _order?.Handle(message);
                    Console.WriteLine("ConsumerService: Order: " + message.OrderId);
                }
                else if (messageString.Contains("userName"))
                {
                    var message = JsonSerializer.Deserialize<CommandMessage>(messageString);
                    //var handler = _serviceProvider.GetService<IMessageSubscriber<User>>();
                    _command?.Handle(message);
                    Console.WriteLine("ConsumerService: User Object:  " + message.Message);
                }
                else if (messageString.Contains("EntryLogId"))
                {
                    var message = JsonSerializer.Deserialize<EventMessage>(messageString);
                    //var handler = _serviceProvider.GetService<IMessageSubscriber<User>>();
                    _event?.Handle(message);
                    Console.WriteLine("ConsumerService: Message Object: " + message.Message);

                    //Handle object by topic 
                    if (message.Topic.Equals("CardTransaction"))
                    {
                        //EntryLog e = (EntryLog) message.Message ;
                        //Console.WriteLine("ConsumerService: EntryLog:  CardNo: " + e.cardNo);
                        //    var entryLog = JsonSerializer.Deserialize<EntryLog>(message.Message);
                    }
                }
                // Deserialize and handle the message
                //var message = JsonSerializer.Deserialize<MyMessage>(messageString);
                //_messageHandler.Handle(message);

                // Acknowledge message
                _channel.BasicAck(ea.DeliveryTag, false);
            };

            foreach (var queue in _rabbitMQSettings.Queue)
            {
                _channel.BasicConsume(queue.QueueName, autoAck: false, consumer);
            }

            Console.WriteLine("RabbitMQ Consumer started");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _channel?.Close();
            return Task.CompletedTask;
        }
    }
}
/*

//public class RabbitMQConsumerService
//{
    private readonly IServiceProvider _serviceProvider;
    private readonly IModel _channel;

    public RabbitMQConsumerService(IServiceProvider serviceProvider, IConnectionFactory connectionFactory)
    {
        _serviceProvider = serviceProvider;

        // Create RabbitMQ connection and channel
        var connection = connectionFactory.CreateConnection();
        _channel = connection.CreateModel();
        _channel.QueueDeclare(queue: "orders_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);
    }

    public void StartConsuming()
    {
        _channel.QueueDeclare(queue: "command-control-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

        _channel.BasicQos(0, 1, false);

        var consumer = new EventingBasicConsumer(_channel);

        _channel.BasicConsume(queue: "command-control-queue", autoAck: false, consumer: consumer); 
 
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var messageString = Encoding.UTF8.GetString(body);
            var props = ea.BasicProperties;
            var replyProps = _channel.CreateBasicProperties();
            replyProps.CorrelationId = props.CorrelationId;

            Console.WriteLine($" [.] Received request: {messageString}"); 


            // Determine message type and process
            if (messageString.Contains("OrderId"))
            { 
                var message = JsonSerializer.Deserialize<Order>(messageString);
                var handler = _serviceProvider.GetService<IMessageSubscriber<Order>>(); 
                handler?.Handle(message);
                Console.WriteLine("ConsumerService: Order: " + message.OrderId);


                var responseBytes = Encoding.UTF8.GetBytes(message.OrderId.ToString());

                //RPC Handle
                _channel.BasicPublish(exchange: "", routingKey: props.ReplyTo, basicProperties: replyProps, body: responseBytes);
            }
            else if (messageString.Contains("UserId"))
            {
                var message = JsonSerializer.Deserialize<User>(messageString);
                var handler = _serviceProvider.GetService<IMessageSubscriber<User>>();
                handler?.Handle(message);
                Console.WriteLine("ConsumerService: User:  " + message.UserId);


                var responseBytes = Encoding.UTF8.GetBytes(message.UserId.ToString());

                //RPC Handle
                _channel.BasicPublish(exchange: "", routingKey: props.ReplyTo, basicProperties: replyProps, body: responseBytes);
            }
            _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
        };

        //_channel.BasicConsume(queue: "orders_queue", autoAck: false, consumer: consumer);
    }
}
}*/