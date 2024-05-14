namespace NotificationHub.Domain.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

        public Product(string productId, string productName, string description, decimal price, int stock)
        {
            ProductId = productId;
            ProductName = productName;
            Description = description;
            Price = price;
            Stock = stock;
        }
    }
}
