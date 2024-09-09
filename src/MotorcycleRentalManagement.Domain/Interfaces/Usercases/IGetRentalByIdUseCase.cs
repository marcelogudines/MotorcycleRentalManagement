using MotorcycleRentalManagement.Domain.Entities;

namespace MotorcycleRentalManagement.Domain.Interfaces.Usercases
{
    public interface IGetRentalByIdUseCase
    {
        Task<Rental> ExecuteAsync(Guid rentalId);
    }
}