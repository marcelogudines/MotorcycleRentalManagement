namespace MotorcycleRentalManagement.Domain.Interfaces.Repositories
{
    public interface IMessagePublisher
    {
        Task PublishMotorcycleRegistered(string motorcycleId, string licensePlate, int year);
    }
}
