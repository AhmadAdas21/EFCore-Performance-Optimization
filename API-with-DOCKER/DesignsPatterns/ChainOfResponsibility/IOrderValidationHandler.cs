namespace API_with_DOCKER.DesignsPatterns.ChainOfResponsibility
{
    public interface IOrderValidationHandler
    {
        IOrderValidationHandler SetNext(IOrderValidationHandler next);

        Task ValidateAsync(OrderValidationContext context,CancellationToken cancellationToken = default);
    }
}
