using MongoDB.Driver;
using MotorcycleRentalManagement.Domain.Entities;
using MotorcycleRentalManagement.Domain.Interfaces.Repositories;

namespace MotorcycleRentalManagement.Infrastructure.Repositories
{
    public class NotificationEventRepository : INotificationEventRepository
    {
        private readonly IMongoDatabase _database;

        public NotificationEventRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task SaveAsync(Notification notification)
        {
            var collection = _database.GetCollection<Notification>("Notifications");
            await collection.InsertOneAsync(notification);
        }
    }
}
