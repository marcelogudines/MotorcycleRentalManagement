using MotorcycleRentalManagement.Domain.Entities;
using MotorcycleRentalManagement.Domain.Interfaces.Repositories;
using MotorcycleRentalManagement.Domain.Interfaces.Usercases;

namespace MotorcycleRentalManagement.Domain.Usercases.RentalCase
{
    public class GetRentalByIdUseCase : IGetRentalByIdUseCase
    {
        private readonly IRentalRepository _rentalRepository;

        public GetRentalByIdUseCase(IRentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }

        public async Task<Rental> ExecuteAsync(Guid rentalId)
        {
            return await _rentalRepository.GetByIdAsync(rentalId);
        }
    }
}