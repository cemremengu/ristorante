namespace Ristorante.Demo
{
    using System;
    using System.Collections.Generic;
    using Messages;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var turkishBus = new InMemoryBus();
            var zimbabweBus = new InMemoryBus();

            Configure(turkishBus, new TableManager(
                turkishBus,
                (customerId, tableNumber, publisher) => new TurkishOrderProcessManager(customerId, tableNumber, publisher)));

            Configure(zimbabweBus, new TableManager(
                zimbabweBus,
                (customerId, tableNumber, publisher) => new ZimbabweProcessManager(customerId, tableNumber, publisher)));

            OpenRestaurant(turkishBus);
            OpenRestaurant(zimbabweBus);


            Console.ReadLine();
        }

        private static void OpenRestaurant(InMemoryBus bus)
        {
            var i = 1;
            for (; i <= 10; i++)
            {
                var customer = new Customer(i, bus);

                customer.EnterRestaurant();
            }
            Console.ReadLine();
            bus.Publish(new ReportProfit());
        }

        private static void Configure(IBus bus, IRestaurantProcess processManager)
        {
            var timerService = new TimerService(bus);

            bus.Subscribe(timerService);

            new ThreadBasedProcessor(timerService);

            var prices = new Dictionary<string, decimal> {{"Burger", 10.0m}, {"Milkshake", 5.0m}};
            var costs = new Dictionary<string, decimal> {{"Burger", 2.75m}, {"Milkshake", 1.25m}};

            var manager = new Manager();

            var chefs = new[]
            {
                new Chef(costs, 500, bus), new Chef(costs, 1500, bus), new Chef(costs, 1000, bus),
                new Chef(costs, 700, bus)
            };

            var kitchen = new Kitchen(bus, chefs);

            var cashier = new Cashier(prices, bus);


            var waiter = new Waiter("Cemre", bus);

            bus.Subscribe<DeliverOrder>(waiter);
            bus.Subscribe<TakeOrder>(waiter);

            bus.Subscribe(kitchen);

            bus.Subscribe<PaymentReceivable>(cashier);
            bus.Subscribe<TakePayment>(cashier);


            bus.Subscribe<RecordOrder>(manager);
            bus.Subscribe<ReportProfit>(manager);

            bus.Subscribe<OrderDelivered>(processManager);
            bus.Subscribe<OrderTaken>(processManager);
            bus.Subscribe<OrderCooked>(processManager);
            bus.Subscribe<PaymentTaken>(processManager);
            bus.Subscribe<FreeDrinkTimeout>(processManager);

        }
    }
}