using Confluent.Kafka;
using Microsoft.Extensions.Options;
using MidiaPost.CQRS.Core.Lib.Producers;
using System.Text.Json;

namespace MidiaPost.Cmd.Api.Producers
{
    public class EventProducer : IEventProducer
    {
        private readonly ProducerConfig _config;

        public EventProducer(IOptions<ProducerConfig> config)
        {
            _config = config.Value;

        }

        public async Task ProducerAsync<T>(string topic, T @event) where T : class
        {

            using var producer = new ProducerBuilder<string, string>(_config)
                .SetKeySerializer(Serializers.Utf8)
                .SetValueSerializer(Serializers.Utf8)
                .Build();

            var eventMessage = new Message<string, string>
            {
                Key = typeof(T).Name,
                Value = JsonSerializer.Serialize(@event, @event.GetType())
            };  

            var deliveryReport = await producer.ProduceAsync(topic, eventMessage);

            if (deliveryReport.Status != PersistenceStatus.Persisted)
            {
                throw new Exception("Error to send message");
            }
        }
    }
}
