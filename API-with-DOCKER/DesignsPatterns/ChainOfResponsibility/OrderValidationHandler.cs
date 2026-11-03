
namespace API_with_DOCKER.DesignsPatterns.ChainOfResponsibility;

public abstract class OrderValidationHandler: IOrderValidationHandler
{
    private IOrderValidationHandler? _next;

    public IOrderValidationHandler SetNext(IOrderValidationHandler next)
    {
        ArgumentNullException.ThrowIfNull(next);

        _next = next;
        return next;
    }

    public async Task ValidateAsync(OrderValidationContext context,CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);

        cancellationToken.ThrowIfCancellationRequested();

        await ValidateCurrentAsync(context, cancellationToken);

        if (_next != null)
        {
            await _next.ValidateAsync(context, cancellationToken);
        }
    }

    protected abstract Task ValidateCurrentAsync(OrderValidationContext context,CancellationToken cancellationToken);
}
