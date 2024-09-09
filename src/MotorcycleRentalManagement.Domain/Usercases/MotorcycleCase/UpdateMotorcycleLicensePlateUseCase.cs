using MotorcycleRentalManagement.Domain.Entities;
using MotorcycleRentalManagement.Domain.Interfaces.Repositories;
using MotorcycleRentalManagement.Domain.Interfaces.Usercases;

namespace MotorcycleRentalManagement.Domain.Usercases.MotorcycleCase
{
    public class UpdateMotorcycleLicensePlateUseCase : IUpdateMotorcycleLicensePlateUseCase
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly INotifiable _notifiable;

        public UpdateMotorcycleLicensePlateUseCase(IMotorcycleRepository motorcycleRepository, INotifiable notifiable)
        {
            _motorcycleRepository = motorcycleRepository;
            _notifiable = notifiable;
        }

        public async Task<INotifiable> ExecuteAsync(string id, string newLicensePlate)
        {
            var motorcycle = await _motorcycleRepository.GetByIdAsync(id);
            if (motorcycle == null)
            {
                _notifiable.AddNotification(nameof(id), "Moto não encontrada no sistema");
                return _notifiable;
            }

            var existingMotorcycleWithPlate = await _motorcycleRepository.GetMotorcyclesAsync(newLicensePlate);
            if (existingMotorcycleWithPlate.Any())
            {
                _notifiable.AddNotification(nameof(newLicensePlate), "A nova placa já existe no sistema");
                return _notifiable;
            }

            motorcycle.UpdateLicensePlate(newLicensePlate);

            if (!motorcycle.IsValid)
            {
                return motorcycle;
            }
            await _motorcycleRepository.UpdateAsync(motorcycle);

            return _notifiable;
        }
    }
}