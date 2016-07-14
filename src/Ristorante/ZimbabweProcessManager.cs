namespace Ristorante
{
    using Infrastructure;
    using Messages;

    public class ZimbabweProcessManager : IRestaurantProcess
    {
        public ZimbabweProcessManager(IPublish publisher)
        {
            Publisher = publisher;
        }

        private IPublish Publisher { get; }

        public void Handle(OrderTaken message)
        {
            Publisher.Publish(new PaymentReceivable(message.Order));

            Publisher.Publish(new ReadyForPayment());
        }

        public void Handle(OrderCooked message)
        {
            Publisher.Publish(new RecordOrder(message.Order));

            Publisher.Publish(new DeliverOrder(message.Order));
        }

        public void Handle(OrderDelivered message)
        {
            Publisher.Publish(new MealCompleted());
        }

        public void Handle(PaymentTaken message)
        {
            Publisher.Publish(new CookOrder(message.Order));
        }
    }
}