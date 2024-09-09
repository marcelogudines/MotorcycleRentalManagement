using MotorcycleRentalManagement.Domain.Entities;

namespace MotorcycleRentalManagement.Domain.Interfaces.Usercases
{
    public interface IUpdateMotorcycleLicensePlateUseCase
    {
        Task<INotifiable> ExecuteAsync(string id, string newLicensePlate);
    }
}
