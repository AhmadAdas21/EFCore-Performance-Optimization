using API_with_DOCKER.DesignsPatterns.Results;
using API_with_DOCKER.services;
using Microsoft.AspNetCore.Mvc;

namespace API_with_DOCKER.Controllers
{
    [ApiController]
    [Route("api/inventory")]
    public class StockController:ControllerBase
    {
        private readonly StockService _stockService;
        public StockController(StockService stockService)
        {
            _stockService = stockService;
        }
        [HttpPost("{productId:int}/purchase")]
        public async Task<IActionResult>Purchase(int productId,CancellationToken cancellation)
        {
            var purchase =await _stockService.PurchaseAsync(cancellation,productId);
            return purchase.Status switch
            {
                ResultStatus.Success => Ok(purchase.Value),
                ResultStatus.ValidationFailure =>BadRequest(purchase.Error),

                ResultStatus.NotFound =>NotFound(purchase.Error),
                ResultStatus.Conflict =>Conflict(purchase.Error),
                ResultStatus.Failure =>StatusCode(500, purchase.Error),

            };
           
        }
    }
}
