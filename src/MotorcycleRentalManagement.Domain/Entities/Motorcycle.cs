namespace MotorcycleRentalManagement.Domain.Entities
{
    public class Motorcycle : Notifiable
    {
        public string Id { get; private set; }
        public int Year { get; private set; }
        public string Model { get; private set; }
        public string LicensePlate { get; private set; }

        public Motorcycle(string id, int year, string model, string licensePlate)
        {
            Id = id;
            Year = year;
            Model = model;
            LicensePlate = licensePlate;

            Validate();
        }

        private void Validate()
        {
            if (string.IsNullOrEmpty(Id))
                AddNotification(nameof(Id), "Identificador é obrigatório");

            if (string.IsNullOrEmpty(Model))
                AddNotification(nameof(Model), "Modelo é obrigatório");

            ValidateLicensePlate(LicensePlate);

            if (Year <= 0)
                AddNotification(nameof(Year), "Ano Inválido");
        }

        public void UpdateLicensePlate(string newLicensePlate)
        {
            ValidateLicensePlate(newLicensePlate);

            if (IsValid)
            {
                LicensePlate = newLicensePlate;
            }
        }

        private void ValidateLicensePlate(string licensePlate)
        {
            if (string.IsNullOrEmpty(licensePlate))
            {
                AddNotification(nameof(LicensePlate), "Placa é obrigatória");
            }

            if (!IsValidLicensePlateFormat(licensePlate))
            {
                AddNotification(nameof(LicensePlate), "Placa com formato inválido");
            }
        }

        private bool IsValidLicensePlateFormat(string licensePlate)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(licensePlate, "^[A-Z]{3}-[0-9]{4}$");
        }
    }
}
