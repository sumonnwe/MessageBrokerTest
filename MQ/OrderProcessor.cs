using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ
{
    public class OrderProcessor : IMessageProcessor
    {
        public void ProcessMessage(string message)
        {
            // Here, you could deserialize and process the message
            Console.WriteLine($"[OrderProcessor] Processing message: {message}");
            // Further processing logic goes here


            var order = JsonConvert.DeserializeObject<Order>(message);
            Console.WriteLine($"[x] Received Order: {order.OrderId}, Customer: {order.CustomerName}, Amount: {order.Amount}");

        }
    }
}
