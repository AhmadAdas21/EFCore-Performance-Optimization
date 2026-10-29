namespace API_with_DOCKER.DesignsPatterns.Factory
{
    public class SmsNotification : INotification
    {
        private readonly ILogger<SmsNotification> _logger;
        public SmsNotification(ILogger<SmsNotification> logger)
        {
            _logger = logger;
        }
        public Task SendAsync(string destination, string message, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (string.IsNullOrWhiteSpace(destination))
            {
                throw new ArgumentException("SMS destination is required.", nameof(destination));
            }
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("Notification message is required.", nameof(message));
            }
            _logger.LogInformation("SMS notification simulated successfully.");
            return Task.CompletedTask;
        }
    }
}
