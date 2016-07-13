namespace Ristorante
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Cashier
    {
        private readonly Dictionary<string, decimal> _prices;
        private readonly Manager _manager;

        public Cashier(Dictionary<string, decimal> prices, Manager manager)
        {
            _prices = prices;
            _manager = manager;
        }

        private Dictionary<int, Order> pendingPayments = new Dictionary<int, Order>();

        public void TakePayment(Customer customer)
        {
            var order = pendingPayments[customer.TableNumber];

            var total = order.OrderItems.Sum(x => x.Cost*x.Quantity);

            Console.WriteLine($"Charging the customer on table {order.TableNumber}. Total ${total}");

            _manager.RecordPayment(total);

        }

        public void ReadyForPayment(Order order)
        {
            foreach (var item in order.OrderItems)
            {
                item.Cost = _prices[item.Name];
            }

            pendingPayments.Add(order.TableNumber, order);
        }
    }
}