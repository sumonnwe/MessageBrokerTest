using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQPublisherService
{
    public interface IMessagePublisher
    {
        void PublishMessage(string message);
    }
}
