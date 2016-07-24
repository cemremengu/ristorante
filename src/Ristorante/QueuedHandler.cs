namespace Ristorante
{
    using System.Collections.Concurrent;

    public class QueuedHandler<T> : IHandle<T>, IProcessor where T : Message
    {
        private readonly QueueProcessor<T> _processor;

        private readonly ConcurrentQueue<T> _queue;

        public QueuedHandler(IHandle<T> next)
        {
            _queue = new ConcurrentQueue<T>();

            _processor = new QueueProcessor<T>(_queue, next);
        }

        public int Length => _queue.Count;


        public void Handle(T message)
        {
            _queue.Enqueue(message);
        }

        public bool TryProcess()
        {
            return _processor.TryProcess();
        }
    }

    public class QueueProcessor<T> : IProcessor where T : Message
    {
        private readonly IHandle<T> _next;
        private readonly ConcurrentQueue<T> _queue;

        public QueueProcessor(ConcurrentQueue<T> queue, IHandle<T> next)
        {
            _queue = queue;
            _next = next;
        }

        public bool TryProcess()
        {
            T message;
            if (_queue.TryDequeue(out message))
            {
                _next.Handle(message);
                return true;
            }

            return false;
        }
    }
}