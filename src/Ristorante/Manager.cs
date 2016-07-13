namespace Ristorante
{
    using System;

    public class Manager
    {
        private decimal dailyTotal;

        private decimal totalCost;

        public void ReportProfit()
        {
            Console.WriteLine($"Generating a report...Payments received ${dailyTotal}, Total cost ${totalCost}, Total profit ${dailyTotal - totalCost}");
        }

        public void RecordPayment(decimal total)
        {
            dailyTotal += total;
        }

        public void RecordCost(decimal cost)
        {
            totalCost += cost;
        }
    }
}