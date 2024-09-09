using MotorcycleRentalManagement.Domain.Entities;

namespace MotorcycleRentalManagement.Domain.Interfaces.Repositories
{
    public interface IRentalRepository
    {
        Task<Rental> GetByIdAsync(Guid id);
        Task<IEnumerable<Rental>> GetAllAsync();
        Task AddAsync(Rental rental);
        Task UpdateAsync(Rental rental);
        Task DeleteAsync(Guid id);
    }
}
