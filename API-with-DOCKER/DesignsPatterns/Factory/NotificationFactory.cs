using Microsoft.IdentityModel.Tokens;

namespace API_with_DOCKER.DesignsPatterns.Factory
{
    public class NotificationFactory:INotificationFactory
    {

        private readonly EmailNotification _emailNotification;
        private readonly SmsNotification _smsNotification;
        private readonly PushNotification _pushNotification;
        public NotificationFactory(EmailNotification emailNotification, SmsNotification smsNotification, PushNotification pushNotification)
        {
            _emailNotification = emailNotification;
            _smsNotification = smsNotification;
            _pushNotification = pushNotification;
        }
        public INotification Create(NotificationType notificationType)
        {

            return notificationType switch
            {
                NotificationType.Email => _emailNotification,
                NotificationType.Sms => _smsNotification,
                NotificationType.Push => _pushNotification,
                _ => throw new ArgumentOutOfRangeException(nameof(notificationType),notificationType, "Unsupported notification type."), };
        }
    }
}
