namespace ConsoleApplication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Chef
    {
        private readonly Manager _manager;
        private readonly Dictionary<string, decimal> _prices;

        public Chef(Dictionary<string, decimal> prices, Manager manager)
        {
            _prices = prices;
            _manager = manager;
        }

        public void CookOrder(Order order)
        {
            foreach (var item in order.OrderItems)
            {
                item.Price = _prices[item.Name];
            }

            _manager.RecordCost(order.OrderItems.Sum(x => x.Price*x.Quantity));

            Console.WriteLine("Cooked the order and notified the manager...");
        }
    }
}