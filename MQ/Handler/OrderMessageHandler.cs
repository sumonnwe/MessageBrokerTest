using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQ.Handler.Interfaces;
using MQ.DTOs;

namespace MQ.Handler
{
    // Example handler for Order messages
    public class OrderMessageHandler : IMessageSubscriber<Order>
    {
        public void Handle(Order message)
        {
            Console.WriteLine($"Processing Order: {message.OrderId}");
        }
    }

}
