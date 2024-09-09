using MotorcycleRentalManagement.Domain.Entities;

namespace MotorcycleRentalManagement.Domain.Interfaces.Usercases
{
    public interface IRegisterDeliveryPersonUseCase
    {
        Task<INotifiable> ExecuteAsync(string id, string name, string cnpj, DateTime birthDate, string cnhNumber, CnhType cnhType, string cnhImage);
    }
}
