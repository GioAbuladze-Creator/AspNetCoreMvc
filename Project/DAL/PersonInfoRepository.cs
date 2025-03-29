using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shared.Abstractions;
using Shared.Commands;
using Shared.Infrastructure;
using Shared.Models;
using Shared.Models.DataTransferObjects;

namespace DAL
{
    public class PersonInfoRepository : IPersonInfoRepository
    {
        private readonly PhysicalPersonsDbContext _dbContext;
        public PersonInfoRepository(PhysicalPersonsDbContext physicalPersonsDbContext)
        {
            _dbContext = physicalPersonsDbContext;
        }
        public async Task<PersonWithRelationsDto> GetFullInfoByIdAsync(int Id)
        {
            var person = await _dbContext.PhysicalPersons.FindAsync(Id);
            if (person is null)
            {
                throw new KeyNotFoundException($"Person with Id {Id} not found.");
            }
            var relatedFromData = person.RelatedFrom.ToDictionary(r => r.FromId, r => r.RelationType);
            var relatedToData = person.RelatedTo.ToDictionary(r => r.ToId, r => r.RelationType);

            return new PersonWithRelationsDto
            {
                Id = person.Id,
                Firstname = person.Firstname,
                Lastname = person.Lastname,
                Gender = person.Gender,
                PersonalId = person.PersonalId,
                BirthDate = person.BirthDate,
                City = person.City,
                PhoneNumbers = person.PhoneNumbers,
                ImagePath = person.ImagePath,
                RelatedFromPersonData = relatedFromData,
                RelatedToPersonData = relatedToData,
            };
        }
        public async Task<PhysicalPerson> GetByIdAsync(int id)
        {
            var person = await _dbContext.PhysicalPersons.FindAsync(id);
            if (person is null)
            {
                throw new KeyNotFoundException($"Person with Id {id} not found.");
            }
            return person;
        }
        public async Task<List<PersonWithRelationsDto>> QuickSearchAsync(string query, int pageNumber, int pageSize)
        {
            var skip = (pageNumber - 1) * pageSize;
            var persons = await _dbContext.PhysicalPersons
                .Where(p => p.Firstname.Contains(query) ||
                            p.Lastname.Contains(query) ||
                            p.PersonalId.Contains(query))
                .Skip(skip)
                .Take(pageSize)
                .Select(p => p.Id)
                .ToListAsync();
            var personDetails = new List<PersonWithRelationsDto>();
            foreach (var id in persons)
            {
                var fullInfo = await GetFullInfoByIdAsync(id);
                personDetails.Add(fullInfo);
            }
            return personDetails;
        }
        public async Task<List<PersonWithRelationsDto>> DetailedSearchAsync(DetailedSearchQueryDto searchObj)
        {
            var query = _dbContext.PhysicalPersons.AsQueryable();

            if (!string.IsNullOrEmpty(searchObj.Firstname))
            {
                query = query.Where(p => p.Firstname.Contains(searchObj.Firstname));
            }
            if (!string.IsNullOrEmpty(searchObj.Lastname))
            {
                query = query.Where(p => p.Lastname.Contains(searchObj.Lastname));
            }
            if (searchObj.Gender.HasValue)
            {
                query = query.Where(p => p.Gender == searchObj.Gender.Value);
            }
            if (!string.IsNullOrEmpty(searchObj.PersonalId))
            {
                query = query.Where(p => p.PersonalId.Contains(searchObj.PersonalId));
            }
            if (searchObj.BirthDate.HasValue)
            {
                query = query.Where(p => p.BirthDate == searchObj.BirthDate.Value);
            }
            if (!string.IsNullOrEmpty(searchObj.City))
            {
                query = query.Where(p => p.City != null && p.City.Name.Contains(searchObj.City));
            }

            if (!string.IsNullOrEmpty(searchObj.PhoneNumber))
            {
                query = query.Where(p => p.PhoneNumbers != null && p.PhoneNumbers
                    .Any(ph => ph.Number.Contains(searchObj.PhoneNumber)));
            }
            var pagedResult = await query
                .Skip((searchObj.PageNumber - 1) * searchObj.PageSize)
                .Take(searchObj.PageSize)
                .Select(p => p.Id)
                .ToListAsync();
            var personDetails = new List<PersonWithRelationsDto>();
            foreach (var id in pagedResult)
            {
                var fullInfo = await GetFullInfoByIdAsync(id);
                personDetails.Add(fullInfo);
            }
            return personDetails;
        }
        public async Task<List<RelatedInfoDto>> GetRelatedInfoAsync()
        {
            var physicalPersons = await _dbContext.PhysicalPersons.ToListAsync();
            var result = physicalPersons.Select(p => new RelatedInfoDto
            {
                Name = p.Firstname,
                RelatedInfo = p.RelatedTo
                    .GroupBy(r => r.RelationType.ToString())
                    .ToDictionary(g => g.Key, g => g.Count().ToString())
            }).ToList();

            return result;
        }
        public async Task<List<PersonWithRelationsDto>> GetAllPersonsAsync()
        {
            var persons = await _dbContext.PhysicalPersons.ToListAsync();

            return persons.Select(person => new PersonWithRelationsDto
            {
                Id = person.Id,
                Firstname = person.Firstname,
                Lastname = person.Lastname,
                Gender = person.Gender,
                PersonalId = person.PersonalId,
                BirthDate = person.BirthDate,
                City = person.City,
                PhoneNumbers = person.PhoneNumbers,
                ImagePath = person.ImagePath,
                RelatedFromPersonData = person.RelatedFrom.ToDictionary(r => r.FromId, r => r.RelationType),
                RelatedToPersonData = person.RelatedTo.ToDictionary(r => r.ToId, r => r.RelationType)
            }).ToList();
        }

        public async Task<UpdatePersonCommandDto> GetPersonForUpdateAsync(int id)
        {
            var person = await _dbContext.PhysicalPersons.FindAsync(id);
            if (person is null)
            {
                throw new KeyNotFoundException($"Person with Id {id} not found.");
            }
            return new UpdatePersonCommandDto()
            {
                Id = person.Id,
                Firstname = person.Firstname,
                Lastname = person.Lastname,
                Gender = person.Gender,
                PersonalId = person.PersonalId,
                BirthDate = person.BirthDate,
                ImagePath = person.ImagePath,
                City = person.City?.Name,
                PhoneNumbers = person.PhoneNumbers?
                    .Select(pn => new UpdatePhoneNumberDto
                    {
                        Type = pn.Type,
                        Number = pn.Number
                    }).ToList() ?? new List<UpdatePhoneNumberDto>()
            };
        }
    }
}
