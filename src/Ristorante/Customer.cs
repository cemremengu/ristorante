namespace Ristorante
{
    using System;
    using Infrastructure;
    using Messages;

    public class Customer :
        IHandle<OrderDelivered>,
        IHandle<ReadyForPayment>,
        IHandle<MealCompleted>
    {
        private readonly IBus _publisher;

        public Customer(int tableNumber, IBus publisher)
        {
            _publisher = publisher;
            TableNumber = tableNumber;
        }

        public int TableNumber { get; }

        public void Handle(MealCompleted message)
        {
            _publisher.Unsubscribe<OrderDelivered>(this);
            _publisher.Unsubscribe<ReadyForPayment>(this);
            _publisher.Unsubscribe<MealCompleted>(this);
        }

        public void Handle(OrderDelivered message)
        {
            //_publisher.Publish(new TakePayment(this));

            Console.WriteLine("Nom nom nom...");
        }

        public void Handle(ReadyForPayment message)
        {
            _publisher.Publish(new TakePayment(this));
        }

        public void EnterRestaurant()
        {
            _publisher.Subscribe<OrderDelivered>(this);
            _publisher.Subscribe<ReadyForPayment>(this);
            _publisher.Subscribe<MealCompleted>(this);

            _publisher.Publish(new TakeOrder(this));
        }
    }
}