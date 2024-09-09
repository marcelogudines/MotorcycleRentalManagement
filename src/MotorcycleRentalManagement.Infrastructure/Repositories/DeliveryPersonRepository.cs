using MongoDB.Driver;
using MotorcycleRentalManagement.Domain.Entities;
using MotorcycleRentalManagement.Domain.Interfaces.Repositories;

namespace MotorcycleRentalManagement.Infrastructure.Repositories
{
    public class DeliveryPersonRepository : IDeliveryPersonRepository
    {
        private readonly IMongoCollection<DeliveryPerson> _deliveryPersonCollection;

        public DeliveryPersonRepository(IMongoDatabase database)
        {
            _deliveryPersonCollection = database.GetCollection<DeliveryPerson>("DeliveryPersons");
        }

        public async Task AddAsync(DeliveryPerson deliveryPerson)
        {
            await _deliveryPersonCollection.InsertOneAsync(deliveryPerson);
        }

        public async Task UpdateAsync(DeliveryPerson deliveryPerson)
        {
            await _deliveryPersonCollection.ReplaceOneAsync(dp=> dp.Id == deliveryPerson.Id, deliveryPerson);
        }

        public async Task<DeliveryPerson> GetByIdAsync(string id)
        {
            return await _deliveryPersonCollection.Find(dp => dp.Id == id).FirstOrDefaultAsync();
        }

        public async Task<DeliveryPerson> GetByCnpjOrCnhNumberAsync(string cnpj, string cnhNumber)
        {
            return await _deliveryPersonCollection
                .Find(dp => dp.Cnpj == cnpj || dp.CnhNumber == cnhNumber)
                .FirstOrDefaultAsync();
        }
    }
}
