using MassTransit;
using MotorcycleRentalManagement.Domain.Entities;
using MotorcycleRentalManagement.Domain.Interfaces.Repositories;
using MotorcycleRentalManagement.Infrastructure.Messaging.Models;

namespace MotorcycleRentalManagement.Infrastructure.Messaging.Consumer
{
    public class MotorcycleRegisteredConsumer : IConsumer<MotorcycleRegisteredEvent>
    {
        private readonly INotificationEventRepository _notificationRepository;

        public MotorcycleRegisteredConsumer(INotificationEventRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task Consume(ConsumeContext<MotorcycleRegisteredEvent> context)
        {
            var motorcycleEvent = context.Message;

            if (motorcycleEvent.Year == 2024)
            {
                // Armazena a notificação no banco de dados
                var notification = new Notification("Moto registrada", $"Uma moto com o ano 2024 foi registrada: {motorcycleEvent.MotorcycleId}");
                await _notificationRepository.SaveAsync(notification);
            }
        }
    }
}
