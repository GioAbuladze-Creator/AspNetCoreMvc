using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Abstractions;
using Shared.Infrastructure;
using Shared.Models;

namespace DAL
{
    public class RelationRepository : IRelationRepository
    {
        private readonly PhysicalPersonsDbContext _dbContext;
        public RelationRepository(PhysicalPersonsDbContext physicalPersonsDbContext)
        {
            _dbContext = physicalPersonsDbContext;
        }
        public async Task AddRelationAsync(int fromId, int toId, RelationType relationType)
        {
            var personFrom = await _dbContext.PhysicalPersons.FindAsync(fromId);
            if (personFrom is null)
            {
                throw new KeyNotFoundException($"Person with Id {fromId} not found.");
            }
            if (personFrom.RelatedTo is null)
            {
                personFrom.RelatedTo = new List<Relation>();
            }

            personFrom.RelatedTo.Add(new Relation()
            {
                FromId = fromId,
                ToId = toId,
                RelationType = relationType
            });
        }
        public async Task RemoveRelationAsync(int fromId, int toId)
        {
            var personFrom = await _dbContext.PhysicalPersons
                .FindAsync(fromId);

            if (personFrom is null)
            {
                throw new KeyNotFoundException($"Person with Id {fromId} not found.");
            }

            var relation = personFrom.RelatedTo
                .SingleOrDefault(r => r.ToId == toId);

            if (relation == null)
            {
                throw new KeyNotFoundException($"Relationship with Id {fromId} to Id {toId} not found.");
            }

            _dbContext.Relation.Remove(relation);
        }
    }
}
