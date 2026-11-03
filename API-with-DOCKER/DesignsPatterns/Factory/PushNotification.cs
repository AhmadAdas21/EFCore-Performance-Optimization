namespace API_with_DOCKER.DesignsPatterns.Factory
{
    public class PushNotification:INotification
    {
        private readonly ILogger<PushNotification> _logger;
        public PushNotification(ILogger<PushNotification> logger)
        {
            _logger = logger;
        }
        public Task SendAsync(string destination, string message, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (string.IsNullOrWhiteSpace(destination))
            {
                throw new ArgumentException("Push notification destination is required.", nameof(destination));
            }
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("Notification message is required.", nameof(message));
            }
            _logger.LogInformation("Push notification simulated successfully.");
            return Task.CompletedTask;
        }
    }
}
