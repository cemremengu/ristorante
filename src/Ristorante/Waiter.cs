namespace Ristorante
{
    using System;
    using Infrastructure;
    using Messages;
    using Newtonsoft.Json.Linq;

    public class Waiter : IHandle<DeliverOrder>, IHandle<TakeOrder>
    {
        public Waiter(string name, IPublish publisher)
        {
            Name = name;

            Publisher = publisher;
        }

        private string Name { get; }

        private IPublish Publisher { get; }

        public void Handle(DeliverOrder message)
        {
            var waiterOrder = message.Order.To<WaiterOrder>();

            Console.WriteLine("Order delivered to table number " + waiterOrder.TableNumber);


            Publisher.Publish(new OrderDelivered(message.Order));
        }

        public void Handle(TakeOrder message)
        {
            Console.WriteLine("Taking order from customer...");


            var order = new WaiterOrder(Name, message.Customer.TableNumber)
                .AddOrderItem("Burger", 1)
                .AddOrderItem("Milkshake", 1);


            Publisher.Publish(new OrderTaken(order));
        }


        private class WaiterOrder : DocumentMessage
        {
            public WaiterOrder()
            {
            }

            public WaiterOrder(string waiterName, int tableNumber)
            {
                WaiterName = waiterName;
                TableNumber = tableNumber;

                Items = new JArray();

                json[nameof(Items)] = Items;
            }

            public string WaiterName
            {
                get { return Get(nameof(WaiterName)).Value<string>(); }

                private set { Set(nameof(WaiterName), new JValue(value)); }
            }

            public int TableNumber
            {
                get { return Get(nameof(TableNumber)).Value<int>(); }

                private set { Set(nameof(TableNumber), new JValue(value)); }
            }

            private JArray Items { get; }

            protected override void Validate()
            {
                JToken ignored;
                if (!json.TryGetValue(nameof(TableNumber), out ignored))
                {
                    throw new Exception("Missing TableNumber");
                }
            }

            public WaiterOrder AddOrderItem(string name, int quantity)
            {
                Items.Add(new WaiterOrderItem(name, quantity).AsJson());

                return this;
            }

            private class WaiterOrderItem : DocumentMessage
            {
                public WaiterOrderItem(string name, int quantity)
                {
                    Name = name;
                    Quantity = quantity;
                }

                public string Name
                {
                    get { return Get(nameof(Name)).Value<string>(); }

                    private set { Set(nameof(Name), new JValue(value)); }
                } // waiter

                public int Quantity
                {
                    get { return Get(nameof(Quantity)).Value<int>(); }

                    private set { Set(nameof(Quantity), new JValue(value)); }
                } // waiter, cashier
            }
        }
    }
}