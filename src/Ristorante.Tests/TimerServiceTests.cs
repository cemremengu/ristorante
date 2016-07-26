using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ristorante.Tests
{
    using Xunit;

    public class TimerServiceTests
    {
        private readonly TimerService _timerService;
        private readonly TestPublisher _testPublisher;
        private DateTimeOffset _now;

        public TimerServiceTests()
        {
            _testPublisher = new TestPublisher();
            _timerService = new TimerService(_testPublisher);
            _now = new DateTimeOffset(2016, 1, 1, 0, 0, 0, 0, TimeSpan.Zero);

            SystemClock.UtcNow = () => _now;
        }


        [Fact]
        public void Immediately_publishes_messages_when_later_than_publish_time()
        {
            var testMessage = new TestMessage();
            var delayedMessage = new DelayedMessage(_now, testMessage);


            _now = _now.AddMilliseconds(1);

            _timerService.Handle(delayedMessage);

            Assert.Contains(testMessage, _testPublisher.Messages);
        }

        [Fact]
        public void Delays_publishing_messages_when_earlier_than_publish_time()
        {
            var testMessage = new TestMessage();
            var delayedMessage = new DelayedMessage(_now.AddMilliseconds(1), testMessage);

            
            _timerService.Handle(delayedMessage);

            Assert.DoesNotContain(testMessage, _testPublisher.Messages);
        }

        [Fact]
        public void Does_not_publish_messages_before_publish_time()
        {
            var testMessage = new TestMessage();
            var delayedMessage = new DelayedMessage(_now.AddMilliseconds(1), testMessage);

            _timerService.Handle(delayedMessage);

            _timerService.TryProcess();

            Assert.DoesNotContain(testMessage, _testPublisher.Messages);
        }

        [Fact]
        public void Does_not_publish_messages_after_publish_time()
        {
            var testMessage = new TestMessage();
            var delayedMessage = new DelayedMessage(_now.AddMilliseconds(1), testMessage);

            _timerService.Handle(delayedMessage);

            _now = _now.AddMilliseconds(10);

            _timerService.TryProcess();

            Assert.Contains(testMessage, _testPublisher.Messages);
        }

        private class TestMessage: Message
        {
            
        }

        private class TestPublisher : IPublish
        {
            public List<Message> Messages = new List<Message>();

            public void Publish(Message message)
            {
                Messages.Add(message);

            }
        }
    }
}
