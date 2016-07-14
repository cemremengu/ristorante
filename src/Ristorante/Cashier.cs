namespace Ristorante
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json.Linq;

    public class Cashier
    {
        private readonly Dictionary<string, decimal> _prices;
        private readonly Manager _manager;

        public Cashier(Dictionary<string, decimal> prices, Manager manager)
        {
            _prices = prices;
            _manager = manager;
        }

        private Dictionary<int, CashierOrder> pendingPayments = new Dictionary<int, CashierOrder>();

        public void TakePayment(Customer customer)
        {
            var order = pendingPayments[customer.TableNumber];

            var total = order.Sum(x => x.Cost*x.Quantity);

            Console.WriteLine($"Charging the customer on table {order.TableNumber}. Total ${total}");

            _manager.RecordPayment(total);

        }

        public void ReadyForPayment(DocumentMessage order)
        {
            var co = order.To<CashierOrder>();

            foreach (var item in co)
            {
                item.Cost = _prices[item.Name];
            }

            pendingPayments.Add(co.TableNumber, co);
        }

        private class CashierOrder : DocumentMessage, IEnumerable<CashierOrderItem>
        {
            public CashierOrder()
            {
                Items = new JArray();
            }

            private JArray Items { get; }

            public int TableNumber => Get(nameof(TableNumber)).Value<int>();

            public IEnumerator<CashierOrderItem> GetEnumerator()
            {
                var items = (JArray)json[nameof(Items)];

                foreach (JObject item in items)
                {
                    yield return FromJson<CashierOrderItem>(item);
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }


        private class CashierOrderItem : DocumentMessage
        {


            public string Name => Get(nameof(Name)).Value<string>();

            public int Quantity => Get(nameof(Quantity)).Value<int>();


            public decimal Cost
            {
                get { return Get(nameof(Cost)).Value<decimal>(); }

                set { Set(nameof(Cost), new JValue(value)); }
            }
        }
    }
}