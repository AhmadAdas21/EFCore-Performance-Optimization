using API_with_DOCKER.dtos.Order;
using API_with_DOCKER.models;

namespace API_with_DOCKER.interfaces
{
    public interface IOrderServices
    {
        Task<List<Order>> GetAllOrdersAsync(CancellationToken cancellationToken = default);

        Task<Order> CreateOrderAsync( CreateOrder request,CancellationToken cancellationToken = default);
    }
}
