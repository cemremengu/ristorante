namespace Ristorante.Tests
{
    using FakeItEasy;
    using Xunit;

    public class DispatcherTests
    {
        public DispatcherTests()
        {
            _dispatcher = new Dispatcher();
            _subscriberX = A.Fake<IHandle<Message>>();
            _subscriberY = A.Fake<IHandle<Message>>();
            _subscriberZ = A.Fake<IHandle<MessageB>>();

            _messageA = new Message();
            _messageB = new MessageB();
        }

        private readonly IHandle<Message> _subscriberX;
        private readonly Dispatcher _dispatcher;
        private readonly Message _messageA;
        private readonly IHandle<Message> _subscriberY;
        private readonly MessageB _messageB;
        private readonly IHandle<MessageB> _subscriberZ;

        public class MessageB : Message
        {
        }

        [Fact]
        public void Should_check_reference_when_unsubscribing()
        {
            Assert.True(_dispatcher.IsEmpty);

            _dispatcher.Subscribe(_subscriberX);
            _dispatcher.Subscribe(_subscriberY);

            Assert.True(!_dispatcher.IsEmpty);

            _dispatcher.Unsubscribe(_subscriberX);

            Assert.True(_dispatcher.CountSubscribers<Message>() == 1);

            _dispatcher.Unsubscribe(_subscriberY);

            Assert.True(_dispatcher.CountSubscribers<Message>() == 0);
        }

        [Fact]
        public void Should_do_basic_subscribe_and_unsubscribe()
        {
            Assert.True(_dispatcher.IsEmpty);

            _dispatcher.Subscribe(_subscriberX);

            Assert.True(!_dispatcher.IsEmpty);

            _dispatcher.Unsubscribe(_subscriberX);

            Assert.True(_dispatcher.CountSubscribers<Message>() == 0);
        }

        [Fact]
        public void Should_handle_inherited_messages()
        {
            _dispatcher.Subscribe(_subscriberX);
            _dispatcher.Subscribe(_subscriberY);
            _dispatcher.Subscribe(_subscriberZ);

            _dispatcher.Handle(_messageB);

            A.CallTo(() => _subscriberZ.Handle(_messageB)).MustHaveHappened();
            A.CallTo(() => _subscriberX.Handle(_messageB)).MustHaveHappened();
            A.CallTo(() => _subscriberY.Handle(_messageB)).MustHaveHappened();
        }

        [Fact]
        public void Should_handle_messages()
        {
            _dispatcher.Subscribe(_subscriberX);
            _dispatcher.Subscribe(_subscriberY);
            _dispatcher.Subscribe(_subscriberZ);

            _dispatcher.Handle(_messageA);

            A.CallTo(() => _subscriberX.Handle(_messageA)).MustHaveHappened();
            A.CallTo(() => _subscriberY.Handle(_messageA)).MustHaveHappened();
            A.CallTo(() => _subscriberZ.Handle(_messageB)).MustNotHaveHappened();

            _dispatcher.Handle(_messageB);

            A.CallTo(() => _subscriberZ.Handle(_messageB)).MustHaveHappened();
        }
    }
}