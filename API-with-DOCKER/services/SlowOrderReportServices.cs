using API_with_DOCKER.DataBase;
using API_with_DOCKER.dtos.Order;
using API_with_DOCKER.models;
using Microsoft.EntityFrameworkCore;

namespace API_with_DOCKER.services
{
    public class SlowOrderReportServices
    {
        private readonly AppDbContext _context;
        public SlowOrderReportServices(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<OrderReportDto>> GetSlowReportAsync(int page,int pageSize, CancellationToken cancellationToken)
        {
            if (page <= 0){
                throw new ArgumentException("Page number must be greater than 0.", nameof(page));
            }
            if (pageSize <= 0 || pageSize > 100)
            {
                throw new ArgumentException("Page size must be between 1 and 100.", nameof(pageSize));
            }
            IQueryable<Order> query = _context.Orders.AsNoTracking();
            var orders=await query
                .OrderBy(o => o.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
            List<OrderReportDto> report = new List<OrderReportDto>();
            foreach(var order in orders)
            {
                int itemCount = await _context.OrderItems.CountAsync(item => item.OrderId == order.Id,cancellationToken);
                report.Add(new OrderReportDto
                {
                    OrderId = order.Id,
                    CustomerId = order.CustomerId,
                    CreatedDate = order.CreatedAtUtc,
                    TotalCost = order.TotalCost,
                    ItemCount = itemCount
                });
            }
            return report;


        }

    }
}
