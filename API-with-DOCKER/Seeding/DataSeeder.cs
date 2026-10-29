using API_with_DOCKER.DataBase;
using API_with_DOCKER.models;
using Microsoft.EntityFrameworkCore;
namespace API_with_DOCKER.Seeding
{
    public class DataSeeder
    {
        private readonly AppDbContext _context;
        private const int ChucnkSize = 1000;

        private const int CustomerCount = 10_000;
        private const int ProductCount = 20_000;
        private const int OrderCount = 50_000;
        private const int OrderItemCount = 200_000;



        public DataSeeder(AppDbContext context)
        {
            _context = context;
        }
        public async Task SeedCustomerValuesAsync(CancellationToken cancellationToken)
        {
            var existingCustomers = await _context.Customers.CountAsync(cancellationToken);
            if (existingCustomers > 0)
            {
                throw new InvalidOperationException("customers already exist in the database.");
            }
            for (int i = 0; i < 10; i++)
            {
                List<Customer> customers = new List<Customer>(); 
                for (int j = 0; j < ChucnkSize; j++)
                {
                    customers.Add(new Customer
                    {
                        Name = $"customer_num is{i * ChucnkSize + j+1}"
                    }); 
                }
                await _context.Customers.AddRangeAsync(customers, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                _context.ChangeTracker.Clear();
            }
        }



    }
}
