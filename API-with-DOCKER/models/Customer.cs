
namespace API_with_DOCKER.models
{
    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public List<Order> Orders { get; set; } = new();
    }
}
