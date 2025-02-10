using MidiaPost.Cmd.Domain.Aggregates;
using MidiaPost.CQRS.Core.Lib.Domain;
using MidiaPost.CQRS.Core.Lib.Events;
using MidiaPost.CQRS.Core.Lib.InfraStructure;
using MidiaPost.CQRS.Core.Lib.Producers;

namespace MidiaPost.Cmd.InfraStructure.Stores
{
    public class EventStore : IEventStore
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IEventProducer _eventProducers;
        public EventStore(IEventStoreRepository eventStoreRepository, IEventProducer eventProducers)
        {
            _eventStoreRepository = eventStoreRepository;
            _eventProducers = eventProducers;
        }
        public async Task<List<Guid>> GetAggregateIdsAsync()
        {
            var eventStream = await _eventStoreRepository.FindAllAsync();

            if (eventStream == null || !eventStream.Any())
                throw new ArgumentNullException(nameof(eventStream), "Could not retrieve event stream from the event store!");

            return eventStream.Select(x => x.AggregateIdentifier).Distinct().ToList();
        }

        public async Task<IEnumerable<BaseEvent>> GetEventsAsync(Guid aggregateId)
        {
           var eventStream = await _eventStoreRepository.FindByAggregateId(aggregateId);

            if(eventStream == null || eventStream.Count == 0)
            {
                throw new DirectoryNotFoundException("Id not Exists");
            }
            return eventStream.OrderBy(e => e.Version).Select(x => x.EventData).ToList();
        }

        public async Task SaveEventAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion)
        {
            var eventStream = await _eventStoreRepository.FindByAggregateId(aggregateId);

            if (expectedVersion != -1 && eventStream[^1].Version != expectedVersion )
            {
                throw new DirectoryNotFoundException("Concurrency problem");
            }

            var version = expectedVersion;

            foreach(var @event in events)
            {
                version++;
                @event.Version = version;
                var eventTupe = @event.GetType().Name;
                var eventModel = new EventModel
                {
                    AggregateIdentifier = aggregateId,
                    EventData = @event,
                    EventType = eventTupe,
                    Version = version
                };

                await _eventStoreRepository.SaveAsync(eventModel);

                var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC");
                await _eventProducers.ProducerAsync(topic, @event);
            }   
        }
    }
    
 }

