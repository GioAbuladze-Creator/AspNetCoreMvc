using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shared.Abstractions;
using Shared.Infrastructure;

namespace DAL
{
    public class ProfilePictureRepository : IProfilePictureRepository
    {
        private readonly PhysicalPersonsDbContext _dbContext;
        public ProfilePictureRepository(PhysicalPersonsDbContext physicalPersonsDbContext)
        {
            _dbContext = physicalPersonsDbContext;
        }
        public async Task<string?> SetProfilePictureAsync(int id, string imagePath)
        {
            var person = await _dbContext.PhysicalPersons.FindAsync(id);
            if (person is null)
            {
                throw new KeyNotFoundException($"Person with Id {id} not found.");
            }
            var pathToDelete = person.ImagePath;
            person.ImagePath = imagePath;
            return pathToDelete;
        }
        public async Task<string> GetProfilePictureAsync(int id)
        {
            var person = await _dbContext.PhysicalPersons.FindAsync(id);
            if (person is null || person.ImagePath is null)
            {
                throw new KeyNotFoundException($"Person profile picture with Id {id} not found.");
            }
            return person.ImagePath;
        }
        public async Task<string> RemoveProfilePictureAsync(int id)
        {
            var person = await _dbContext.PhysicalPersons.FindAsync(id);
            if (person is null || person.ImagePath is null)
            {
                throw new KeyNotFoundException($"Person profile picture with Id {id} not found.");
            }
            var pathToDelete = person.ImagePath;
            person.ImagePath = null;
            return pathToDelete;
        }
    }
}

