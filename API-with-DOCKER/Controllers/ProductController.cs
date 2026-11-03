using API_with_DOCKER.DataBase;
using API_with_DOCKER.DesignsPatterns.Results;
using API_with_DOCKER.dtos.Product;
using API_with_DOCKER.interfaces;
using API_with_DOCKER.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace API_with_DOCKER.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {

        private readonly IProductServices _productService;

        public ProductController(IProductServices productService)
        {
            _productService = productService;
        }
        [HttpGet]
        [ProducesResponseType(typeof(List<Product>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProducts()
        {
            var Products = await _productService.GetAllProductsAsync();
            return Ok(Products);

        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProduct([FromBody] AddProduct Request)
        {
            {
                var product = await _productService.CreateProductAsync(Request);

                return StatusCode(201, product);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] updateProduct request)
        {
            var result = await _productService.UpdateProductAsync(id, request);

            return result.Status switch
            {
                ResultStatus.Success => Ok(result.Value),
                ResultStatus.ValidationFailure =>BadRequest(result.Error),
                ResultStatus.NotFound =>NotFound(result.Error),
                _ => StatusCode(500, "Operation failed")
            };
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);

            return result.Status switch
            {


                ResultStatus.Success => NoContent(),



                ResultStatus.ValidationFailure =>BadRequest(result.Error),

                ResultStatus.NotFound =>NotFound(result.Error),

                ResultStatus.Conflict =>Conflict(result.Error),

                _ => StatusCode(500, "Operation failed")
            };
        }

    }
}
