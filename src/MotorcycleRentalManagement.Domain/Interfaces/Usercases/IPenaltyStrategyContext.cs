using MotorcycleRentalManagement.Domain.Entities;

namespace MotorcycleRentalManagement.Domain.Interfaces
{
    public interface IPenaltyStrategyContext
    {
        /// <summary>
        /// Aplica a penalidade com base na estratégia correta.
        /// </summary>
        /// <param name="rental">A entidade de locação.</param>
        /// <param name="returnDate">A data de devolução informada.</param>
        /// <returns>O valor da penalidade a ser aplicado.</returns>
        decimal ApplyPenalty(Rental rental, DateTime returnDate);
    }
}                                                           