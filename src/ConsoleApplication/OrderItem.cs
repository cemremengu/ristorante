namespace ConsoleApplication
{
    public class OrderItem
    {
        public OrderItem(string name, int quantity)
        {
            Name = name;
            Quantity = quantity;
        }

        public decimal Cost { get; set; }

        public string Name { get; }

        public int Quantity { get; }

        public decimal Price { get; set; }
    }
}