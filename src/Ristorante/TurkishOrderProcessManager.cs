namespace Ristorante
{
    using Infrastructure;
    using Messages;

    public class TurkishOrderProcessManager : IRestaurantProcess
    {
        public TurkishOrderProcessManager(IPublish publisher)
        {
            Publisher = publisher;
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

            Publisher.Publish(new ReadyForPayment());
        }

        public void Handle(PaymentTaken message)
        {
            Publisher.Publish(new RecordOrder(message.Order));

            Publisher.Publish(new MealCompleted());
        }
    }
}