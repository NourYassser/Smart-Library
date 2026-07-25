using System.Collections.Concurrent;

namespace Library.BuildingBlocks.RabbitMQ.Subscriptions
{
    public interface ISubscriptionManager
    {
        void AddSubscription<TEvent, THandler>()
            where THandler : class;

        Type? GetHandler(string eventName);

        Type? GetEventType(string eventName);

        bool HasSubscription(string eventName);
    }

    public sealed class SubscriptionManager : ISubscriptionManager
    {
        private readonly ConcurrentDictionary<string, Type> _handlers = new();
        private readonly ConcurrentDictionary<string, Type> _events = new();

        public void AddSubscription<TEvent, THandler>()
            where THandler : class
        {
            var eventName = typeof(TEvent).Name;

            _handlers[eventName] = typeof(THandler);
            _events[eventName] = typeof(TEvent);
        }

        public bool HasSubscription(string eventName)
            => _handlers.ContainsKey(eventName);

        public Type? GetHandler(string eventName)
            => _handlers.GetValueOrDefault(eventName);

        public Type? GetEventType(string eventName)
            => _events.GetValueOrDefault(eventName);
    }
}
