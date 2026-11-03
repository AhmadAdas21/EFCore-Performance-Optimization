using System.ComponentModel.DataAnnotations;

namespace API_with_DOCKER.dtos.Product
{
    public record deleteProduct
    {
        [Required]
        public int Id { get; init; }
    }
}
