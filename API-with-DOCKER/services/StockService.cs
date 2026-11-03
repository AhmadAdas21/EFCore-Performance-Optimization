using System.Linq;
using API_with_DOCKER.DataBase;
using API_with_DOCKER.DesignsPatterns.Results;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace API_with_DOCKER.services
{
    public class StockService
    {
        private readonly AppDbContext _context;
        public StockService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Result<bool>> PurchaseAsync(CancellationToken cancellationToken, int productId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(product=>product.Id==productId, cancellationToken);
            if (product == null)
            {
                return  Result<bool>.NotFound("product not found");
            }

            if (product.StockQuantity <= 0)
            {
                return Result<bool>.Conflict("the quantity must be greater than 0");
            }
            int affectedRows = await _context.Products.Where(x=>x.Id==productId && x.StockQuantity>0).ExecuteUpdateAsync(setters => setters.SetProperty(x=>x.StockQuantity,x=>x.StockQuantity -1 ),cancellationToken);
            if (affectedRows > 0)
            {
                return Result<bool>.Success(true);
            }
            else
            {
                
                return Result<bool>.Conflict("product not found or the stock is zero ");
            }

        }
    }
}
