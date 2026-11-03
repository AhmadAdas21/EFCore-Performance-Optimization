namespace API_with_DOCKER.DesignsPatterns.ChainOfResponsibility
{
    public class StockValidationHandler:OrderValidationHandler
    {
        protected override Task ValidateCurrentAsync(
        OrderValidationContext context,
        CancellationToken cancellationToken)
        {
            foreach (var item in context.Request.Items)
            {
                if (!context.Products.TryGetValue(item.ProductId, out var product))
                {
                    throw new KeyNotFoundException($"Product {item.ProductId} not found.");
                }

                if (product.StockQuantity < item.Quantity)
                {
                    throw new InvalidOperationException($"Insufficient stock for product {product.Id}. " +$"Available: {product.StockQuantity}, " +
                        $"Requested: {item.Quantity}");
                }
            }

            return Task.CompletedTask;
        }
        }
}
