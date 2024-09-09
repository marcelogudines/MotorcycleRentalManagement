using MotorcycleRentalManagement.Domain.Entities;
using MotorcycleRentalManagement.Domain.Interfaces.Repositories;
using MotorcycleRentalManagement.Domain.Interfaces.Usercases;

namespace MotorcycleRentalManagement.Domain.Usercases.MotorcycleCase
{
    public class DeleteMotorcycleUseCase : IDeleteMotorcycleUseCase
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly INotifiable _notifiable;

        public DeleteMotorcycleUseCase(IMotorcycleRepository motorcycleRepository, INotifiable notifiable)
        {
            _motorcycleRepository = motorcycleRepository;
            _notifiable = notifiable;
        }

        public async Task<INotifiable> ExecuteAsync(string id)
        {
            var motorcycle = await _motorcycleRepository.GetByIdAsync(id);
            if (motorcycle == null)
            {
                _notifiable.AddNotification(nameof(id), "Moto não encontrada no sistema");
                return _notifiable;
            }

            await _motorcycleRepository.DeleteAsync(id);
            return _notifiable;
        }
    }
}