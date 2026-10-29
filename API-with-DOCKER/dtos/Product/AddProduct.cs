using System.ComponentModel.DataAnnotations;

namespace API_with_DOCKER.dtos.Product
{
    public record AddProduct
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; init; }
        [Required]
        [StringLength(500, MinimumLength = 10)]
        public string Description { get; init; }
        [Required]
        [Range(1, 10000000.00)]
        public decimal Price { get; init; }
        [Required]
        [Range(0, int.MaxValue)]
        public int StockQuantity { get; init; }
    }
}
