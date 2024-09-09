using MotorcycleRentalManagement.Domain.Entities;
using MotorcycleRentalManagement.Domain.Interfaces;
using MotorcycleRentalManagement.Domain.Interfaces.Repositories;

namespace MotorcycleRentalManagement.Domain.Usercases.RentalCase
{
    public class ReturnRentalUseCase : IReturnRentalUseCase
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly INotifiable _notifiable;
        private readonly IPenaltyStrategyContext _penaltyStrategyContext;

        public ReturnRentalUseCase(
            IRentalRepository rentalRepository,
            INotifiable notifiable,
            IPenaltyStrategyContext penaltyStrategyContext)
        {
            _rentalRepository = rentalRepository;
            _notifiable = notifiable;
            _penaltyStrategyContext = penaltyStrategyContext;
        }

        public async Task<INotifiable> ExecuteAsync(Guid rentalId, DateTime returnDate)
        {
            var rental = await _rentalRepository.GetByIdAsync(rentalId);
            if (rental == null)
            {
                _notifiable.AddNotification(nameof(rentalId), "Locação não encontrada.");
                return _notifiable;
            }

            // Verifica se a data de devolução é inferior à data de início da locação
            if (returnDate < rental.StartDate)
            {
                _notifiable.AddNotification(nameof(returnDate), "A data de devolução não pode ser inferior à data de início da locação.");
                return _notifiable;
            }

            // Calcula a penalidade com base na data de devolução
            decimal penalty = _penaltyStrategyContext.ApplyPenalty(rental, returnDate);

            // Atualiza o valor total da locação com a penalidade aplicada
            rental.ApplyPenalty(penalty);

            //Insere a data de Devolução
            rental.ApplyReturnDate(returnDate);

            // Atualiza o repositório com as informações da locação devolvida
            await _rentalRepository.UpdateAsync(rental);

            return _notifiable;
        }
    }
}
