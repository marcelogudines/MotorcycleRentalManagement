using MotorcycleRentalManagement.Domain.Entities;
using MotorcycleRentalManagement.Domain.Interfaces.Repositories;
using MotorcycleRentalManagement.Domain.Interfaces.Usercases;

namespace MotorcycleRentalManagement.Domain.Usercases.MotorcycleCase
{
    public class MotorcycleUseCase : IMotorcycleUseCase
    {
        private readonly IMotorcycleRepository _motorcycleRepository;

        public MotorcycleUseCase(IMotorcycleRepository motorcycleRepository)
        {
            _motorcycleRepository = motorcycleRepository;
        }

        public async Task<IEnumerable<Motorcycle>> GetMotorcyclesAsync(string licensePlate)
        {
            return await _motorcycleRepository.GetMotorcyclesAsync(licensePlate);
        }
    }
}
