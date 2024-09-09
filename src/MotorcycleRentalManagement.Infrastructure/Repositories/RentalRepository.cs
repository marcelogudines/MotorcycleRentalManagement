using MongoDB.Driver;
using MotorcycleRentalManagement.Domain.Entities;
using MotorcycleRentalManagement.Domain.Interfaces.Repositories;

namespace MotorcycleRentalManagement.Infrastructure.Repositories
{
    public class RentalRepository : IRentalRepository
    {
        private readonly IMongoCollection<Rental> _rentalCollection;

        public RentalRepository(IMongoDatabase database)
        {
            _rentalCollection = database.GetCollection<Rental>("Rentals");
        }

        public async Task<Rental> GetByIdAsync(Guid rentalId)
        {
            return await _rentalCollection.Find(r => r.Id == rentalId).FirstOrDefaultAsync();
        }

        public async Task AddAsync(Rental rental)
        {
            await _rentalCollection.InsertOneAsync(rental);
        }

        public async Task UpdateAsync(Rental rental)
        {
            await _rentalCollection.ReplaceOneAsync(r => r.Id == rental.Id, rental);
        }

        public Task<IEnumerable<Rental>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
