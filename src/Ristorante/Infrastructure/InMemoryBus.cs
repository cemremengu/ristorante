namespace Ristorante.Infrastructure
{
    public class InMemoryBus : IBus
    {
        private readonly Dispatcher _dispatcher;
        private readonly IHandle<Message> _handler;

        public InMemoryBus()
        {
            _handler = _dispatcher = new Dispatcher();
        }

        public void Publish(Message message)
        {
            _handler.Handle(message);
        }

        public void Subscribe<T>(IHandle<T> subscriber) where T : Message
        {
            _dispatcher.Subscribe(subscriber);
        }

        public void Unsubscribe<T>(IHandle<T> subscriber) where T : Message
        {
            _dispatcher.Unsubscribe(subscriber);
        }
    }
}