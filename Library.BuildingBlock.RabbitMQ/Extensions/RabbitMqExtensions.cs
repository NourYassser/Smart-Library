using Library.BuildingBlocks.Messaging;
using Library.BuildingBlocks.RabbitMQ.Configuration;
using Library.BuildingBlocks.RabbitMQ.Connection;
using Library.BuildingBlocks.RabbitMQ.EventBus;
using Library.BuildingBlocks.RabbitMQ.Subscriptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library.BuildingBlock.RabbitMQ.Extensions;

public static class RabbitMqExtensions
{
    public static IServiceCollection AddRabbitMq(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<RabbitMqOptions>(
            configuration.GetSection(RabbitMqOptions.SectionName));

        services.AddSingleton<IRabbitMqPersistentConnection,
            RabbitMqPersistentConnection>();

        services.AddSingleton<IEventBus,
            RabbitMqEventBus>();

        services.AddSingleton<ISubscriptionManager,
            SubscriptionManager>();

        return services;
    }
}