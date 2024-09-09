using MotorcycleRentalManagement.Domain.Entities;

namespace MotorcycleRentalManagement.Domain.Interfaces.Repositories
{
    public interface INotificationEventRepository
    {
        Task SaveAsync(Notification notification);
    }
}
