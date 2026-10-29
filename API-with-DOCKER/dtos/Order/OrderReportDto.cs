namespace API_with_DOCKER.dtos.Order
{
    public record OrderReportDto
    {
        public int OrderId { get; init; }
        public string CustomerId { get; init; } = string.Empty;
        public DateTime CreatedDate { get; init; }
        public decimal TotalAmount { get; init; }
        public int TotalItems { get; init; }
    }
}
