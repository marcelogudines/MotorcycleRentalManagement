using MotorcycleRentalManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRentalManagement.Domain.Interfaces.Usercases
{
    public interface IUpdateDeliveryPersonCnhImageUseCase
    {
        Task<INotifiable> ExecuteAsync(string id, string cnhImage);
    }
}
