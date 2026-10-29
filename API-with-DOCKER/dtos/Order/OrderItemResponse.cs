
namespace API_with_DOCKER.dtos.Order
{
    public record OrderItemResponse
    {
        public int ProductId { get; init; }

        public int Quantity { get; init; }

        public decimal UnitPrice { get; init; }

        public decimal TotalPrice { get; init; }
    }
}
