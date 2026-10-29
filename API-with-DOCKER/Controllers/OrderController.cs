
using API_with_DOCKER.dtos.Order;
using API_with_DOCKER.interfaces;
using API_with_DOCKER.models;
using Microsoft.AspNetCore.Mvc;

namespace API_with_DOCKER.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController:ControllerBase
    {
        private readonly  IOrderServices _orderService;
        public OrderController(IOrderServices orderService)
        {
            _orderService = orderService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllOrders(CancellationToken cancellationToken)
        {
            var orders = await _orderService.GetAllOrdersAsync(cancellationToken);

            return Ok(orders.Select(ToResponse).ToList());
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrder request, CancellationToken cancellationToken)
        {
            var order = await _orderService.CreateOrderAsync(request, cancellationToken);

            return StatusCode( StatusCodes.Status201Created, ToResponse(order));
        }

        private static OrderResponse ToResponse(Order order)
        {
            return new OrderResponse
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                PricingType = order.PricingType,
                Subtotal = order.Subtotal,
                TotalCost = order.TotalCost,
                CreatedAtUtc = order.CreatedAtUtc,

                Items = order.Items.Select(item => new OrderItemResponse
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    TotalPrice = item.TotalPrice

                }).ToList()
            };
        }


    }
}
