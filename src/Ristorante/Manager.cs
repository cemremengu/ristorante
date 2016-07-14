namespace Ristorante
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json.Linq;

    public class Manager
    {
        private readonly List<ManagerOrder> dailyOrders = new List<ManagerOrder>();

        public void ReportProfit()
        {
            var dailyTotal = 0m;

            var totalCost = 0m;

            foreach (var item in dailyOrders.SelectMany(x => x))
            {
                totalCost += item.Cost*item.Quantity;

                dailyTotal += item.Price*item.Quantity;
            }
            Console.WriteLine(
                $"Generating a report...Payments received ${dailyTotal}, Total cost ${totalCost}, Total profit ${dailyTotal - totalCost}");
        }


        public void RecordOrder(DocumentMessage order)
        {
            dailyOrders.Add(order.To<ManagerOrder>());
        }

        private class ManagerOrder : DocumentMessage, IEnumerable<ManagerOrderItem>
        {
            public ManagerOrder()
            {
                Items = new JArray();
            }

            private JArray Items { get; }

            public IEnumerator<ManagerOrderItem> GetEnumerator()
            {
                var items = (JArray) json[nameof(Items)];

                foreach (JObject item in items)
                {
                    yield return FromJson<ManagerOrderItem>(item);
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private class ManagerOrderItem : DocumentMessage
        {
            public decimal Cost => Get("Price").Value<decimal>();

            public decimal Price => Get("Cost").Value<decimal>();

            public int Quantity => Get(nameof(Quantity)).Value<int>();
        }
    }
}