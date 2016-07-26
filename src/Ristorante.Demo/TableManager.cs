namespace Ristorante.Demo
{
    using System;
    using System.Collections.Generic;
    using Messages;
    using Newtonsoft.Json.Linq;

    public class TableManager : IRestaurantProcess
    {
        private readonly IBus _bus;
        private readonly Func<Guid, int, IPublish , IRestaurantProcess> _processFactory;
        private readonly Dictionary<int, IRestaurantProcess> _tables;

        public TableManager(IBus bus, Func<Guid, int, IPublish, IRestaurantProcess> processFactory)
        {
            _bus = bus;
            _processFactory = processFactory;

            _tables = new Dictionary<int, IRestaurantProcess>();
        }


        public void Handle(OrderDelivered message)
        {
            var tableId = GetTableId(message.Order);

            _tables[tableId].Handle(message);
        }

        public void Handle(OrderTaken message)
        {
            var tableId = GetTableId(message.Order);

            var process = _processFactory(message.CustomerId, tableId, _bus);


            _tables.Add(tableId, process);

            process.Handle(message);
        }

        public void Handle(OrderCooked message)
        {
            var tableId = GetTableId(message.Order);

            _tables[tableId].Handle(message);
        }

        public void Handle(PaymentTaken message)
        {
            var tableId = GetTableId(message.Order);

            _tables[tableId].Handle(message);
        }

        public void Handle(MealCompleted message)
        {
            _tables.Remove(message.TableNumber);
        }

        private int GetTableId(DocumentMessage document)
        {
            return document.To<TableOrder>().TableNumber;
        }

        private class TableOrder : DocumentMessage
        {
            public int TableNumber => Get(nameof(TableNumber)).Value<int>();
        }

        public void Handle(FreeDrinkTimeout message)
        {
            var tableId = message.TableNumber;

            IRestaurantProcess process;

            if (_tables.TryGetValue(tableId, out process))
            {
                process.Handle(message);
            }

        }
    }
}