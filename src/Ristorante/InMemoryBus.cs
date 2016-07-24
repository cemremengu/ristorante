namespace Ristorante
{
    using System;

    public class InMemoryBus : IBus
    {
        private readonly CorrelationManager _correlationManager;
        private readonly Dispatcher _dispatcher;
        private readonly IHandle<Message> _handler;

        public InMemoryBus()
        {
            _dispatcher = new Dispatcher();

            var queue = new QueuedHandler<Message>(_dispatcher);

            _handler = queue;

            new ThreadBasedProcessor(queue);


            _correlationManager = new CorrelationManager();

            _dispatcher.Subscribe(_correlationManager);
        }


        public void Publish(Message message)
        {
            _handler.Handle(message);
        }

        public void Subscribe<T>(IHandle<T> subscriber) where T : Message
        {
            _dispatcher.Subscribe(subscriber);
        }

        public void Subscribe<T>(Guid correlationId, IHandle<T> subscriber) where T : Message
        {
            _correlationManager.Subscribe(correlationId, subscriber);
        }

        public void Unsubscribe<T>(IHandle<T> subscriber) where T : Message
        {
            _dispatcher.Unsubscribe(subscriber);
        }

        public void Unsubscribe<T>(Guid correlationId, IHandle<T> subscriber) where T : Message
        {
            _correlationManager.Unsubscribe(correlationId, subscriber);
        }
    }
}