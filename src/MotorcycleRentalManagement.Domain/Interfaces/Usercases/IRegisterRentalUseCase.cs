using MotorcycleRentalManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRentalManagement.Domain.Interfaces.Usercases
{
    public interface IRegisterRentalUseCase
    {
        Task<(INotifiable Notifiable, Guid? RentalId)> ExecuteAsync(
             string deliveryPersonId, string motorcycleId, DateTime endDate, DateTime expectedEndDate);
    }
}
