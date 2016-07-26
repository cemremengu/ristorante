namespace Ristorante.Demo
{
    using System;
    using Messages;

    public class Customer :
        IHandle<OrderDelivered>,
        IHandle<ReadyForPayment>,
        IHandle<MealCompleted>,
        IHandle<FreeDrink>
    {
        private readonly IBus _publisher;

        public Customer(int tableNumber, IBus publisher)
        {
            _publisher = publisher;
            TableNumber = tableNumber;

            Id = Guid.NewGuid();
        }

        public int TableNumber { get; }

        public Guid Id { get; }

        public void Handle(MealCompleted message)
        {
            _publisher.Unsubscribe<OrderDelivered>(Id, this);
            _publisher.Unsubscribe<ReadyForPayment>(Id, this);
            _publisher.Unsubscribe<MealCompleted>(Id, this);
            _publisher.Unsubscribe<FreeDrink>(Id, this);

        }

        public void Handle(OrderDelivered message)
        {
            Console.WriteLine("Nom nom nom...");
        }

        public void Handle(ReadyForPayment message)
        {
            _publisher.Publish(new TakePayment(this));
        }

        public void EnterRestaurant()
        {
            _publisher.Subscribe<OrderDelivered>(Id, this);
            _publisher.Subscribe<ReadyForPayment>(Id, this);
            _publisher.Subscribe<MealCompleted>(Id, this);
            _publisher.Subscribe<FreeDrink>(Id, this);

            _publisher.Publish(new TakeOrder(this));
        }

        public void Handle(FreeDrink message)
        {
            Console.WriteLine("Enjoying a free drink!!");
        }
    }
}