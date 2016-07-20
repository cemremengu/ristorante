namespace Ristorante.Messages
{
    using Infrastructure;
    using System;

    public class DeliverOrder : Message
    {
        public readonly DocumentMessage Order;

        public DeliverOrder(DocumentMessage order)
        {
            Order = order;
        }
    }

    public class TakeOrder : Message
    {
        public readonly Customer Customer;

        public TakeOrder(Customer customer)
        {
            Customer = customer;
        }
    }

    public class OrderDelivered : Message
    {
        public readonly DocumentMessage Order;

        public OrderDelivered(DocumentMessage order)
        {
            Order = order;
        }
    }

    public class OrderTaken : Message
    {
        public readonly Guid CustomerId;
        public readonly DocumentMessage Order;

        public OrderTaken(Guid customerId, DocumentMessage order)
        {
            CustomerId = customerId;
            Order = order;
        }
    }
}