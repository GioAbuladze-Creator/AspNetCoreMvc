using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Shared.Commands;
using Shared.Models;
namespace Shared.Abstractions
{
    public interface IPhysicalPersonRepository
    {
        Task CreatePhysicalPersonAsync(PhysicalPerson physicalPerson);
        void UpdatePhysicalPerson(PhysicalPerson physicalPerson);
        Task<string?> DeletePhysicalPersonAsync(int id);
        Task<City?> GetCityByNameAsync(string city);
      


    }
}
