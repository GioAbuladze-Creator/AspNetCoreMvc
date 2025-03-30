using System.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Abstractions;
using Shared.Infrastructure;


namespace DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private PhysicalPersonsDbContext _dbContext;
        public IPhysicalPersonRepository PhysicalPersonRepository { get; }
        public IRelationRepository RelationRepository { get; }
        public IPersonInfoRepository PersonInfoRepository { get; }
        public IProfilePictureRepository ProfilePictureRepository { get; }


        public UnitOfWork(PhysicalPersonsDbContext physicalPersonsDbContext, IPhysicalPersonRepository physicalPersonRepository, IRelationRepository relationRepository, IPersonInfoRepository personInfoRepository, IProfilePictureRepository profilePictureRepository)
        {
            _dbContext = physicalPersonsDbContext;
            PhysicalPersonRepository = physicalPersonRepository;
            RelationRepository = relationRepository;
            PersonInfoRepository = personInfoRepository;
            ProfilePictureRepository = profilePictureRepository;
        }
        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }
    }
}
