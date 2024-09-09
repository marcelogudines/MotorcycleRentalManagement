using MotorcycleRentalManagement.Domain.Entities;
using MotorcycleRentalManagement.Domain.Interfaces.Usercases;
using MotorcycleRentalManagement.Domain.Services;

public class EarlyReturnPenaltyStrategy : IRentalPenaltyStrategy
{
    public PenaltyType PenaltyType => PenaltyType.EarlyReturn;

    public decimal CalculatePenalty(Rental rental, DateTime returnDate)
    {
        int remainingDays = (rental.ExpectedEndDate - returnDate).Days;

        return rental.RentalPlan switch
        {
            7 => remainingDays * (rental.PricePerDay * 0.2m), // Multa de 20% para plano de 7 dias
            15 => remainingDays * (rental.PricePerDay * 0.4m), // Multa de 40% para plano de 15 dias
            _ => 0m // Sem penalidade para outros planos
        };
    }
}