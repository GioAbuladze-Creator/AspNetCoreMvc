using Shared.Commands;
using Shared.Models;
using Shared.Models.DataTransferObjects;

namespace Shared.Abstractions
{
    public interface IPersonsService
    {
        public Task<int> CreatePersonAsync(CreatePersonCommandDto personDto);
        public Task<string?> DeletePersonAsync(int id);
        public Task UpdatePersonAsync(UpdatePersonCommandDto physicalPerson);
        public Task CreateRelationshipAsync(int fromId, int toId, RelationType relationType);
        public Task RemoveRelationshipAsync(int fromId, int toId);
        public Task<PersonWithRelationsDto> GetFullInfoByIdAsync(int personId);
        public Task<List<PersonWithRelationsDto>> QuickSearchAsync(string query, int pageNumber, int pageSize);
        public Task<List<PersonWithRelationsDto>> DetailedSearchAsync(DetailedSearchQueryDto searchObj);
        public Task<List<RelatedInfoDto>> GetRelatedInfoAsync();
        public Task<List<PersonWithRelationsDto>> GetAllPersonsAsync();
        public Task<UpdatePersonCommandDto> GetPersonForUpdateAsync(int id);
        public Task<string?> SetProfilePictureAsync(int id, string imagePath);
        public Task<string> GetProfilePictureAsync(int id);
        public Task<string> RemoveProfilePictureAsync(int id);
    }
}
