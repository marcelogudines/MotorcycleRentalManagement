using MotorcycleRentalManagement.Domain.Entities;

namespace MotorcycleRentalManagement.API.Models.Requests
{
    public class RegisterDeliveryPersonRequest
    {
        public required string Identificador { get; set; }
        public required string Nome { get; set; }
        public required string Cnpj { get; set; }
        public DateTime DataNascimento { get; set; }
        public required string NumeroCnh { get; set; }
        public CnhType TipoCnh { get; set; }
        public string? ImagemCnh { get; set; } // Imagem da CNH codificada em Base64
    }
}