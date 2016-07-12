namespace ConsoleApplication
{
    using System;
    using System.Collections.Generic;

    internal class Program
    {
        private static void Main(string[] args)
        {
            //var prices = new Dictionary<string, decimal> {{"Burger", 10.0m}, {"Milkshake", 5.0m}};
            //var costs = new Dictionary<string, decimal> {{"Burger", 2.75m}, {"Milkshake", 1.25m}};

            //var manager = new Manager();

            //var chef = new Chef(costs, manager);

            //var cashier = new Cashier(prices, manager);

            //var waiter = new Waiter("Cemre", cashier);

            //for (var i = 1; i <= 10; i++)
            //{
            //    var customer = new Customer(i);

            //    var order = waiter.TakeOrder(customer);

            //    chef.CookOrder(order);

            //    waiter.DeliverOrder(order);

            //    cashier.TakePayment(customer);
            //}

            //manager.ReportProfit();

            var wo = new WaiterOrder("Cemre", 1);

            wo.AddOrderItem("test", 1);
            wo.AddOrderItem("test2", 2);

            Console.WriteLine(wo);

            var co = OrderDocument.FromJson<ChefOrder>(wo.AsJson());

            foreach (var item in co)
            {
                item.Price = 2.75m;
            }

            Console.WriteLine(co);

            wo = OrderDocument.FromJson<WaiterOrder>(co.AsJson());

            Console.WriteLine(wo);

            Console.ReadLine();
        }
    }
}