using API_with_DOCKER.dtos.Order;
using API_with_DOCKER.models;

namespace API_with_DOCKER.DesignsPatterns.ChainOfResponsibility
{
    public class OrderValidationContext
    {
        public CreateOrder Request { get; }

        public Dictionary<int, Product> Products { get; } = new();

        public OrderValidationContext(CreateOrder request)
        {
            Request = request
                ?? throw new ArgumentNullException(nameof(request));
        }
    }
}
