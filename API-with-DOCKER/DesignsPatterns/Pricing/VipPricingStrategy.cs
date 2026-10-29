namespace API_with_DOCKER.DesignsPatterns.Pricing
{
    public class VipPricingStrategy : IPricingStrategy
    {
        private const decimal VipDiscountPercentage = 0.1m;
        public decimal CalculatePrice(decimal originalPrice)
        {
            if (originalPrice < 0)
            {
                throw new Exception("Price cannot be negative");
            }
            decimal discountedPrice = (originalPrice * VipDiscountPercentage);
            decimal finalPrice = originalPrice - discountedPrice;
            return finalPrice;
        }
    }
}
