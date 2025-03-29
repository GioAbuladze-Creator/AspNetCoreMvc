using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Commands;
using Shared.Models;
using Shared.Models.DataTransferObjects;

namespace Shared.Abstractions
{
    public interface IPersonInfoRepository
    {
        Task<PersonWithRelationsDto> GetFullInfoByIdAsync(int personId);
        Task<PhysicalPerson> GetByIdAsync(int id);
        Task<List<PersonWithRelationsDto>> QuickSearchAsync(string query, int pageNumber, int pageSize);
        Task<List<PersonWithRelationsDto>> DetailedSearchAsync(DetailedSearchQueryDto searchObj);
        Task<List<RelatedInfoDto>> GetRelatedInfoAsync();
        Task<List<PersonWithRelationsDto>> GetAllPersonsAsync();
        Task<UpdatePersonCommandDto> GetPersonForUpdateAsync(int id);
    }
}
