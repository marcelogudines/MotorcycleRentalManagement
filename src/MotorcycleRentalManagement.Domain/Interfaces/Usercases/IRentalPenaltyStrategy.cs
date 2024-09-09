using MotorcycleRentalManagement.Domain.Entities;
using MotorcycleRentalManagement.Domain.Services;

namespace MotorcycleRentalManagement.Domain.Interfaces.Usercases
{
    public interface IRentalPenaltyStrategy
    {
        PenaltyType PenaltyType { get; }

        /// <summary>
        /// Calcula a penalidade com base no aluguel e na data de devolução.
        /// </summary>
        /// <param name="rental">A entidade de locação.</param>
        /// <param name="returnDate">A data de devolução informada.</param>
        /// <returns>O valor da penalidade a ser aplicada.</returns>
        decimal CalculatePenalty(Rental rental, DateTime returnDate);
    }
}
