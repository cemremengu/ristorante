namespace Ristorante.Infrastructure
{
    using System;

    public class PendingWorkLoadBalancer<T> : IHandle<T> where T : Message
    {
        private readonly QueuedHandler<T>[] _handlers;
        private readonly IPublish _publisher;


        public PendingWorkLoadBalancer(IPublish publisher, QueuedHandler<T>[] handlers)
        {
            _publisher = publisher;
            _handlers = handlers;
        }

        public void Handle(T message)
        {
            foreach (var handler in _handlers)
            {
                if (handler.Length == 0)
                {
                    handler.Handle(message);

                    return;
                }
            }

            Console.WriteLine("Retrying...");
            _publisher.Publish(message);
        }
    }
}