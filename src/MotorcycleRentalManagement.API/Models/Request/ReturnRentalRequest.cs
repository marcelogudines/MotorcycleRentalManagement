using MotorcycleRentalManagement.Domain.Entities;

namespace MotorcycleRentalManagement.API.Models.Requests
{
    public class ReturnRentalRequest
    {
        public DateTime DataDevolucao { get; set; }
    }
}