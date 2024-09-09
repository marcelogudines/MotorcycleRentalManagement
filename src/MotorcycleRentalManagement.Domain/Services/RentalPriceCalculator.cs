namespace MotorcycleRentalManagement.Domain.Services
{
    public class RentalPriceCalculator
    {
        public (decimal totalPrice, int rentalPlan) CalculatePrice(int days)
        {
            return days switch
            {
                <= 7 => (30m * days, 7),  // Até 7 dias - R$30,00 por dia
                <= 15 => (28m * days, 15), // Entre 8 e 15 dias - R$28,00 por dia
                <= 30 => (22m * days, 30), // Entre 16 e 30 dias - R$22,00 por dia
                <= 45 => (20m * days, 45), // Entre 31 e 45 dias - R$20,00 por dia
                <= 50 => (18m * days, 50), // Entre 46 e 50 dias - R$18,00 por dia
                _ => throw new ArgumentException("Número de dias de locação inválido") // Qualquer número maior que 50 é inválido
            };
        }
    }
}
