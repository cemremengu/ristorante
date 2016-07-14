namespace Ristorante.Messages
{
    using Infrastructure;

    public class RecordOrder : Message
    {
        public readonly DocumentMessage Order;

        public RecordOrder(DocumentMessage order)
        {
            Order = order;
        }
    }

    public class ReportProfit : Message
    {
    }
}