using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiaPost.CQRS.Core.Lib.Producers
{
    public interface IEventProducer
    {
        Task ProducerAsync<T>(string topic, T @event) where T : class;
    }
}
