namespace Ristorante.Demo.Messages
{
    using System;

    public class FreeDrinkTimeout : Message
    {
        public FreeDrinkTimeout(Guid timeoutId, int tableNumber)
        {
            TimeoutId = timeoutId;
            TableNumber = tableNumber;
        }

        public Guid TimeoutId { get; }
        public int TableNumber { get; }
    }
}