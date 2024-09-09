namespace MotorcycleRentalManagement.Infrastructure.Messaging.Models
{
    public class MotorcycleRegisteredEvent
    {
        public string MotorcycleId { get; }
        public string LicensePlate { get; }
        public int Year { get; }

        public MotorcycleRegisteredEvent(string motorcycleId, string licensePlate, int year)
        {
            MotorcycleId = motorcycleId;
            LicensePlate = licensePlate;
            Year = year;
        }
    }
}
