using API_with_DOCKER.DataBase;

namespace API_with_DOCKER.services
{
    public class StockService
    {
        private readonly AppDbContext _context;
        public StockService(AppDbContext context)
        {
            _context = context;
        }
    }
}
