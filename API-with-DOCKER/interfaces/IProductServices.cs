using API_with_DOCKER.dtos.Product;
using API_with_DOCKER.models;
using API_with_DOCKER.DesignsPatterns.Results;
namespace API_with_DOCKER.interfaces
{
    public interface IProductServices
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product> CreateProductAsync(AddProduct request);
        Task<Result<Product>> GetProductByIdResultAsync(int id);
        Task<Result<Product>> UpdateProductAsync(int id, updateProduct request);
        Task<Result<bool>> DeleteProductAsync(int id);
    }
}
