namespace Ristorante
{
    using System;
    using System.Collections.Generic;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var prices = new Dictionary<string, decimal> { { "Burger", 10.0m }, { "Milkshake", 5.0m } };
            var costs = new Dictionary<string, decimal> { { "Burger", 2.75m }, { "Milkshake", 1.25m } };

            var manager = new Manager();

            var chef = new Chef(costs);

            var cashier = new Cashier(prices);

            var waiter = new Waiter("Cemre", cashier);

            for (var i = 1; i <= 10; i++)
            {
                var customer = new Customer(i);

                var order = waiter.TakeOrder(customer);

                chef.CookOrder(order);

                waiter.DeliverOrder(order);

                cashier.TakePayment(customer);

                manager.RecordOrder(order);
            }

            manager.ReportProfit();


            Console.ReadLine();
        }
    }
}