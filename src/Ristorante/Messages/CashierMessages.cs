namespace Ristorante.Messages
{
    using Infrastructure;

    public class TakePayment : Message
    {
        public readonly Customer Customer;

        public TakePayment(Customer customer)
        {
            Customer = customer;
        }
    }


    public class PaymentTaken : Message
    {
        public readonly DocumentMessage Order;

        public PaymentTaken(DocumentMessage order)
        {
            Order = order;
        }
    }


    public class PaymentReceivable : Message
    {
        public readonly DocumentMessage Order;

        public PaymentReceivable(DocumentMessage order)
        {
            Order = order;
        }
    }
}