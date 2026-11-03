
using API_with_DOCKER.DataBase;
using Microsoft.EntityFrameworkCore;

namespace API_with_DOCKER.DesignsPatterns.ChainOfResponsibility;

public class ProductValidationHandler : OrderValidationHandler
{
    private readonly AppDbContext _context;

    public ProductValidationHandler(AppDbContext context)
    {
        _context = context;
    }

    protected override async Task ValidateCurrentAsync(OrderValidationContext context,CancellationToken cancellationToken)
    {
        var items = context.Request.Items;

        if (items == null || items.Count == 0)
        {
            throw new ArgumentException("order must contain at least one item.");
        }

        if (items.Any(item =>item == null ||item.ProductId <= 0 || item.Quantity <= 0))
        {
            throw new ArgumentException( "invalid product ID or quantity.");
        }

        var productIds = items.Select(item => item.ProductId).ToList();

        if (productIds.Count != productIds.Distinct().Count())
        {
            throw new ArgumentException( "duplicate products are not allowed.");
        }

        var products = await _context.Products.AsNoTracking().Where(product => productIds.Contains(product.Id))
            .ToDictionaryAsync(product => product.Id,cancellationToken);

        if (products.Count != productIds.Count)
        {
            throw new KeyNotFoundException("one or more products were not found.");
        }

        foreach (var product in products)
        {
            context.Products.Add(product.Key, product.Value);
        }
    }
}
