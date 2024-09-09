using MotorcycleRentalManagement.Domain.Entities;
using MotorcycleRentalManagement.Domain.Interfaces.Repositories;
using MotorcycleRentalManagement.Domain.Interfaces.Usercases;

namespace MotorcycleRentalManagement.Domain.Usercases.MotorcycleCase
{
    public class RegisterMotorcycleMotorcycleUseCase : IRegisterMotorcycleMotorcycleUseCase
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly INotifiable _notifiable;
        private readonly IMessagePublisher _messagePublisher;

        public RegisterMotorcycleMotorcycleUseCase(IMotorcycleRepository motorcycleRepository, INotifiable notifiable, IMessagePublisher messagePublisher)
        {
            _motorcycleRepository = motorcycleRepository;
            _notifiable = notifiable;
            _messagePublisher = messagePublisher;
        }

        public async Task<INotifiable> RegisterMotorcycleAsync(string id, int year, string model, string licensePlate)
        {
            var existingMotorcycle = await _motorcycleRepository.GetByIdAsync(id);
            if (existingMotorcycle != null)
            {
                _notifiable.AddNotification(nameof(id), "O identificador já existe no sistema");
                return _notifiable;
            }

            var existingMotorcycleLicencePlate = await _motorcycleRepository.GetMotorcyclesAsync(licensePlate);
            if (existingMotorcycleLicencePlate.Any())
            {
                _notifiable.AddNotification(nameof(licensePlate), "A placa já existe no sistema");
                return _notifiable;
            }

            var motorcycle = new Motorcycle(id, year, model, licensePlate);

            if (!motorcycle.IsValid)
            {
                return motorcycle;
            }

            await _motorcycleRepository.AddAsync(motorcycle);

            await _messagePublisher.PublishMotorcycleRegistered(motorcycle.Id, motorcycle.LicensePlate, motorcycle.Year);

            return _notifiable;
        }
    }
}
