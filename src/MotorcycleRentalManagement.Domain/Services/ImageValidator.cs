namespace MotorcycleRentalManagement.Domain.Services
{
    public class ImageValidator
    {
        private const int MaxImageSizeInBytes = 1 * 1024 * 1024; // 1MB
        private readonly byte[] _pngHeader = new byte[] { 0x89, 0x50, 0x4E, 0x47 }; // Header para PNG
        private readonly byte[] _bmpHeader = new byte[] { 0x42, 0x4D }; // Header para BMP

        public bool IsValidImageFormat(string base64Image)
        {
            try
            {
                // Decodifica a string base64 para um array de bytes
                var imageBytes = Convert.FromBase64String(base64Image);

                // Verifica se o tamanho da imagem excede 1MB
                if (imageBytes.Length > MaxImageSizeInBytes)
                {
                    return false;
                }

                // Verifica se o array de bytes é grande o suficiente para verificar o header
                if (imageBytes.Length < 4)
                {
                    return false;
                }

                // Verifica o cabeçalho PNG (4 bytes) ou BMP (2 bytes)
                return StartsWith(imageBytes, _pngHeader) || StartsWith(imageBytes, _bmpHeader);
            }
            catch (Exception)
            {
                // Caso ocorra uma exceção (como uma string base64 inválida), retorna false
                return false;
            }
        }

        private bool StartsWith(byte[] imageBytes, byte[] header)
        {
            // Verifica se os primeiros bytes da imagem coincidem com o header fornecido
            for (int i = 0; i < header.Length; i++)
            {
                if (imageBytes[i] != header[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
