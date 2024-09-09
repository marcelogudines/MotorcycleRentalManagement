using MotorcycleRentalManagement.Domain.Entities;
using MotorcycleRentalManagement.Domain.Interfaces;
using MotorcycleRentalManagement.Domain.Interfaces.Usercases;

namespace MotorcycleRentalManagement.Domain.Services
{
    public class PenaltyStrategyContext : IPenaltyStrategyContext
    {
        private readonly Dictionary<PenaltyType, IRentalPenaltyStrategy> _penaltyStrategies;

        public PenaltyStrategyContext(IEnumerable<IRentalPenaltyStrategy> penaltyStrategies)
        {
            // Mapeia as estratégias com base no tipo de penalidade
            _penaltyStrategies = penaltyStrategies.ToDictionary(strategy => strategy.PenaltyType);
        }

        public decimal ApplyPenalty(Rental rental, DateTime returnDate)
        {
            // Seleciona a estratégia correta com base na data de devolução
            var penaltyType = GetPenaltyType(rental, returnDate);

            if (_penaltyStrategies.TryGetValue(penaltyType, out var penaltyStrategy))
            {
                return penaltyStrategy.CalculatePenalty(rental, returnDate);
            }

            return 0m; // Sem penalidade se devolvido na data esperada
        }

        private PenaltyType GetPenaltyType(Rental rental, DateTime returnDate)
        {
            if (returnDate < rental.ExpectedEndDate)
            {
                return PenaltyType.EarlyReturn;
            }
            else if (returnDate > rental.ExpectedEndDate)
            {
                return PenaltyType.LateReturn;
            }

            throw new InvalidOperationException("Devolução na data esperada não gera penalidade.");
        }
    }

    public enum PenaltyType
    {
        EarlyReturn,
        LateReturn
    }
}