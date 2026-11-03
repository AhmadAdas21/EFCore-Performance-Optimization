namespace API_with_DOCKER.DesignsPatterns.Factory
{
    public interface INotificationFactory
    {
        INotification Create(NotificationType notificationType);
    }
}
