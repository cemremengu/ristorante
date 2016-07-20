using System;
using System.Collections.Generic;

namespace Ristorante.Infrastructure
{
    public class CorrelationManager : IHandle<CorrelatedMessage>
    {
        private readonly Dictionary<Guid, Dispatcher> _subscriptions;

        public CorrelationManager()
        {
            _subscriptions = new Dictionary<Guid, Dispatcher>();
        }

        public void Subscribe<T>(Guid correlation, IHandle<T> subscriber) where T : Message
        {
            Dispatcher dispatcher;
            if (!_subscriptions.TryGetValue(correlation, out dispatcher))
            {
                dispatcher = new Dispatcher();

                _subscriptions[correlation] = dispatcher;
            }

            dispatcher.Subscribe(subscriber);
        }

        public void Unsubscribe<T>(Guid correlation, IHandle<T> subscriber) where T : Message
        {
            Dispatcher dispatcher;

            if (_subscriptions.TryGetValue(correlation, out dispatcher))
            {
                dispatcher.Unsubscribe(subscriber);

                if (dispatcher.IsEmpty)
                {
                    _subscriptions.Remove(correlation);
                }
            }
        }

        public void Handle(CorrelatedMessage message)
        {
            if (message.CorrelationId != Guid.Empty)
            {
                Dispatcher dispatcher;

                if (_subscriptions.TryGetValue(message.CorrelationId, out dispatcher))
                {
                    dispatcher.Handle(message);
                }
            }
        }
    }
}
