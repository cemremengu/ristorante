namespace Ristorante.Demo
{
    using System;
    using Messages;

    public class ZimbabweProcessManager : IRestaurantProcess
    {
        private readonly Guid _customerId;
        private readonly int _tableNumber;

        public ZimbabweProcessManager(Guid customerId, int tableNumber, IPublish publisher)
        {
            Publisher = publisher;

            _customerId = customerId;
            _tableNumber = tableNumber;
        }

        private IPublish Publisher { get; }

        public void Handle(OrderTaken message)
        {
            Publisher.Publish(new PaymentReceivable(message.Order));

            Publisher.Publish(new ReadyForPayment(_customerId));
        }

        public void Handle(OrderCooked message)
        {
            Publisher.Publish(new RecordOrder(message.Order));

            Publisher.Publish(new DeliverOrder(message.Order));
        }

        public void Handle(OrderDelivered message)
        {
            Publisher.Publish(new MealCompleted(_customerId, _tableNumber));
        }

        public void Handle(PaymentTaken message)
        {
            Publisher.Publish(new CookOrder(message.Order));
        }

        public void Handle(FreeDrinkTimeout message)
        {
            throw new NotImplementedException();
        }
    }
}