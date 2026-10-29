namespace API_with_DOCKER.DesignsPatterns.Pricing
{
    public class NormalPricingStrategy : IPricingStrategy
    {
        public decimal CalculatePrice(decimal originalPrice)
        {
            if(originalPrice < 0)
            {
                throw new ArgumentException("Price cannot be negative");
            }
            return originalPrice;
        }
    }
}
