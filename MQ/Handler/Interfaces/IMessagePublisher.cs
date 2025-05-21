using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ.Handler.Interfaces
{
    public interface IMessagePublisher
    {
        Task<string> PublishMessage(string message, string queue, string exchange, string routingKey);

        Task<string> PublishMessageWithResponse(string message, string queueName, string exchange, string routingKey);
    }
}
