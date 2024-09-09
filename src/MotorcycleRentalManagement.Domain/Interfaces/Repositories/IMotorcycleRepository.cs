using MotorcycleRentalManagement.Domain.Entities;

namespace MotorcycleRentalManagement.Domain.Interfaces.Repositories
{
    public interface IMotorcycleRepository
    {
        Task<Motorcycle> GetByIdAsync(string id);
        Task AddAsync(Motorcycle motorcycle);
        Task UpdateAsync(Motorcycle motorcycle);
        Task DeleteAsync(string id);
        Task<IEnumerable<Motorcycle>> GetMotorcyclesAsync(string licensePlate);
    }
}
