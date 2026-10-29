using API_with_DOCKER.DataBase;
using API_with_DOCKER.models;
using Microsoft.EntityFrameworkCore;
namespace API_with_DOCKER.Seeding
{
    public class DataSeeder
    {
        private readonly AppDbContext _context;
        private const int ChunkSize = 1000;

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
                for (int j = 0; j < ChunkSize; j++)
                {
                    customers.Add(new Customer
                    {
                        Name = $"customer_num is{i * ChunkSize + j+1}"
                    }); 
                }
                await _context.Customers.AddRangeAsync(customers, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                _context.ChangeTracker.Clear();
            }
        }
        public async Task SeedProductValuesAsync(CancellationToken cancellationToken)
        {
            var existingProducts = await _context.Products.CountAsync(cancellationToken);
            if (existingProducts > 0)
            {
                throw new InvalidOperationException("products already exist in the database.");
            }
            for(int i =0; i < 20; i++) 
            {
                List<Product> products = new List<Product>();
                for(int j = 0; j < ChunkSize; j++)
                {
                    int productNumber = i * ChunkSize + j + 1;
                    products.Add(new Product
                    {
                        Name = $"product_num is {productNumber}",
                        Description = $"description for product {productNumber}",
                        Price = (decimal)(productNumber%2==0? 50 : productNumber*1),
                        StockQuantity = productNumber * 5,
                        IsActive = productNumber % 2 == 0 ? true : false
                    });
                }
                await _context.Products.AddRangeAsync(products, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                _context.ChangeTracker.Clear();
            }
        }
        public async Task SeedOrderValuesAsync(CancellationToken cancellationToken)
        {
            var existingOrders= await _context.Orders.CountAsync(cancellationToken);
            if (existingOrders > 0)
            {
                throw new InvalidOperationException("orders already exist in the database.");
            }
            var customersId = await _context.Customers.Select(Customer=>Customer.Id).ToListAsync(cancellationToken);
            if (customersId.Count <= 0)
            {
                throw new InvalidOperationException("No customers found in the database.");
            }
            var products= await _context.Products.OrderBy(p => p.Id).Select(p => new
                                                                    {
                                                                      p.Id,
                                                                      p.Price
                                                                    }).ToListAsync(cancellationToken); ;
            if(products.Count <4)
            {
                throw new InvalidOperationException("No products found in the database.");
            }
            for (int i = 0; i < 50; i++)
            {
                List<Order> orders = new List<Order>();
                for (int j = 0; j < ChunkSize; j++)
                {
                    int orderNumber = i * ChunkSize + j + 1;
                    int k = (orderNumber - 1) * 4;
                    var product1 = products[k % products.Count];
                    var product2 = products[(k + 1) % products.Count];
                    var product3 = products[(k + 2) % products.Count];
                    var product4 = products[(k + 3) % products.Count];

                    decimal subtotal =product1.Price + product2.Price +product3.Price + product4.Price;
                    orders.Add(new Order
                    {
                        CustomerId = customersId[(orderNumber - 1) % customersId.Count],

                        Items = new List<OrderItem>
                        {
                            new OrderItem
                            {
                                ProductId = product1.Id,
                                Quantity = 1,
                                UnitPrice = product1.Price
                            },
                            new OrderItem
                            {
                                 ProductId = product2.Id,
                                Quantity = 1,
                                UnitPrice = product2.Price
                            },
                            new OrderItem
                            {
                                ProductId = product3.Id,
                                Quantity = 1,
                                UnitPrice = product3.Price
                            },
                            new OrderItem{
                                ProductId = product4.Id,
                                Quantity = 1,
                                UnitPrice = product4.Price
                            }
                        },
                        Subtotal = subtotal,
                        TotalCost = subtotal
                    });
                }
                _context.Orders.AddRange(orders);

                await _context.SaveChangesAsync(cancellationToken);

                _context.ChangeTracker.Clear();
            }
        }




    }
}
