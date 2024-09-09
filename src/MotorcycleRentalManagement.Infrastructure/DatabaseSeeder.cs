using MongoDB.Driver;
using MotorcycleRentalManagement.Domain.Entities;

namespace MotorcycleRentalManagement.Infrastructure
{
    public class DatabaseSeeder
    {
        private readonly IMongoDatabase _database;

        public DatabaseSeeder(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task SeedAsync()
        {
            await SeedMotorcyclesAsync();
            await SeedDeliveryPersonsAsync();
        }

        private async Task SeedMotorcyclesAsync()
        {
            var motorcyclesCollection = _database.GetCollection<Motorcycle>("Motorcycles");

            // Verificar se já existem registros
            if (await motorcyclesCollection.CountDocumentsAsync(_ => true) == 0)
            {
                var motorcycles = new List<Motorcycle>
                {
                    new Motorcycle(Guid.NewGuid().ToString(), 2022, "Yamaha MT-03", "ABC-1234"),
                    new Motorcycle(Guid.NewGuid().ToString(), 2022, "Honda CB500", "XYZ-5678"),
                    new Motorcycle(Guid.NewGuid().ToString(), 2023, "Kawasaki Ninja 400", "DEF-3456")
                };

                await motorcyclesCollection.InsertManyAsync(motorcycles);
            }
        }

        private async Task SeedDeliveryPersonsAsync()
        {
            var deliveryPersonsCollection = _database.GetCollection<DeliveryPerson>("DeliveryPersons");

            // Verificar se já existem registros
            if (await deliveryPersonsCollection.CountDocumentsAsync(_ => true) == 0)
            {
                var deliveryPersons = new List<DeliveryPerson>
                {
                    new DeliveryPerson(Guid.NewGuid().ToString(), "John Doe", "12345678901234", new DateTime(1990, 1, 1), "CNH123456", CnhType.A, ""),
                    new DeliveryPerson(Guid.NewGuid().ToString(), "Jane Smith", "98765432109876", new DateTime(1985, 5, 15), "CNH987654", CnhType.B, "")
                };

                await deliveryPersonsCollection.InsertManyAsync(deliveryPersons);
            }
        }
    }
}