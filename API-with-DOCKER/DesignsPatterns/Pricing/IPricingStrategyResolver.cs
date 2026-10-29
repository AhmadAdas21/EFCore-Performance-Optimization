namespace API_with_DOCKER.DesignsPatterns.Pricing
{
    public interface IPricingStrategyResolver
    {
        IPricingStrategy Resolve(string pricingType);
    }
}
