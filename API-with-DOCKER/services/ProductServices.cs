using API_with_DOCKER.DataBase;
using API_with_DOCKER.DesignsPatterns.Specification;
using API_with_DOCKER.dtos.Product;
using API_with_DOCKER.interfaces;
using API_with_DOCKER.models;
using API_with_DOCKER.DesignsPatterns.Specification;
using API_with_DOCKER.DesignsPatterns.Results;
using Microsoft.EntityFrameworkCore;

namespace API_with_DOCKER.services
{
    public class ProductServices:IProductServices
    {

        private readonly AppDbContext AppDbContext;

        public ProductServices(AppDbContext context)
        {
            AppDbContext = context;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await AppDbContext.Products.AsNoTracking().ToListAsync();
        }
        public async Task<Product> CreateProductAsync(AddProduct dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity
            };

            AppDbContext.Products.Add(product);

            await AppDbContext.SaveChangesAsync();

            return product;
        }

        public async Task<List<Product>> SearchProductsAsync(decimal minPrice, decimal maxPrice)
        {
            return await AppDbContext.Products
                .AsNoTracking()
                .Where(ProductSpecifications.Active())
                .Where(ProductSpecifications.PriceInRange(minPrice, maxPrice))
                .Where(ProductSpecifications.InStock())
                .ToListAsync();
        }

        public async Task<Result<Product>> GetProductByIdResultAsync(int id)
        {
            if (id <= 0)
            {
                return Result<Product>.ValidationFailure(
                    "Invalid product ID");
            }

            var product = await AppDbContext.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return Result<Product>.NotFound(
                    "Product not found");
            }

            return Result<Product>.Success(product);
        }

        public async Task<Result<Product>> UpdateProductAsync(int id, updateProduct request)
        {
            if (id <= 0 ||(request.name == null &&request.description == null && request.price == null) || request.price is <= 0)
            {
                return Result<Product>.ValidationFailure("invalid update for product");
            }

            var product = await AppDbContext.Products.FindAsync(id);

            if (product == null)
            {
                return Result<Product>.NotFound("product" +
                    ".30 not found");
            }

            if (request.name != null)
            {
                product.Name = request.name;
            }
            if (request.description != null)
            {
                product.Description = request.description;
            }
            if (request.price.HasValue)
            {
                product.Price = request.price.Value;
            }
            await AppDbContext.SaveChangesAsync();
            return Result<Product>.Success(product);
        }

        public async Task<Result<bool>> DeleteProductAsync(int id)
        {
            if (id <= 0)
            {
                return Result<bool>.ValidationFailure( "Invalid product ID");
            }

            var product = await AppDbContext.Products.FindAsync(id);

            if (product == null)
            {
                return Result<bool>.NotFound("Product not found");
            }

            bool hasOrders = await AppDbContext.OrderItems.AnyAsync(item => item.ProductId == id);

            if (hasOrders)
            {
                return Result<bool>.Conflict("Cannot delete a product used in orders");
            }

            AppDbContext.Products.Remove(product);

            await AppDbContext.SaveChangesAsync();

            return Result<bool>.Success(true);
        }




    }

}
