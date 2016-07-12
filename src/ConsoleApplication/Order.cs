using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    public class Order
    {
        public Order(string waiterName, int tableNumber)
        {
            WaiterName = waiterName;
            TableNumber = tableNumber;
        }

        public string WaiterName { get; }

        public int TableNumber { get;}

        public List<OrderItem> OrderItems { get; set; }
    }
}
