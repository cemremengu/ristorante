namespace Ristorante.Infrastructure
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    public class CompetingConsumer<T> : IHandle<T> where T : Message
    {
        private readonly ConcurrentQueue<T> _queue;

        public CompetingConsumer(IEnumerable<IHandle<T>> handlers)
        {
            _queue = new ConcurrentQueue<T>();

            foreach (var handler in handlers)
            {
               new ThreadBasedProcessor(new QueueProcessor<T>(_queue, handler));
            }
        }

        public void Handle(T message)
        {
            _queue.Enqueue(message);
        }
    }
}