namespace MotorcycleRentalManagement.Domain.Entities
{
    public interface INotifiable
    {
        void AddNotification(string property, string message);
        bool IsValid { get; }
        IReadOnlyCollection<Notification> Notifications { get; }
    }

    public class Notification
    {
        public string Property { get; }
        public string Message { get; }

        public Notification(string property, string message)
        {
            Property = property;
            Message = message;
        }
    }

    public class Notifiable : INotifiable
    {
        private readonly List<Notification> _notifications;

        public IReadOnlyCollection<Notification> Notifications => _notifications;

        public Notifiable()
        {
            _notifications = new List<Notification>();
        }

        public void AddNotification(string property, string message)
        {
            _notifications.Add(new Notification(property, message));
        }

        public bool IsValid => !_notifications.Any();
    }
}