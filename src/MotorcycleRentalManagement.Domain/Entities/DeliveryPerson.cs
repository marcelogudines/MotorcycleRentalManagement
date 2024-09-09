using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace MotorcycleRentalManagement.Domain.Entities
{
    public class DeliveryPerson : Notifiable
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Cnpj { get; private set; }
        public DateTime BirthDate { get; private set; }
        public string CnhNumber { get; private set; }
        public CnhType CnhType { get; private set; }
        public string CnhImagePath { get; private set; }

        public DeliveryPerson(string id, string name, string cnpj, DateTime birthDate, string cnhNumber, CnhType cnhType, string cnhImagePath)
        {
            Id = id;
            Name = name;
            Cnpj = cnpj;
            BirthDate = birthDate;
            CnhNumber = cnhNumber;
            CnhType = cnhType;
            CnhImagePath = cnhImagePath;

            Validate();
        }

        private void Validate()
        {
            if (string.IsNullOrEmpty(Name))
                AddNotification(nameof(Name), "Nome é obrigatório");

            if (string.IsNullOrEmpty(Cnpj))
                AddNotification(nameof(Cnpj), "CNPJ é obrigatório");

            if (string.IsNullOrEmpty(CnhNumber))
                AddNotification(nameof(CnhNumber), "CNH é obrigatório");

            if (!Enum.IsDefined(typeof(CnhType), CnhType))
                AddNotification(nameof(CnhType), "TIpo de CNH Inválido");
        }

        public bool CanRentMotorcycle()
        {
            if (CnhType != CnhType.A && CnhType != CnhType.AB)
            {
                AddNotification(nameof(CnhType), "O entregador deve ter CNH tipo A ou AB para alugar uma moto");
                return false;
            }
            return true;
        }

        public void UpdateCnhImage(string imagePath)
        {
            CnhImagePath = imagePath;
        }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CnhType
    {
        [EnumMember(Value = "A")]
        A,

        [EnumMember(Value = "B")]
        B,

        [EnumMember(Value = "AB")]
        AB
    }
}
