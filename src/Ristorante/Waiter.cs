namespace Ristorante
{
    using System;
    using System.Collections.Generic;

    public class Waiter
    {
        private string Name { get; }

        private Cashier Cashier { get; }

        public Waiter(string name, Cashier cashier)
        {
            Name = name;

            Cashier = cashier;
        }

        public Order TakeOrder(Customer customer)
        {
            Console.WriteLine("Taking order from customer...");

            return new Order(Name, customer.TableNumber)
            {
                OrderItems =
                    new List<OrderItem>
                    {
                        new OrderItem ("Burger", 1),
                        new OrderItem ("Milkshake", 1)
                    }
            
            };
        }

        public void DeliverOrder(Order order)
        {
            Console.WriteLine("Order delivered to table number " + order.TableNumber);

            Cashier.ReadyForPayment(order );

        }
    }
}