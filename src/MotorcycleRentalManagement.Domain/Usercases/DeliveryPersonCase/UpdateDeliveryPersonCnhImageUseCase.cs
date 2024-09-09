using MotorcycleRentalManagement.Domain.Entities;
using MotorcycleRentalManagement.Domain.Interfaces.Repositories;
using MotorcycleRentalManagement.Domain.Interfaces.Usercases;
using MotorcycleRentalManagement.Domain.Services;

namespace MotorcycleRentalManagement.Domain.Usercases.DeliveryPersonCase
{
    public class UpdateDeliveryPersonCnhImageUseCase : IUpdateDeliveryPersonCnhImageUseCase
    {
        private readonly IDeliveryPersonRepository _deliveryPersonRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly INotifiable _notifiable;
        private readonly ImageValidator _imageValidator;

        public UpdateDeliveryPersonCnhImageUseCase(
            IDeliveryPersonRepository deliveryPersonRepository,
            IFileStorageService fileStorageService,
            ImageValidator imageValidator,
            INotifiable notifiable)
        {
            _deliveryPersonRepository = deliveryPersonRepository;
            _fileStorageService = fileStorageService;
            _imageValidator = imageValidator;
            _notifiable = notifiable;
        }

        public async Task<INotifiable> ExecuteAsync(string id, string cnhImage)
        {
            // Valida se o entregador existe no sistema
            var deliveryPerson = await _deliveryPersonRepository.GetByIdAsync(id);
            if (deliveryPerson == null)
            {
                _notifiable.AddNotification(nameof(id), "Entregador não encontrado");
                return _notifiable;
            }

           
            // Validação da imagem (formato e tamanho)
            if (!_imageValidator.IsValidImageFormat(cnhImage))
            {
                _notifiable.AddNotification(nameof(cnhImage), "Imagem inválida, somente PNG ou BMP. Certifique-se que o tamanho não ultrapasse 1MB");
                return _notifiable;
            }

            // Salva a imagem no disco local e obtém o caminho
            var imagePath = await _fileStorageService.SaveFileAsync(cnhImage);

            // Atualiza o caminho da imagem no cadastro do entregador
            deliveryPerson.UpdateCnhImage(imagePath);

            // Salva a atualização no repositório
            await _deliveryPersonRepository.UpdateAsync(deliveryPerson);

            return deliveryPerson;
        }
    }
}
