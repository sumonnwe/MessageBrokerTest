using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQ.DTOs;
using MQ.Handler.Interfaces;

namespace MQ.Handler
{
    // Example handler for User messages
    public class UserMessageHandler : IMessageSubscriber<User>
    {
        public void Handle(User message)
        {
            Console.WriteLine($"Processing User: {message.userId}");
        }
    }
}
