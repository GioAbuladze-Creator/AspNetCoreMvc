using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Shared.Abstractions;

namespace Business;

public class FileService(IWebHostEnvironment environment) : IFileService
{

    public async Task<string> SaveFileAsync(IFormFile imageFile)
    {
        var contentPath = environment.ContentRootPath;
        var path = Path.Combine(contentPath, "wwwroot");

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        var ext = Path.GetExtension(imageFile.FileName);

        var fileName = $"{Guid.NewGuid().ToString()}{ext}";
        var fileNameWithPath = Path.Combine(path, fileName);
        await using var stream = new FileStream(fileNameWithPath, FileMode.Create);
        await imageFile.CopyToAsync(stream);
        return fileName;
    }


    public void DeleteFile(string fileNameWithExtension)
    {
        if (string.IsNullOrEmpty(fileNameWithExtension))
        {
            throw new ArgumentNullException(nameof(fileNameWithExtension));
        }
        var contentPath = environment.ContentRootPath;
        var path = Path.Combine(contentPath, $"wwwroot", fileNameWithExtension);

        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Invalid file path");
        }
        File.Delete(path);
    }

    public async Task<byte[]> GetFileAsync(string imageName)
    {
        var contentPath = environment.ContentRootPath;
        var path = Path.Combine(contentPath, "wwwroot", imageName);
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"File not found: {imageName}");
        }
        return await File.ReadAllBytesAsync(path);
    }
}