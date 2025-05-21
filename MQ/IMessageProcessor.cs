using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ
{
    public interface IMessageProcessor
    {
        void ProcessMessage(string message);
    }
}
