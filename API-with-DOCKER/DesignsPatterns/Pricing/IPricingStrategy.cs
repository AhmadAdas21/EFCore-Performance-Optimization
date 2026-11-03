namespace API_with_DOCKER.DesignsPatterns.Pricing
{
    public interface IPricingStrategy
    {
        decimal CalculatePrice(decimal originalPrice);
    }
}
