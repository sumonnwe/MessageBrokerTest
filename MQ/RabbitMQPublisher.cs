// RabbitMqPublisher.cs
using System.Text;
using RabbitMQ.Client;
using Microsoft.Extensions.Options;
using System.Runtime;
using MQ.Handler.Interfaces;
using System.Collections.Concurrent;
using RabbitMQ.Client.Events;
using System.Threading.Channels;

namespace MQ
{

    public class RabbitMQPublisher : IMessagePublisher, IDisposable
    {
        //private string _exchangeName = "orders_exchange"; 
        private readonly RabbitMQSettings _settings;
        private readonly IConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;

        private readonly string _replyQueueName;
        private readonly ConcurrentDictionary<string, string> _responseDict = new();
        private readonly TaskCompletionSource<string> _responseTask;
        private readonly EventingBasicConsumer _consumer;

        public RabbitMQPublisher(IOptions<RabbitMQSettings> options, IConnectionFactory connectionFactory)
        {
            _settings = options.Value;
            _connectionFactory = connectionFactory;
            
            // InitializeRabbitMQ();
            // Create a connection and channel
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _replyQueueName = _channel.QueueDeclare().QueueName;
            _consumer = new EventingBasicConsumer(_channel);

            _responseTask = new TaskCompletionSource<string>();
            _consumer.Received += (model, ea) =>
            { 
                var response = Encoding.UTF8.GetString(ea.Body.ToArray());
                var correlationId = ea.BasicProperties.CorrelationId;

                _responseTask.SetResult(response);
                //if (_responseDict.ContainsKey(correlationId))
                //{
                //    _responseDict[correlationId] = response;
                //}
            };

            _channel.BasicConsume(queue: _replyQueueName, autoAck: true, consumer: _consumer); 
        }


        private void InitializeRabbitMQ()
        {
            /*
             _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _replyQueueName = _channel.QueueDeclare().QueueName;
            _consumer = new EventingBasicConsumer(_channel);

            _consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var response = Encoding.UTF8.GetString(body);
                var correlationId = ea.BasicProperties.CorrelationId;

                if (_responseDict.ContainsKey(correlationId))
                {
                    _responseDict[correlationId] = response;
                }
            };

            _channel.BasicConsume(queue: _replyQueueName, autoAck: true, consumer: _consumer); 
             */

        }


        public async Task<string> PublishMessage(string message, string queueName, string exchangeName, string routingKey)
        {
            var correlationId = Guid.NewGuid().ToString();
            var props = _channel.CreateBasicProperties();
            props.ReplyTo = _replyQueueName;
            props.CorrelationId = correlationId;

            var body = Encoding.UTF8.GetBytes(message); 

            // Publish the message to the exchange
            _channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: props, body: body);

            Console.WriteLine($"[X] Published message: {message}");
            return await _responseTask.Task;


            //_responseDict[correlationId] = null;
            //while (_responseDict[correlationId] == null)
            //{
                // Waiting for response
                //Console.WriteLine("..waiting ...");
            //}

            //return _responseDict[correlationId];
        }

        public async Task<string> PublishMessageWithResponse(string message, string queueName, string exchangeName, string routingKey)
        {

            //Callback replyQ
            var correlationId = Guid.NewGuid().ToString();
            var props = _channel.CreateBasicProperties();
            props.ReplyTo = _replyQueueName;
            props.CorrelationId = correlationId;
            //Set the message
            var body = Encoding.UTF8.GetBytes(message);

            // Declare the exchange
            //_channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct, durable: true);
            //If just want to declare exchange, no need following two lines to bind to queue.
            //_channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            //_channel.QueueBind(queueName, exchangeName, routingKey);


            // Publish the message to the exchange
            _channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: props, body: body);

            Console.WriteLine($"[...] Published message: {message}");
            return await _responseTask.Task;

            //_responseDict[correlationId] = null;
            //while (_responseDict[correlationId] == null)
            //{
            //Waiting for response
            //}

            //return _responseDict[correlationId]; 
        }

        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
        }
    }
}