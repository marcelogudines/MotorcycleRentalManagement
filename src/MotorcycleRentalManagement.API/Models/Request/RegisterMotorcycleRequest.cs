namespace MotorcycleRentalManagement.API.Models.Request
{
    public class RegisterMotorcycleRequest
    {
        public string Identificador { get; set; }
        public int Ano { get; set; }
        public string Modelo { get; set; }
        public string Placa { get; set; }
    }

}
