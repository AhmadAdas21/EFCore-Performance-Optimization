
using API_with_DOCKER.dtos.Order;
using API_with_DOCKER.interfaces;
using API_with_DOCKER.models;
using Microsoft.AspNetCore.Mvc;
using API_with_DOCKER.services;
namespace API_with_DOCKER.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderServices _orderService;
        private readonly SlowOrderReportServices _slowReportService;
        private readonly OptimizedOrderReportService _optimizedReportService;
        public OrderController(IOrderServices orderService, SlowOrderReportServices slowOrder, OptimizedOrderReportService optimizedReportService)
        {
            _orderService = orderService;
            _slowReportService = slowOrder;
            _optimizedReportService = optimizedReportService;
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

            return StatusCode(StatusCodes.Status201Created, ToResponse(order));
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
        [HttpGet("report")]
        public async Task<IActionResult> GetSlowOrderReport(CancellationToken cancellationToken=default, [FromQuery] int page = 1, [FromQuery] int pageSize = 15)
        {
            if (page <= 0 || pageSize <= 0 || pageSize > 100)
            {
                return BadRequest("the page and the page size must be above 0");
            }
            var reports = await _slowReportService.GetSlowReportAsync(page, pageSize, cancellationToken);
            return Ok(reports);
        }

        [HttpGet("report/optimized")]
        public async Task<IActionResult> GetOptimizedOrderReport(CancellationToken cancellationToken,[FromQuery] int page = 1,[FromQuery] int pageSize = 15)
        {
            if (page <= 0 || pageSize <= 0 || pageSize > 100)
            {
                return BadRequest("Invalid pagination parameters.");
            }

            var report = await _optimizedReportService.GetOptimizedReportAsync(page, pageSize, cancellationToken);

            return Ok(report);
        }


    }
}
