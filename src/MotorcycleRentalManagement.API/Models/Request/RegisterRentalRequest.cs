using MotorcycleRentalManagement.Domain.Entities;

namespace MotorcycleRentalManagement.API.Models.Requests
{
    public class RegisterRentalRequest
    {
        public string EntregadorId { get; set; }
        public string MotoId { get; set; }
        public DateTime DataTermino { get; set; }
        public DateTime DataPrevisaoTermino { get; set; }
    }
}