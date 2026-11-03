
namespace API_with_DOCKER.dtos.Order
{
    public record OrderResponse
    {
        public int Id { get; init; }

        public int CustomerId { get; init; }

        public string PricingType { get; init; } = string.Empty;

        public decimal Subtotal { get; init; }

        public decimal TotalCost { get; init; }

        public DateTime CreatedAtUtc { get; init; }

        public List<OrderItemResponse> Items { get; init; } = new();
    }
}
