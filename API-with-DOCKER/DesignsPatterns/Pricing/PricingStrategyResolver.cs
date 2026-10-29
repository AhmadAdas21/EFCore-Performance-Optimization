namespace API_with_DOCKER.DesignsPatterns.Pricing
{
    public class PricingStrategyResolver : IPricingStrategyResolver
    {
        private readonly IPricingStrategy _normalStrategy;
        private readonly IPricingStrategy _vipStrategy;
        private readonly IPricingStrategy _blackFridayStrategy;

        public PricingStrategyResolver( NormalPricingStrategy normalStrategy,VipPricingStrategy vipStrategy, BlackFridayPricingStrategy blackFridayStrategy)
        {
            _normalStrategy = normalStrategy;
            _vipStrategy = vipStrategy;
            _blackFridayStrategy = blackFridayStrategy;
        }
        public IPricingStrategy Resolve(string pricingType)
        {
            if (string.IsNullOrWhiteSpace(pricingType))
            {
                throw new ArgumentException( "Pricing type is required.", nameof(pricingType));
            }
            return pricingType switch
            {
                "Normal" => _normalStrategy,
                "VIP" => _vipStrategy,
                "BlackFriday" => _blackFridayStrategy,
                _ => throw new ArgumentException($"Unknown pricing type: {pricingType}")
            };
        }
    }
}
