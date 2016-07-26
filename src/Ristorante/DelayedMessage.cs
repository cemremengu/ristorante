namespace Ristorante
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    public static class SystemClock
    {
        public static Func<DateTimeOffset> UtcNow = () => DateTimeOffset.UtcNow;
    }

    public class DelayedMessage : Message
    {
        public DelayedMessage(DateTimeOffset when, Message toPublish)
        {
            When = when;
            ToPublish = toPublish;
        }

        public DelayedMessage(TimeSpan when, Message toPublish) : this(SystemClock.UtcNow().Add(when), toPublish)
        {
        }

        public DateTimeOffset When { get; }
        public Message ToPublish { get; }
    }

    public class TimerService: IHandle<DelayedMessage>, IProcessor
    {
        private readonly IPublish _publisher;
        private ConcurrentDictionary<DateTimeOffset, IReadOnlyList<DelayedMessage>> _pending;

        public TimerService(IPublish publisher)
        {
            _publisher = publisher;
            _pending = new ConcurrentDictionary<DateTimeOffset, IReadOnlyList<DelayedMessage>>();
        }

        public void Handle(DelayedMessage message)
        {
            if (message.When <= SystemClock.UtcNow())
            {
                _publisher.Publish(message.ToPublish);

                return;
            }

            _pending.AddOrUpdate(
                message.When,
                new List<DelayedMessage>() {message},
                (_, current) => new List<DelayedMessage>(current) {message});

        }

        public bool TryProcess()
        {
            bool processed = false;

            foreach (var key in _pending.Keys.Where(x => x <= SystemClock.UtcNow()))
            {
                IReadOnlyList<DelayedMessage> pending;

                if (_pending.TryRemove(key, out pending))
                {
                    processed = true;

                    foreach (var p in pending)
                    {
                        _publisher.Publish(p.ToPublish);
                    }
                }
            }
            return processed;
            
        }
    }
}