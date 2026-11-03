
using API_with_DOCKER.DataBase;
using API_with_DOCKER.DesignsPatterns.Pricing;
using API_with_DOCKER.dtos.Order;
using API_with_DOCKER.interfaces;
using API_with_DOCKER.models;
using Microsoft.EntityFrameworkCore;
using API_with_DOCKER.DesignsPatterns.ChainOfResponsibility;
namespace API_with_DOCKER.services
{
    public class OrderServices : IOrderServices
    {
        private readonly AppDbContext _context;
        private readonly IPricingStrategyResolver _pricingResolver;
        private readonly IOrderValidationHandler _validationPipeline;
        public OrderServices(AppDbContext context, IPricingStrategyResolver pricingResolver, IOrderValidationHandler validationPipeline)
        {
            _context = context;
            _pricingResolver = pricingResolver;
            _validationPipeline = validationPipeline;
        }

        public async Task<List<Order>> GetAllOrdersAsync(
            CancellationToken cancellationToken = default)
        {
            return await _context.Orders.AsNoTracking().Include(order => order.Items).ToListAsync(cancellationToken);
        }

        public async Task<Order> CreateOrderAsync( CreateOrder request,CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);

            var validationContext = new OrderValidationContext(request);

            await _validationPipeline.ValidateAsync( validationContext,cancellationToken);

            var products = validationContext.Products;



            List<OrderItem> orderItems = new();

            foreach (var item in request.Items)
            {
                var product = products[item.ProductId];

                orderItems.Add(new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price
                });
            }

            decimal subtotal = orderItems.Sum(item => item.TotalPrice);

            IPricingStrategy strategy = _pricingResolver.Resolve(request.PricingType);

            decimal finalPrice = strategy.CalculatePrice(subtotal);

            finalPrice = Math.Round(finalPrice,2,MidpointRounding.AwayFromZero);

            Order order = new Order
            {
                CustomerId = request.CustomerId,
                Items = orderItems,
                PricingType = request.PricingType,
                Subtotal = subtotal,
                TotalCost = finalPrice,
                CreatedAtUtc = DateTime.UtcNow
            };

            await _context.Orders.AddAsync( order, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return order;
        }
    }
}
