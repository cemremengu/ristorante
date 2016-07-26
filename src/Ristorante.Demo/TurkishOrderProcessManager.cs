namespace Ristorante.Demo
{
    using System;
    using Messages;

    public class TurkishOrderProcessManager : IRestaurantProcess
    {
        private readonly Guid _customerId;
        private readonly int _tableNumber;
        private Guid _timeoutId;

        public TurkishOrderProcessManager(Guid customerId, int tableNumber, IPublish publisher)
        {
            Publisher = publisher;

            _customerId = customerId;
            _tableNumber = tableNumber;
        }

        private IPublish Publisher { get; }

        public void Handle(OrderTaken message)
        {
            _timeoutId = Guid.NewGuid();

            Publisher.Publish(new CookOrder(message.Order));

            Publisher.Publish(new DelayedMessage(TimeSpan.FromSeconds(1), new FreeDrinkTimeout(_timeoutId, _tableNumber)));
        }

        public void Handle(OrderCooked message)
        {
            _timeoutId = Guid.Empty;

            Publisher.Publish(new DeliverOrder(message.Order));
        }

        public void Handle(FreeDrinkTimeout message)
        {
            if (message.TimeoutId != _timeoutId)
                return;

            _timeoutId = Guid.NewGuid();

            Publisher.Publish(new FreeDrink(_customerId));

            Publisher.Publish(new DelayedMessage(TimeSpan.FromSeconds(1), new FreeDrinkTimeout(_timeoutId, _tableNumber)));

        }

        public void Handle(OrderDelivered message)
        {
            Publisher.Publish(new PaymentReceivable(message.Order));

            Publisher.Publish(new ReadyForPayment(_customerId));
        }

        public void Handle(PaymentTaken message)
        {
            Publisher.Publish(new RecordOrder(message.Order));

            Publisher.Publish(new MealCompleted(_customerId, _tableNumber));
        }
    }
}