using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Abstractions
{
    public interface IProfilePictureRepository
    {
        Task<string?> SetProfilePictureAsync(int id, string imagePath);
        Task<string> GetProfilePictureAsync(int id);
        Task<string> RemoveProfilePictureAsync(int id);
    }
}
