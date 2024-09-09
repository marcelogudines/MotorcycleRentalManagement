using MotorcycleRentalManagement.Domain.Entities;

namespace MotorcycleRentalManagement.Domain.Interfaces.Repositories
{
    public interface IDeliveryPersonRepository
    {
        Task AddAsync(DeliveryPerson deliveryPerson);
        Task<DeliveryPerson> GetByIdAsync(string id);
        Task<DeliveryPerson> GetByCnpjOrCnhNumberAsync(string cnpj, string cnhNumber);
        Task UpdateAsync(DeliveryPerson deliveryPerson);
    }
}
