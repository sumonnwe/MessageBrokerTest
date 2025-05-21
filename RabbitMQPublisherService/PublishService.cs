using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQPublisherService
{
    public class PublishService
    {  
        private readonly IMessagePublisher _messagePublisher;

        // Inject IMessagePublisher
        public PublishService(IMessagePublisher messagePublisher)
        {
            _messagePublisher = messagePublisher;
        }

        public async Task RunAsync()
        {
            Console.WriteLine("Enter messages to publish to RabbitMQ (or type 'exit' to quit):");

            string? message;
            while ((message = Console.ReadLine())?.ToLower() != "exit")
            {
                _messagePublisher.PublishMessage(message);
                Console.WriteLine("Message published!");
            }

            await Task.CompletedTask;
        }
    }
} 
