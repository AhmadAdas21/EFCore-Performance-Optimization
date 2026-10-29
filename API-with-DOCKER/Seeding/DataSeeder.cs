using API_with_DOCKER.DataBase;

namespace API_with_DOCKER.Seeding
{
    public class DataSeeder
    {
        private readonly AppDbContext _context;
        private const int chucnkSize = 1000;

        private const int CustomerCount = 10_000;
        private const int ProductCount = 20_000;
        private const int OrderCount = 50_000;
        private const int OrderItemCount = 200_000;



        public DataSeeder(AppDbContext context)
        {
            _context = context;
        }



    }
}
