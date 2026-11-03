using System.Text.Json;
using API_with_DOCKER.dtos.Product;
using API_with_DOCKER.interfaces;
using API_with_DOCKER.models;
using Microsoft.Extensions.Caching.Distributed;
using API_with_DOCKER.DesignsPatterns.Results;
using StackExchange.Redis;
using System.Collections.Concurrent;
namespace API_with_DOCKER.DesignsPatterns.Decorater
{
    public class CachedProductService : IProductServices
    {
        private readonly IProductServices _inner;
        private readonly IDistributedCache _cache;
        private readonly ILogger<CachedProductService> _logger;


        private const string CacheKey = "AllProductsCache";

        public CachedProductService(IProductServices inner, IDistributedCache cache, ILogger<CachedProductService> logger)
        {
            _inner = inner;
            _cache = cache;
            _logger = logger;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            var cachedData = await SafeGetCacheAsync(CacheKey);

            if (cachedData != null)
            {
                var cachedProducts = JsonSerializer.Deserialize<List<Product>>(cachedData);

                if (cachedProducts != null)
                {
                    _logger.LogInformation("cache hit");
                    return cachedProducts;
                }
            }
            _logger.LogInformation("cache missed");

            var products = await _inner.GetAllProductsAsync();

            await SafeSetCacheAsync(CacheKey, JsonSerializer.Serialize(products));

            return products;
        }

        public async Task<Product> CreateProductAsync(AddProduct request)
        {
            var product =await _inner.CreateProductAsync(request);
            await SafeRemoveCacheAsync($"product:{product.Id}");
            await SafeRemoveCacheAsync(CacheKey);
            _logger.LogInformation("the cache removed");
            return product;
        }

        public async Task<Result<Product>> GetProductByIdResultAsync(int id)
        {
            if (id <= 0)
            {

            return await _inner.GetProductByIdResultAsync(id);
            }
            string cacheKey = $"product:{id}";


            var cachedData = await SafeGetCacheAsync(cacheKey);

            if (cachedData != null)
            {
                var product = JsonSerializer.Deserialize<Product>(cachedData);

                if (product != null)
                {
                    _logger.LogInformation("cache hir for product the id is  {Id}", id);
                    return Result<Product>.Success(product);
                }
            }


            var semaphore = ProductLocks.GetOrAdd(id, _ => new SemaphoreSlim(1, 1));

            await semaphore.WaitAsync();

            try
            {

                cachedData = await SafeGetCacheAsync(cacheKey);

                if (cachedData != null)
                {
                    var product = JsonSerializer.Deserialize<Product>(cachedData);

                    if (product != null)
                    {
                        _logger.LogInformation("cache hir for product the id is  {Id}", id);

                        return Result<Product>.Success(product);
                    }
                }

                _logger.LogInformation("cache miss product the id is  {Id}", id);

                var result = await _inner.GetProductByIdResultAsync(id);

                if (result.IsSuccess && result.Value != null)
                {
                    await SafeSetCacheAsync(cacheKey,JsonSerializer.Serialize(result.Value));
                }

                return result;
            }
            finally
            {
                semaphore.Release();
            }
        }


        public async Task<Result<Product>> UpdateProductAsync(int id, updateProduct request)
        {
            var semaphore = ProductLocks.GetOrAdd(id, _ => new SemaphoreSlim(1, 1));

            await semaphore.WaitAsync();

            try
            {
                var result = await _inner.UpdateProductAsync(id, request);

                if (!result.IsSuccess)
                {

                    return result;
                }
                await SafeRemoveCacheAsync($"product:{id}");
                await SafeRemoveCacheAsync(CacheKey);

                _logger.LogInformation("cache invalidated after update for product {Id}", id);

                return result;
            }
            finally
            {
                semaphore.Release();
            }
        }



        public async Task<Result<bool>> DeleteProductAsync(int id)
        {
            var semaphore = ProductLocks.GetOrAdd(id, _ => new SemaphoreSlim(1, 1));

            await semaphore.WaitAsync();

            try
            {
                var result = await _inner.DeleteProductAsync(id);

                if (!result.IsSuccess)
                {
                    return result;
                }
                await SafeRemoveCacheAsync($"product:{id}");
                await SafeRemoveCacheAsync(CacheKey);

                _logger.LogInformation("cache invalidated after update for product {Id}", id);

                return result;
            }
            finally
            {
                semaphore.Release();
            }
        }



        private async Task<string?> SafeGetCacheAsync(string key)
        {
            try
            {
                return await _cache.GetStringAsync(key);
            }
            catch (Exception ex) when (ex is RedisException or TimeoutException)
            {
                _logger.LogWarning(ex,"Redis unavailable during GET for {CacheKey}. Using database.",key);

                return null;
            }
        }

        private async Task SafeSetCacheAsync(string key, string value)
        {
            try
            {
                await _cache.SetStringAsync(key,value,
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow =TimeSpan.FromMinutes(10)
                    });
            }
            catch (Exception ex) when (ex is RedisException or TimeoutException)
            {
                _logger.LogWarning(ex,"Redis unavailable during SET for {CacheKey}.",key);
            }
        }

        private async Task SafeRemoveCacheAsync(string key)
        {
            try
            {
                await _cache.RemoveAsync(key);
            }
            catch (Exception ex) when (ex is RedisException or TimeoutException)
            {
                _logger.LogWarning( ex,"Redis unavailable during INVALIDATION for {CacheKey}.", key);
            }
        }
        private static readonly ConcurrentDictionary<int, SemaphoreSlim>
            ProductLocks = new();


    }
}
