namespace Ristorante
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using Newtonsoft.Json.Linq;

    public class Chef
    {
        private readonly Manager _manager;
        private readonly Dictionary<string, decimal> _prices;

        public Chef(Dictionary<string, decimal> prices, Manager manager)
        {
            _prices = prices;
            _manager = manager;
        }

        public void CookOrder(DocumentMessage order)
        {
            Thread.Sleep(500);

            var totalCost = 0m;

            foreach (var item in order.To<ChefOrder>())
            {
                item.Price = _prices[item.Name];

                totalCost += item.Price*item.Quantity;
            }

            _manager.RecordCost(totalCost);

            Console.WriteLine("Cooked the order and notified the manager...");
        }


        private class ChefOrderItem : DocumentMessage
        {
            public string Name
            {
                get { return Get(nameof(Name)).Value<string>(); }

                private set { Set(nameof(Name), new JValue(value)); }
            }

            public int Quantity
            {
                get { return Get(nameof(Quantity)).Value<int>(); }

                private set { Set(nameof(Quantity), new JValue(value)); }
            }


            public decimal Price
            {
                get { return Get(nameof(Price)).Value<decimal>(); }

                set { Set(nameof(Price), new JValue(value)); }
            }
        }

        private class ChefOrder : DocumentMessage, IEnumerable<ChefOrderItem>
        {
            public ChefOrder()
            {
                Items = new JArray();
            }

            private JArray Items { get; }

            public IEnumerator<ChefOrderItem> GetEnumerator()
            {
                var items = (JArray) json[nameof(Items)];

                foreach (JObject item in items)
                {
                    yield return FromJson<ChefOrderItem>(item);
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}