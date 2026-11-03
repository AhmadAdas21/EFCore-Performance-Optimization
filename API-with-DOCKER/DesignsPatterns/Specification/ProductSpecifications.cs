using API_with_DOCKER.models;
using System.Linq.Expressions;

namespace API_with_DOCKER.DesignsPatterns.Specification
{
    public static class ProductSpecifications
    {
        public static Expression<Func<Product, bool>> Active()
        {
            return p => p.IsActive;
        }

        public static Expression<Func<Product, bool>> PriceInRange(decimal min, decimal max)
        {
            return product => product.Price >= min && product.Price <= max;
        }

        public static Expression<Func<Product, bool>> InStock()
        {
            return p => p.StockQuantity > 0;
        }
    }
}
