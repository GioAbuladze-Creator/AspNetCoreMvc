using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shared.Abstractions;
using Shared.Commands;
using Shared.Infrastructure;
using Shared.Models;

namespace DAL
{
    public class PhysicalPersonRepository : IPhysicalPersonRepository
    {
        private readonly PhysicalPersonsDbContext _dbContext;
        public PhysicalPersonRepository(PhysicalPersonsDbContext physicalPersonsDbContext)
        {
            _dbContext = physicalPersonsDbContext;
        }
        public async Task CreatePhysicalPersonAsync(PhysicalPerson person)
        {
            await _dbContext.PhysicalPersons.AddAsync(person);
        }
        public async Task<string?> DeletePhysicalPersonAsync(int id)
        {
            var person = await _dbContext.PhysicalPersons.FindAsync(id);

            if (person is null)
            {
                throw new KeyNotFoundException($"Person with Id {id} not found.");
            }
            var imagePathToDelete = person.ImagePath;
            _dbContext.Relation.RemoveRange(person.RelatedFrom);
            _dbContext.Relation.RemoveRange(person.RelatedTo);
            _dbContext.PhysicalPersons.Remove(person);
            return imagePathToDelete;
        }
        public void UpdatePhysicalPerson(PhysicalPerson physicalPerson)
        {
            _dbContext.PhysicalPersons.Update(physicalPerson);
        }
        public async Task<City?> GetCityByNameAsync(string city)
        {
            return await _dbContext.City
                .FirstOrDefaultAsync(c => c.Name.ToLower() == city.ToLower());
        }
    }
}
