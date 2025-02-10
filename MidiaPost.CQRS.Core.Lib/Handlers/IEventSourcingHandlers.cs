using MidiaPost.CQRS.Core.Lib.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiaPost.CQRS.Core.Lib.Handlers
{
    public interface IEventSourcingHandlers<T>
    {
        Task SaveAsync(AggregateRoot aggregate);

        Task<T> GetByIdAsync(Guid aggregateId);

        Task RepublishEventsAsync();
    }
}
