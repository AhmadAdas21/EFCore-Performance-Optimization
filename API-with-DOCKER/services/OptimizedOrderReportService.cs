using API_with_DOCKER.DataBase;
using API_with_DOCKER.dtos.Order;
using Microsoft.EntityFrameworkCore;

namespace API_with_DOCKER.services
{
    public class OptimizedOrderReportService
    {
        private readonly AppDbContext _context;

        public OptimizedOrderReportService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrderReportDto>> GetOptimizedReportAsync(int page,int pageSize,CancellationToken cancellationToken)
        {
            if (page <= 0 || pageSize <= 0 || pageSize > 100)
            {
                throw new ArgumentException("must be above 0 and below 100");
            }
           var orders= _context.Orders.AsNoTracking().OrderBy(order=>order.Id).Skip((page-1)*pageSize).Take(pageSize).Select(order=>new OrderReportDto
           {
               OrderId=order.Id,
               CreatedDate=order.CreatedAtUtc,
               CustomerId=order.CustomerId,
               TotalCost=order.TotalCost,
               ItemCount=order.Items.Count,


           }).ToListAsync(cancellationToken);
           return await orders;
        }
    }
}