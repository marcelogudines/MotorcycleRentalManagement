using MongoDB.Driver;
using MotorcycleRentalManagement.Domain.Entities;
using MotorcycleRentalManagement.Domain.Interfaces.Repositories;

namespace MotorcycleRentalManagement.Infrastructure.Repositories
{
    public class MotorcycleRepository : IMotorcycleRepository
    {
        private readonly IMongoCollection<Motorcycle> _motorcycleCollection;

        public MotorcycleRepository(IMongoDatabase database)
        {
            _motorcycleCollection = database.GetCollection<Motorcycle>("Motorcycles");
        }

        public async Task<Motorcycle> GetByIdAsync(string id)
        {
            return await _motorcycleCollection.Find(m => m.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Motorcycle>> GetMotorcyclesAsync(string licensePlate)
        {
            var filterDefinition = Builders<Motorcycle>.Filter.Empty;

            if (!string.IsNullOrWhiteSpace(licensePlate))
            {
                filterDefinition &= Builders<Motorcycle>.Filter.Eq(m => m.LicensePlate, licensePlate);
            }

            return await _motorcycleCollection.Find(filterDefinition).ToListAsync();
        }

        public async Task AddAsync(Motorcycle motorcycle)
        {
            await _motorcycleCollection.InsertOneAsync(motorcycle);
        }

        public async Task UpdateAsync(Motorcycle motorcycle)
        {
            await _motorcycleCollection.ReplaceOneAsync(m => m.Id == motorcycle.Id, motorcycle);
        }

        public async Task DeleteAsync(string id)
        {
            await _motorcycleCollection.DeleteOneAsync(m => m.Id == id);
        }
    }
}
