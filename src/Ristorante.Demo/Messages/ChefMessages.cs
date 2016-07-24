namespace Ristorante.Demo.Messages
{
    public class CookOrder : Message
    {
        public readonly DocumentMessage Order;

        public CookOrder(DocumentMessage order)
        {
            Order = order;
        }
    }

    public class OrderCooked : Message
    {
        public readonly DocumentMessage Order;

        public OrderCooked(DocumentMessage order)
        {
            Order = order;
        }
    }
}