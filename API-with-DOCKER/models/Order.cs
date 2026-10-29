
namespace API_with_DOCKER.models
{
    public class Order
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public Customer Customer { get; set; } = null!;

        public List<OrderItem> Items { get; set; } = new();

        public string PricingType { get; set; } = "Normal";

        public decimal Subtotal { get; set; }

        public decimal TotalCost { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
