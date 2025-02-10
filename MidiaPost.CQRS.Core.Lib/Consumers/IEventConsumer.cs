using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiaPost.CQRS.Core.Lib.Consumers
{
    public interface IEventConsumer
    {
        void Consume(string topic);
    }
}
