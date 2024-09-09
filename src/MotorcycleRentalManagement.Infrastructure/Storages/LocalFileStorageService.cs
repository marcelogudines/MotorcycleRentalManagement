using Microsoft.Extensions.Configuration;
using MotorcycleRentalManagement.Domain.Interfaces.Repositories;

namespace MotorcycleRentalManagement.Infrastructure.Storages
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly string _rootPath;

        public LocalFileStorageService(IConfiguration configuration)
        {
            // Verifica se o caminho configurado existe, senão usa o diretório da aplicação
            var configuredPath = configuration["Storage:DeliveryPersonImagesPath"];

            if (string.IsNullOrEmpty(configuredPath) || !Directory.Exists(configuredPath))
            {
                // Se o caminho não estiver configurado ou não existir, define o diretório da aplicação como padrão
                _rootPath = Path.Combine(Directory.GetCurrentDirectory(), "DeliveryPersonImages");

                // Verifica se o diretório da aplicação existe, se não, cria
                if (!Directory.Exists(_rootPath))
                {
                    Directory.CreateDirectory(_rootPath);
                }
            }
            else
            {
                _rootPath = configuredPath;
            }
        }

        public async Task<string> SaveFileAsync(string base64)
        {
            // Verifica se a pasta específica dentro do root path existe, senão, cria
            var folderPath = Path.Combine(_rootPath, "CNHImages");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Decodifica a string base64 para um array de bytes
            byte[] imageBytes;
            try
            {
                imageBytes = Convert.FromBase64String(base64);
            }
            catch (FormatException)
            {
                throw new ArgumentException("A string Base64 fornecida é inválida");
            }

           
            var fileExtension = ".png"; 
            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(folderPath, fileName);

            // Salva o arquivo no disco local
            await File.WriteAllBytesAsync(filePath, imageBytes);

            return fileName; 
        }

        public async Task DeleteFileAsync(string filePath)
        {
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }

        public string GetFilePath(string fileName, string folder)
        {
            return Path.Combine(_rootPath, folder, fileName);
        }
    }
}
