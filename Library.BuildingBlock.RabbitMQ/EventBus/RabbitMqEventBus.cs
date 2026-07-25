using Library.BuildingBlocks.Events;
using Library.BuildingBlocks.Messaging;
using Library.BuildingBlocks.RabbitMQ.Configuration;
using Library.BuildingBlocks.RabbitMQ.Connection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Library.BuildingBlocks.RabbitMQ.EventBus
{
    public sealed class RabbitMqEventBus : IEventBus
    {
        private readonly IRabbitMqPersistentConnection _connection;
        private readonly RabbitMqOptions _options;


        public RabbitMqEventBus(
            IRabbitMqPersistentConnection connection,
            IOptions<RabbitMqOptions> options)
        {
            _connection = connection;
            _options = options.Value;
        }

        public async Task PublishAsync(
    IntegrationEvent @event,
    CancellationToken cancellationToken = default)
        {
            var channel =
                await _connection.CreateChannelAsync(cancellationToken);

            await channel.ExchangeDeclareAsync(
                exchange: _options.ExchangeName,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false,
                cancellationToken: cancellationToken);

            var json =
                JsonSerializer.Serialize(@event);

            var body =
                Encoding.UTF8.GetBytes(json);

            var properties =
                new BasicProperties
                {
                    Persistent = true
                };

            await channel.BasicPublishAsync(
                   exchange: _options.ExchangeName,
                   routingKey: @event.EventName,
                   mandatory: false,
                   basicProperties: properties,
                   body: body,
                   cancellationToken: cancellationToken);

            await channel.DisposeAsync();
        }
    }
}
