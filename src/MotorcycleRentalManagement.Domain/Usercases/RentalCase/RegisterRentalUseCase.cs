using MotorcycleRentalManagement.Domain.Entities;
using MotorcycleRentalManagement.Domain.Interfaces.Repositories;
using MotorcycleRentalManagement.Domain.Interfaces.Usercases;
using MotorcycleRentalManagement.Domain.Services;

namespace MotorcycleRentalManagement.Domain.Usercases.RentalCase
{
    public class RegisterRentalUseCase : IRegisterRentalUseCase
    {
        private readonly IDeliveryPersonRepository _deliveryPersonRepository;
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly IRentalRepository _rentalRepository;
        private readonly INotifiable _notifiable;
        private readonly RentalPriceCalculator _priceCalculator;

        public RegisterRentalUseCase(
            IDeliveryPersonRepository deliveryPersonRepository,
            IMotorcycleRepository motorcycleRepository,
            IRentalRepository rentalRepository,
            INotifiable notifiable,
            RentalPriceCalculator priceCalculator)
        {
            _deliveryPersonRepository = deliveryPersonRepository;
            _motorcycleRepository = motorcycleRepository;
            _rentalRepository = rentalRepository;
            _notifiable = notifiable;
            _priceCalculator = priceCalculator;
        }

        public async Task<(INotifiable Notifiable, Guid? RentalId)> ExecuteAsync(
            string deliveryPersonId, string motorcycleId, DateTime endDate, DateTime expectedEndDate)
        {
            // Verifica se o entregador existe
            var deliveryPerson = await _deliveryPersonRepository.GetByIdAsync(deliveryPersonId);
            if (deliveryPerson == null)
            {
                _notifiable.AddNotification(nameof(deliveryPersonId), "Entregador não encontrado");
                return (_notifiable, null);
            }

            // Verifica se o entregador está habilitado na categoria A
            if (deliveryPerson.CnhType != CnhType.A)
            {
                _notifiable.AddNotification(nameof(deliveryPerson.CnhType), "Somente entregadores habilitados na categoria A podem efetuar locações");
                return (_notifiable, null);
            }

            // Verifica se a moto existe
            var motorcycle = await _motorcycleRepository.GetByIdAsync(motorcycleId);
            if (motorcycle == null)
            {
                _notifiable.AddNotification(nameof(motorcycleId), "Moto não encontrada");
                return (_notifiable, null);
            }

            // Calcula o número de dias da locação
            var startDate = DateTime.UtcNow.AddDays(1); // A data de início é sempre o primeiro dia após a criação
            int rentalDays = (endDate - startDate).Days;

            // Validação: número de dias não pode ser menor que 0
            if (rentalDays < 0)
            {
                _notifiable.AddNotification(nameof(endDate), "A data de término não pode ser anterior à data de início");
                return (_notifiable, null);
            }

            // Validação: número de dias não pode exceder 50 dias
            if (rentalDays > 50)
            {
                _notifiable.AddNotification(nameof(rentalDays), "A locação não pode exceder 50 dias");
                return (_notifiable, null);
            }

            // Calcula o preço total da locação com base no número de dias
            var (totalPrice, rentalPlan) = _priceCalculator.CalculatePrice(rentalDays);

            // Cria a locação
            var rental = new Rental(Guid.NewGuid(), motorcycleId, deliveryPersonId, endDate, expectedEndDate, totalPrice, rentalPlan);

            // Valida se a locação está em um estado válido
            if (!rental.IsValid)
            {
                return (rental, null);
            }

            // Salva a locação no repositório
            await _rentalRepository.AddAsync(rental);

            // Retorna as notificações e o ID da locação
            return (_notifiable, rental.Id);
        }
    }
}
