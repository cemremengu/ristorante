namespace Ristorante.Demo
{
    using System;
    using System.Collections.Generic;
    using Messages;
    using Newtonsoft.Json.Linq;

    public class TableManager : IRestaurantProcess
    {
        private readonly IBus _bus;
        private readonly Func<Guid, IPublish, IRestaurantProcess> _processFactory;
        private readonly Dictionary<int, IRestaurantProcess> _tables;

        public TableManager(IBus bus, Func<Guid, IPublish, IRestaurantProcess> processFactory)
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
            var process = _processFactory(message.CustomerId, _bus);

            var tableId = GetTableId(message.Order);

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

        private int GetTableId(DocumentMessage document)
        {
            return document.To<TableOrder>().TableNumber;
        }

        private class TableOrder : DocumentMessage
        {
            public int TableNumber => Get(nameof(TableNumber)).Value<int>();
        }
    }
}