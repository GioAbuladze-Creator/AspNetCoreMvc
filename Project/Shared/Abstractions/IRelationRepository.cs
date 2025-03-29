using Shared.Models;

namespace Shared.Abstractions
{
    public interface IRelationRepository
    {
        Task AddRelationAsync(int fromId, int toId, RelationType relationType);
        Task RemoveRelationAsync(int fromId, int toId);
    }
}