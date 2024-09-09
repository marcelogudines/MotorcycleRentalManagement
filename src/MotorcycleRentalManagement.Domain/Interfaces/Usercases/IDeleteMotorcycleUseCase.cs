using MotorcycleRentalManagement.Domain.Entities;

namespace MotorcycleRentalManagement.Domain.Interfaces.Usercases
{
    public interface IDeleteMotorcycleUseCase
    {
        Task<INotifiable> ExecuteAsync(string id);
    }
}
