namespace Ristorante.Messages
{
    using System;
    using Infrastructure;

    public class ReadyForPayment : CorrelatedMessage
    {
        public ReadyForPayment(Guid correlationId) : base(correlationId)
        {
            
        }
    }

    public class MealCompleted : CorrelatedMessage
    {
        public MealCompleted(Guid correlationId) : base(correlationId)
        {
        }

    }
}