using Microsoft.Extensions.Options;
using MidiaPost.Cmd.Domain.Aggregates;
using MidiaPost.Cmd.InfraStructure.Config;
using MidiaPost.CQRS.Core.Lib.Domain;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiaPost.Cmd.InfraStructure.Repository
{
    public class EventStoreRepository : IEventStoreRepository
    {
        public readonly IMongoCollection<EventModel> _eventStoreCollection;

       public EventStoreRepository(IOptions<MongoDbConfig> config)
        {
            var mongoClient = new MongoClient(config.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(config.Value.Database);

            _eventStoreCollection = mongoDatabase.GetCollection<EventModel>(config.Value.Collection);
        }

        public async Task<List<EventModel>> FindAllAsync()
        {
            return await _eventStoreCollection.Find(_ => true).ToListAsync().ConfigureAwait(false);
        }

        public async Task<List<EventModel>> FindByAggregateId(Guid aggregateId)
        {
            return await _eventStoreCollection.Find(e => e.AggregateIdentifier == aggregateId).ToListAsync();
        }

        public async Task SaveAsync(EventModel @Event)
        {
            await _eventStoreCollection.InsertOneAsync(@Event).ConfigureAwait(false);
        }
    }
}
