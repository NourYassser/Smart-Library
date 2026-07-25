using Library.BuildingBlocks.RabbitMQ.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Library.BuildingBlocks.RabbitMQ.Connection
{
    public interface IRabbitMqPersistentConnection : IAsyncDisposable
    {
        bool IsConnected { get; }

        ValueTask<IChannel> CreateChannelAsync(
            CancellationToken cancellationToken = default);
    }
    public sealed class RabbitMqPersistentConnection
    : IRabbitMqPersistentConnection
    {
        private readonly RabbitMqOptions _options;

        private IConnection? _connection;

        public RabbitMqPersistentConnection(
            IOptions<RabbitMqOptions> options)
        {
            _options = options.Value;
        }

        public bool IsConnected =>
            _connection is { IsOpen: true };

        public async ValueTask<IChannel> CreateChannelAsync(
            CancellationToken cancellationToken = default)
        {
            if (!IsConnected)
            {
                await ConnectAsync(cancellationToken);
            }

            return await _connection!.CreateChannelAsync(
                cancellationToken: cancellationToken);
        }

        private async Task ConnectAsync(
            CancellationToken cancellationToken)
        {
            if (IsConnected)
                return;

            var factory = new ConnectionFactory
            {
                HostName = _options.HostName,
                Port = _options.Port,
                UserName = _options.UserName,
                Password = _options.Password
            };

            _connection =
                await factory.CreateConnectionAsync(cancellationToken);
        }

        public async ValueTask DisposeAsync()
        {
            if (_connection != null)
                await _connection.DisposeAsync();
        }
    }
}