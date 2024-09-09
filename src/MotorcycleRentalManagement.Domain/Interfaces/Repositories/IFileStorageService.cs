namespace MotorcycleRentalManagement.Domain.Interfaces.Repositories
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(string file);
        Task DeleteFileAsync(string filePath);
        string GetFilePath(string fileName, string folder);
    }
}
