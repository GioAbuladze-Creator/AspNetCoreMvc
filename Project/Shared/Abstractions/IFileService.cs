using Microsoft.AspNetCore.Http;

namespace Shared.Abstractions
{
    public interface IFileService
    {
        void DeleteFile(string fileNameWithExtension);
        Task<byte[]> GetFileAsync(string imageName);
        Task<string> SaveFileAsync(IFormFile imageFile);
    }
}