namespace API_with_DOCKER.dtos.Product
{
    public record updateProduct
    {
        public decimal? price { get; init; }
        public string? description { get; init; }
        public string? name { get; init; }
    }
}
