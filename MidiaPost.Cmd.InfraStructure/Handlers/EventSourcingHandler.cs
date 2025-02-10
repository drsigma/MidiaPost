using MidiaPost.Cmd.Domain.Aggregates;
using MidiaPost.CQRS.Core.Lib.Domain;
using MidiaPost.CQRS.Core.Lib.Handlers;
using MidiaPost.CQRS.Core.Lib.InfraStructure;
using MidiaPost.CQRS.Core.Lib.Producers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiaPost.Cmd.InfraStructure.Handlers
{
    public class EventSourcingHandler : IEventSourcingHandlers<PostAggregate>
    {
        public IEventStore  _eventStore;
        private readonly IEventProducer _eventProducer;

        public EventSourcingHandler(IEventStore eventStore, IEventProducer eventProducer)
        {
            _eventStore = eventStore;
            _eventProducer = eventProducer;
        }
        public async Task<PostAggregate> GetByIdAsync(Guid aggregateId)
        {
            var aggregate  = new PostAggregate();
            var events =  await _eventStore.GetEventsAsync(aggregateId);

            if (events.Count() == 0 || events == null)
            {
                return aggregate;
            }

            aggregate.ReplayEvents(events);
            var lastVersion = events.Last().Version;

            return aggregate;
                
        }
        public async Task RepublishEventsAsync()
        {
            var aggregateIds = await _eventStore.GetAggregateIdsAsync();

            if (aggregateIds == null || !aggregateIds.Any()) return;

            foreach (var aggregateId in aggregateIds)
            {
                var aggregate = await GetByIdAsync(aggregateId);

                if (aggregate == null || !aggregate.Active) continue;

                var events = await _eventStore.GetEventsAsync(aggregateId);

                foreach (var @event in events)
                {
                    var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC");
                    await _eventProducer.ProducerAsync(topic, @event);
                }
            }
        }

        public async Task SaveAsync(AggregateRoot aggregate)
        {
            await _eventStore.SaveEventAsync(aggregate.Id, aggregate.GetUncommittedChanges(), aggregate.Version);
            aggregate.MarkChangesAsCommitted();
        }
    }
}
