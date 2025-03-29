using System.Data;

namespace Shared.Abstractions
{
    public interface IUnitOfWork : IDisposable
    {
        IPhysicalPersonRepository PhysicalPersonRepository { get; }
        IRelationRepository RelationRepository { get; }
        IPersonInfoRepository PersonInfoRepository { get; }
        IProfilePictureRepository ProfilePictureRepository { get; }
        Task Save();
    }
}