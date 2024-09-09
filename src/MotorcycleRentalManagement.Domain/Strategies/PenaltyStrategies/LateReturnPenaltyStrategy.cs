using MotorcycleRentalManagement.Domain.Entities;
using MotorcycleRentalManagement.Domain.Interfaces.Usercases;
using MotorcycleRentalManagement.Domain.Services;

namespace MotorcycleRentalManagement.Domain.Strategies.PenaltyStrategies
{
    public class LateReturnPenaltyStrategy : IRentalPenaltyStrategy
    {
        public PenaltyType PenaltyType => PenaltyType.LateReturn;

        public decimal CalculatePenalty(Rental rental, DateTime returnDate)
        {
            int extraDays = (returnDate - rental.ExpectedEndDate).Days;
            return extraDays * 50m; // Multa de R$50,00 por dia extra
        }
    }
}