using MotorcycleRentalManagement.Domain.Entities;
using MotorcycleRentalManagement.Domain.Interfaces.Repositories;
using MotorcycleRentalManagement.Domain.Interfaces.Usercases;
using MotorcycleRentalManagement.Domain.Services;

namespace MotorcycleRentalManagement.Domain.Usercases.DeliveryPersonCase
{
    public class RegisterDeliveryPersonUseCase : IRegisterDeliveryPersonUseCase
    {
        private readonly IDeliveryPersonRepository _deliveryPersonRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly INotifiable _notifiable;
        private readonly ImageValidator _imageValidator;

        public RegisterDeliveryPersonUseCase(
            IDeliveryPersonRepository deliveryPersonRepository,
            IFileStorageService fileStorageService,
            ImageValidator imageValidator, INotifiable notifiable)
        {
            _deliveryPersonRepository = deliveryPersonRepository;
            _fileStorageService = fileStorageService;
            _imageValidator = imageValidator;
            _notifiable = notifiable;
        }

        public async Task<INotifiable> ExecuteAsync(string id, string name, string cnpj, DateTime birthDate, string cnhNumber, CnhType cnhType, string cnhImage)
        {
            //Valida Identificador
            var existingPersonId = await _deliveryPersonRepository.GetByIdAsync(id);

            if (existingPersonId != null)
            {
                _notifiable.AddNotification(nameof(id), "O identificador já existe no sistema");
                return _notifiable;
            }

            // Valida se o CNPJ e o CNH são únicos
            var existingPerson = await _deliveryPersonRepository.GetByCnpjOrCnhNumberAsync(cnpj, cnhNumber);
            if (existingPerson != null)
            {

                _notifiable.AddNotification(nameof(cnpj), "CNPJ or CNH já existem no sistema");
                return _notifiable;
            }

            // Validação da imagem (formato e tamanho)
            if (!_imageValidator.IsValidImageFormat(cnhImage))
            {
                _notifiable.AddNotification(nameof(cnhImage), "Imagem inválida, somente PNG ou BMP. Certifique-se que o tamanho não ultrapasse 1MB");
                return _notifiable;
            }

            //Salva a imagem no disco local e obtém o caminho
            var imagePath = await _fileStorageService.SaveFileAsync(cnhImage);

            // Cria o entregador e associa o caminho da imagem
            var deliveryPerson = new DeliveryPerson(id, name, cnpj, birthDate, cnhNumber, cnhType, imagePath);

            if (!deliveryPerson.IsValid)
            {
                return deliveryPerson;
            }

            // Salva o entregador no repositório
            await _deliveryPersonRepository.AddAsync(deliveryPerson);

            return deliveryPerson;
        }
    }
}
