using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQ.DTOs;
using MQ.Handler.Interfaces;
using MQ.Message;

namespace MQ.Handler
{
    // Example handler for User messages
    public class EventMessageHandler : IMessageSubscriber<EventMessage>
    {
        public void Handle(EventMessage message)
        {
            Console.WriteLine($"Processing EventMessage: Topic : {message.Topic} , DeviceId: {message.DeviceId} , Message: {message.Message} ");
        }
    }
}
