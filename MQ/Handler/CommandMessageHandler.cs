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
    public class CommandMessageHandler : IMessageSubscriber<CommandMessage>
    {
        public void Handle(CommandMessage message)
        { 
            Console.WriteLine($"Processing CommandMessage: Topic : {message.Topic} , DeviceId: {message.DeviceId} , CommandType: {message.CommandType},  Message: {message.Message} ");
        }
    }
}
