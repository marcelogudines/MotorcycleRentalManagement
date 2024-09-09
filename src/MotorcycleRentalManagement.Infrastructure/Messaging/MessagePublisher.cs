using MassTransit;
using MotorcycleRentalManagement.Domain.Interfaces.Repositories;
using MotorcycleRentalManagement.Infrastructure.Messaging.Models;

namespace MotorcycleRentalManagement.Infrastructure.Messaging
{
    public class MessagePublisher : IMessagePublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public MessagePublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublishMotorcycleRegistered(string motorcycleId, string licensePlate, int year)
        {
            var motorcycleRegisteredEvent = new MotorcycleRegisteredEvent(motorcycleId, licensePlate, year);
            await _publishEndpoint.Publish(motorcycleRegisteredEvent);
        }
    }
}

