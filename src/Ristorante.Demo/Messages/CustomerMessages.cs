namespace Ristorante.Demo.Messages
{
    using System;

    public class ReadyForPayment : CorrelatedMessage
    {
        public ReadyForPayment(Guid correlationId) : base(correlationId)
        {
        }
    }

    public class MealCompleted : CorrelatedMessage
    {
        public int TableNumber { get; }

        public MealCompleted(Guid correlationId, int tableNumber) : base(correlationId)
        {
            TableNumber = tableNumber;
        }

    }

    public class FreeDrink : CorrelatedMessage
    {
        public FreeDrink(Guid correlationId) : base(correlationId)
        {
        }
    }
}