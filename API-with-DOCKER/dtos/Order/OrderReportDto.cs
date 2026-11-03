namespace API_with_DOCKER.dtos.Order
{
    public record OrderReportDto
    {
        public int OrderId { get; init; }
        public int CustomerId { get; init; } 
        public DateTime CreatedDate { get; init; }
        public decimal TotalCost { get; init; }
        public int ItemCount { get; init; }
    }
}
