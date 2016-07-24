namespace Ristorante.Demo
{
    using System;
    using Messages;

    public class TurkishOrderProcessManager : IRestaurantProcess
    {
        private readonly Guid _customerId;

        public TurkishOrderProcessManager(Guid customerId, IPublish publisher)
        {
            Publisher = publisher;

            _customerId = customerId;
        }

        private IPublish Publisher { get; }

        public void Handle(OrderTaken message)
        {
            Publisher.Publish(new CookOrder(message.Order));
        }

        public void Handle(OrderCooked message)
        {
            Publisher.Publish(new DeliverOrder(message.Order));
        }

        public void Handle(OrderDelivered message)
        {
            Publisher.Publish(new PaymentReceivable(message.Order));

            Publisher.Publish(new ReadyForPayment(_customerId));
        }

        public void Handle(PaymentTaken message)
        {
            Publisher.Publish(new RecordOrder(message.Order));

            Publisher.Publish(new MealCompleted(_customerId));
        }
    }
}