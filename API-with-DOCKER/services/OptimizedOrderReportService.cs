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
           
            throw new NotImplementedException();
        }
    }
}