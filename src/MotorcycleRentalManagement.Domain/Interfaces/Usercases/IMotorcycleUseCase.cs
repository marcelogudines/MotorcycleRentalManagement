using MotorcycleRentalManagement.Domain.Entities;

namespace MotorcycleRentalManagement.Domain.Interfaces.Usercases
{
    public interface IMotorcycleUseCase
    {
        Task<IEnumerable<Motorcycle>> GetMotorcyclesAsync(string licensePlate);
    }
}
