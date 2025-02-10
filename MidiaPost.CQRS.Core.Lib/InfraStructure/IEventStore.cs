using MidiaPost.CQRS.Core.Lib.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiaPost.CQRS.Core.Lib.InfraStructure
{
    public interface IEventStore
    {
        Task SaveEventAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion);
        Task<IEnumerable<BaseEvent>> GetEventsAsync(Guid aggregateId);
        Task<List<Guid>> GetAggregateIdsAsync();
    }
}
