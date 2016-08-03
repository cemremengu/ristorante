namespace Ristorante.Tests
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json.Linq;
    using Xunit;

    public class Spikes
    {
        [Theory]
        [InlineData(0)]
        [InlineData(100)]

        public void Stock_level_can_be_initialized(int quantity)
        {
            var stockLevel = StockLevel.Initialize(quantity);

            Then(stockLevel, new StockLevelInitialized(quantity));
        }

        private static void Then(StockLevel stockLevel, params Event[] expected)
        {
            var actual = stockLevel.GetPendingEvents();


            Assert.Equal(expected.Length, actual.Count);

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.Equal(JObject.FromObject(expected[i]).ToString(), JObject.FromObject(actual[i]).ToString());
            }
        }


        [Theory]
        [InlineData(0, 5)]
        [InlineData(0, 10)]
        [InlineData(5, 5)]
        public void Adding_stock(int initialLevel, int quantity)
        {
            var stockLevel = Given(new StockLevelInitialized(initialLevel));

            stockLevel.Add(quantity);

            Then(stockLevel, new StockAdded(quantity));

        }

        private static StockLevel Given(params Event[] e)
        {
            var stockLevel = (StockLevel) Activator.CreateInstance(typeof(StockLevel), true);

            Array.ForEach(e, stockLevel.Apply);

            return stockLevel;
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Add_quantity_must_be_greater_than_zero(int quantity)
        {
            var stockLevel = Given(new StockLevelInitialized(0));

            var ex = Record.Exception(() => stockLevel.Add(quantity) );

            var ae = Assert.IsType<ArgumentOutOfRangeException>(ex);

            Assert.Equal("quantity", ae.ParamName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Remove_quantity_must_be_greater_than_zero(int quantity)
        {
            var stockLevel = StockLevel.Initialize(0);

            var ex = Record.Exception(() => stockLevel.Remove(quantity) );

            var ae = Assert.IsType<ArgumentOutOfRangeException>(ex);

            Assert.Equal("quantity", ae.ParamName);
        }

        [Theory]
        [InlineData(10, 5, 5)]
        [InlineData(10, 6, 4)]
        [InlineData(10, 2, 8)]
        public void Removing_stock(int initialLevel, int quantity, int expected)
        {
            var stockLevel = Given(new StockLevelInitialized(initialLevel));

            stockLevel.Remove(quantity);

            Then(stockLevel, new StockRemoved(quantity));
        }


        [Theory]
        [InlineData(0, 1)]
        [InlineData(0, 2)]
        public void Cannot_remove_more_stock_than_current_level(int initialLevel, int quantity)
        {
            var stockLevel = StockLevel.Initialize(initialLevel);

            var ex = Record.Exception(() => { stockLevel.Remove(quantity); });

            Assert.IsType<StockOverdrawn>(ex);
        }

        [Fact]
        public void Sequence_exploration()
        {
            var stockLevel = Given(
                new StockLevelInitialized(10), 
                new StockRemoved(5), 
                new StockAdded(3), 
                new StockRemoved(5), 
                new StockAdded(3), 
                new StockRemoved(5), 
                new StockAdded(3));

            var ex = Record.Exception(() => { stockLevel.Remove(5); });

            Assert.IsType<StockOverdrawn>(ex);
        }

        public class StockLevel
        {
            private static Dictionary<Type, Action<StockLevel, Event>> _dispatchers = new Dictionary
                <Type, Action<StockLevel, Event>>()
            {
                [typeof(StockLevelInitialized)] = (level, @event) => level.Apply((StockLevelInitialized) @event),
                [typeof(StockAdded)] = (level, @event) => level.Apply((StockAdded) @event),
                [typeof(StockRemoved)] = (level, @event) => level.Apply((StockRemoved) @event)
            };

            private List<Event> _pendingEvents;

            

            private StockLevel()
            {
                _pendingEvents = new List<Event>();
            }

            private void Append(Event @event)
            {
                Apply(@event);

                _pendingEvents.Add(@event);
            }

            public void Apply(Event @event)
            {
                Action<StockLevel, Event> dispatcher;

                if (_dispatchers.TryGetValue(@event.GetType(), out dispatcher))
                {
                    dispatcher(this, @event);
                }
            }

            public IReadOnlyList<Event> GetPendingEvents()
            {
                var result = _pendingEvents.ToArray();

                _pendingEvents.Clear();


                return result;
            }

            private StockLevel(int quantity):this()
            {
                Append(new StockLevelInitialized(quantity));
            }

            private void Apply(StockLevelInitialized @event)
            {

                Quantity = @event.Quantity;
            }

   

            private int Quantity { get; set; }

            public static StockLevel Initialize(int quantity)
            {
                return new StockLevel(quantity);
            }

            public void Add(int quantity)
            {
                if (quantity <= 0)
                    throw new ArgumentOutOfRangeException(nameof(quantity));

                Append(new StockAdded(quantity));
            }

            private void Apply(StockAdded @event)
            {
                Quantity += @event.Quantity;
            }

            public void Remove(int quantity)
            {
                if (quantity <= 0)
                    throw new ArgumentOutOfRangeException(nameof(quantity));

                if (quantity > Quantity)
                    throw new StockOverdrawn();

                Append(new StockRemoved(quantity));
            }

            private void Apply(StockRemoved @event)
            {
                Quantity -= @event.Quantity;
            }


        }

        public class StockLevelInitialized : Event
        {
            public StockLevelInitialized(int quantity)
            {
                Quantity = quantity;
            }

            public int Quantity { get; }
        }

        public class StockAdded : Event
        {
            public StockAdded(int quantity)
            {
                Quantity = quantity;
            }

            public int Quantity { get; }

        }

        public class StockRemoved: Event
        {
            public StockRemoved(int quantity)
            {
                Quantity = quantity;
            }

            public int Quantity { get; }

        }

        public abstract class Event
        {
            
        }

        public class StockOverdrawn : Exception
        {
        }
    }
}