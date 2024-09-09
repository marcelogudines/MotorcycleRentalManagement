using MotorcycleRentalManagement.Domain.Entities;

namespace MotorcycleRentalManagement.Domain.Interfaces
{
    public interface IReturnRentalUseCase
    {
        Task<INotifiable> ExecuteAsync(Guid rentalId, DateTime returnDate);
    }
}
