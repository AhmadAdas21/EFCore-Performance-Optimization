namespace API_with_DOCKER.interfaces
{
    public interface IPaymentService
    {
        Task<bool> PayAsync(decimal amount);
    }
}
