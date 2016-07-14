namespace Ristorante.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Dispatcher : IHandle<Message>
    {
        private readonly Dictionary<Type, List<IMessageHandler>> _subscriptions =
            new Dictionary<Type, List<IMessageHandler>>();

        public void Handle(Message message)
        {
            var messageType = message.GetType();

            do
            {
                List<IMessageHandler> handlers;

                if (_subscriptions.TryGetValue(messageType, out handlers))
                {
                    foreach (var messageHandler in handlers)
                    {
                        messageHandler.Handle(message);
                    }
                }

                messageType = messageType.BaseType;
            } while (messageType != typeof(object));
        }

        public void Subscribe<T>(IHandle<T> subscriber) where T : Message
        {
            var s = new MessageHandler<T>(subscriber);

            List<IMessageHandler> handlers;

            if (!_subscriptions.TryGetValue(typeof(T), out handlers))
            {
                handlers = new List<IMessageHandler>();
            }

            if (!handlers.Any(x => x.IsSame(subscriber)))
            {
                handlers = new List<IMessageHandler>(handlers) {s};
                _subscriptions[typeof(T)] = handlers;
            }
        }

        public void Unsubscribe<T>(IHandle<T> subscriber) where T : Message
        {
            List<IMessageHandler> handlers;

            if (_subscriptions.TryGetValue(typeof(T), out handlers))
            {
                handlers = new List<IMessageHandler>(handlers.Where(x => !x.IsSame(subscriber)));
                _subscriptions[typeof(T)] = handlers;
            }
        }


        private interface IMessageHandler
        {
            void Handle(Message message);

            bool IsSame(object subscriber);
        }

        private class MessageHandler<T> : IMessageHandler where T : Message
        {
            private readonly IHandle<T> _handler;

            public MessageHandler(IHandle<T> handler)
            {
                _handler = handler;
            }

            public void Handle(Message message)
            {
                _handler.Handle((T) message);
            }

            public bool IsSame(object subscriber)
            {
                return ReferenceEquals(_handler, subscriber);
            }
        }
    }


    public class Message
    {
    }

    public interface IHandle<T> where T : Message
    {
        void Handle(T message);
    }

    public interface IPublish
    {
        void Publish(Message message);
    }

    public interface IBus : IPublish
    {
        void Subscribe<T>(IHandle<T> subscriber) where T : Message;

        void Unsubscribe<T>(IHandle<T> subscriber) where T : Message;
    }
}