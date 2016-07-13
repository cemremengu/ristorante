namespace ConsoleApplication
{
    using System.Collections;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class Order
    {
        public Order(string waiterName, int tableNumber)
        {
            WaiterName = waiterName;
            TableNumber = tableNumber;
        }

        public string WaiterName { get; } // waiter

        public int TableNumber { get; } // waiter, cashier

        public List<OrderItem> OrderItems { get; set; } // waiter, cashier, chef
    }

    public class WaiterOrder : OrderDocument
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
        } // waiter

        public int TableNumber
        {
            get { return Get(nameof(TableNumber)).Value<int>(); }

            private set { Set(nameof(TableNumber), new JValue(value)); }
        } // waiter, cashier

        private JArray Items { get; }

        public void AddOrderItem(string name, int quantity)
        {
            Items.Add(new WaiterOrderItem(name, quantity).AsJson());
        }

        private class WaiterOrderItem : OrderDocument
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

    public class ChefOrder : OrderDocument, IEnumerable<ChefOrderItem>
    {
        public IEnumerator<ChefOrderItem> GetEnumerator()
        {
            var items = (JArray) json["Items"];

            foreach (JObject item in items)
            {
                yield return FromJson<ChefOrderItem>(item);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<ChefOrderItem>) this).GetEnumerator();
        }
    }

    public class ChefOrderItem : OrderDocument
    {
        public decimal Price
        {
            get { return Get(nameof(Price)).Value<decimal>(); }

            set { Set(nameof(Price), new JValue(value)); }
        } // waiter, cashier
    }


    public class OrderDocument
    {
        protected JObject json;

        public OrderDocument()
        {
            json = new JObject();
        }

        public JObject AsJson()
        {
            return JObject.Parse(json.ToString()); // prevent people from fiddling with json object.
        }

        public T Get<T>(string name) where T : OrderDocument, new()
        {
            return FromJson<T>((JObject) json[name]);
        }

        public void Set<T>(string name, T value) where T : OrderDocument
        {
            json[name] = value.AsJson();
        }

        public JValue Get(string name)
        {
            return (JValue) json[name];
        }

        public void Set(string name, JValue value)
        {
            json[name] = value;
        }


        public static T FromJson<T>(JObject json) where T : OrderDocument, new()
        {
            return new T { json = json }; ;
        }

        public override string ToString()
        {
            return json.ToString(Formatting.Indented);
        }
    }


    public class OrderItem
    {
        public OrderItem(string name, int quantity)
        {
            Name = name;
            Quantity = quantity;
        }

        public decimal Cost { get; set; } // cashier

        public string Name { get; } // waiter, chef, cashier

        public int Quantity { get; } // waiter, chef, cashier

        public decimal Price { get; set; } // chef
    }
}