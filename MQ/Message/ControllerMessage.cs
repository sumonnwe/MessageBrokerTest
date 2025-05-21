using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQ.DTOs;

namespace MQ.Message
{
    public class ControllerMessage<T>
    {
        public string terminalSetup { get; set; }
        public string serialNo { get; set; }

        public T Model { get; set; }
    }
}
