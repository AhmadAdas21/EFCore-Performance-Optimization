namespace API_with_DOCKER.DesignsPatterns.Pricing
{
    public class BlackFridayPricingStrategy: IPricingStrategy
    {
        private const decimal discountPercentage = 0.5m;
        public decimal CalculatePrice(decimal originalPrice)
        {
            if (originalPrice < 0)
            {
                throw new Exception("Price cannot be negative");
            }
            decimal discountedPrice = (originalPrice * discountPercentage);
            decimal finalPrice=originalPrice - discountedPrice;
            return finalPrice;

        }
    }
}
