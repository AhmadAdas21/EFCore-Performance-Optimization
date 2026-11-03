
using System.ComponentModel.DataAnnotations;

namespace API_with_DOCKER.dtos.Order
{
    public class CreateOrder
    {
        [Range(1, int.MaxValue)]
        public int CustomerId { get; set; }

        [Required]
        [MinLength(1)]
        public List<CreateOrderItem> Items { get; set; } = new();

        [Required]
        public string PricingType { get; set; } = "Normal";
    }
}
