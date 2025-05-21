using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ.Handler.Interfaces
{
    public interface IMessageSubscriber<T>
    {
        void Handle(T message);
    }
}
