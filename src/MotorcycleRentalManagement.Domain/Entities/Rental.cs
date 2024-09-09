using System.Diagnostics;

namespace MotorcycleRentalManagement.Domain.Entities
{
    public class Rental : Notifiable
    {
        public Guid Id { get; private set; }
        public string MotorcycleId { get; private set; }
        public string DeliveryPersonId { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public DateTime ExpectedEndDate { get; private set; }
        public decimal Price { get; private set; }
        public int RentalPlan { get; private set; }
        public decimal PricePerDay { get; private set; }
        public DateTime? ReturnDate { get; private set; }

        public Rental(Guid id, string motorcycleId, string deliveryPersonId, DateTime endDate, DateTime expectedEndDate, decimal price, int rentalPlan)
        {
            Id = id;
            MotorcycleId = motorcycleId;
            DeliveryPersonId = deliveryPersonId;
            StartDate = DateTime.UtcNow.AddDays(1); // O início da locação é o primeiro dia após a data de criação
            EndDate = endDate;
            ExpectedEndDate = expectedEndDate;
            Price = price;
            RentalPlan = rentalPlan;
            PricePerDay = price / rentalPlan; // Calcula o preço por dia baseado no plano

            Validate();
        }

        private void Validate()
        {
            if (string.IsNullOrEmpty(MotorcycleId))
                AddNotification(nameof(MotorcycleId), "O identificador da moto é obrigatório");

            if (string.IsNullOrEmpty(DeliveryPersonId))
                AddNotification(nameof(DeliveryPersonId), "O identificador do entregador é obrigatório");

            if (StartDate < DateTime.UtcNow.Date)
                AddNotification(nameof(StartDate), "A data de início não pode ser anterior à data de hoje");

            if (EndDate <= StartDate)
                AddNotification(nameof(EndDate), "A data de término deve ser posterior à data de início");

            if (ExpectedEndDate < StartDate)
                AddNotification(nameof(ExpectedEndDate), "A data de previsão de término ser posterior à data de início");

            if (Price <= 0)
                AddNotification(nameof(Price), "Ocorreu uma falha no cálculo do preço, deve ser maior que zero");
        }

        // Método para aplicar penalidade ao valor total da locação
        public void ApplyPenalty(decimal penalty)
        {
            Price += penalty;
            PricePerDay = Price / RentalPlan;
        }

        // Método para adicionar a data de devolução
        public void ApplyReturnDate(DateTime returnDate)
        {
            ReturnDate = returnDate;
        }
    }
}
