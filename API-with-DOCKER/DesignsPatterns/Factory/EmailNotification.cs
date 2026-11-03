namespace API_with_DOCKER.DesignsPatterns.Factory
{
    public class EmailNotification:INotification
    {
        private readonly ILogger<EmailNotification> _logger;
        public EmailNotification(ILogger<EmailNotification> logger)
        {
            _logger = logger;
        }
        public Task SendAsync(string destination,string message,CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(destination))
            {
                throw new ArgumentException( "Email destination is required.",nameof(destination));
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("Notification message is required.",nameof(message));
            }

            _logger.LogInformation( "Email notification simulated successfully.");

            return Task.CompletedTask;
        }
    }
}
