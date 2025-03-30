using System;
using System.Net.NetworkInformation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shared.Abstractions;
using Shared.Commands;
using Shared.Models;
using Shared.Models.DataTransferObjects;

namespace Business
{
    public class PersonsService : IPersonsService
    {
        private readonly IUnitOfWork _unitOfWork;


        public PersonsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> CreatePersonAsync(CreatePersonCommandDto personDto)
        {
            var person = new PhysicalPerson()
            {
                Firstname = personDto.Firstname,
                Lastname = personDto.Lastname,
                BirthDate = personDto.BirthDate,
                Gender = personDto.Gender,
                PersonalId = personDto.PersonalId,
                PhoneNumbers = personDto.PhoneNumbers?.Select(x => new PhoneNumber()
                {
                    Number = x.Number,
                    Type = x.Type
                }).ToList()
            };

            if (personDto.City is not null)
            {
                var existingCity = await _unitOfWork.PhysicalPersonRepository.GetCityByNameAsync(personDto.City.Name);
                if (existingCity is not null)
                {
                    person.City = existingCity;
                }
                else
                {
                    person.City = new City()
                    {
                        Name = personDto.City.Name
                    };
                };
            }
            await _unitOfWork.PhysicalPersonRepository.CreatePhysicalPersonAsync(person);
            await _unitOfWork.Save();
            return person.Id;
        }
        public async Task<string?> DeletePersonAsync(int id)
        {
            var imagePathToDelete = await _unitOfWork.PhysicalPersonRepository.DeletePhysicalPersonAsync(id);
            await _unitOfWork.Save();
            return imagePathToDelete;
        }
        public async Task UpdatePersonAsync(UpdatePersonCommandDto personDto)
        {
            if (personDto == null)
            {
                throw new ArgumentNullException(nameof(personDto), "Update DTO cannot be null.");
            }

            var person = await _unitOfWork.PersonInfoRepository.GetByIdAsync(personDto.Id);
            if (!string.IsNullOrWhiteSpace(personDto.Firstname))
            {
                person.Firstname = personDto.Firstname;
            }

            if (!string.IsNullOrWhiteSpace(personDto.Lastname))
            {
                person.Lastname = personDto.Lastname;
            }

            if (personDto.Gender.HasValue)
            {
                person.Gender = personDto.Gender.Value;
            }

            if (!string.IsNullOrWhiteSpace(personDto.PersonalId))
            {
                person.PersonalId = personDto.PersonalId;
            }
            if (!string.IsNullOrWhiteSpace(personDto.ImagePath))
            {
                person.ImagePath = personDto.ImagePath;
            }

            if (personDto.BirthDate.HasValue)
            {
                person.BirthDate = personDto.BirthDate.Value;
            }

            if (personDto.City != null && !string.IsNullOrWhiteSpace(personDto.City))
            {
                var existingCity = await _unitOfWork.PhysicalPersonRepository.GetCityByNameAsync(personDto.City);
                if (existingCity != null)
                {
                    person.City = existingCity;
                }
                else
                {
                    person.City = new City { Name = personDto.City };
                }
            }

            if (personDto.PhoneNumbers != null && personDto.PhoneNumbers.Any())
            {
                if (person.PhoneNumbers is not null)
                {
                    person.PhoneNumbers.Clear();
                }
                person.PhoneNumbers = personDto.PhoneNumbers.Where(x => x.Number != null).Select(x => new PhoneNumber
                {
                    Number = x.Number,
                    Type = x.Type
                }).ToList();
            }

            _unitOfWork.PhysicalPersonRepository.UpdatePhysicalPerson(person);
            await _unitOfWork.Save();
        }
        public async Task CreateRelationshipAsync(int fromId, int toId, RelationType relationType)
        {
            if (fromId == toId)
            {
                throw new ArgumentException("A person cannot have a relationship with themselves.");
            }
            await _unitOfWork.RelationRepository.AddRelationAsync(fromId, toId, relationType);
            await _unitOfWork.Save();
        }
        public async Task RemoveRelationshipAsync(int fromId, int toId)
        {
            await _unitOfWork.RelationRepository.RemoveRelationAsync(fromId, toId);
            await _unitOfWork.Save();
        }

        public async Task<PersonWithRelationsDto> GetFullInfoByIdAsync(int id)
        {
            return await _unitOfWork.PersonInfoRepository.GetFullInfoByIdAsync(id);
        }
        public async Task<List<PersonWithRelationsDto>> QuickSearchAsync(string query, int pageNumber, int pageSize)
        {
            return await _unitOfWork.PersonInfoRepository.QuickSearchAsync(query, pageNumber, pageSize);
        }
        public async Task<List<PersonWithRelationsDto>> DetailedSearchAsync(DetailedSearchQueryDto searchObj)
        {
            return await _unitOfWork.PersonInfoRepository.DetailedSearchAsync(searchObj);
        }
        public async Task<List<RelatedInfoDto>> GetRelatedInfoAsync()
        {
            return await _unitOfWork.PersonInfoRepository.GetRelatedInfoAsync();
        }
        public async Task<List<PersonWithRelationsDto>> GetAllPersonsAsync()
        {
            return await _unitOfWork.PersonInfoRepository.GetAllPersonsAsync();
        }
        public async Task<string?> SetProfilePictureAsync(int id, string imagePath)
        {
            var pathToDelete = await _unitOfWork.ProfilePictureRepository.SetProfilePictureAsync(id, imagePath);
            await _unitOfWork.Save();
            return pathToDelete;
        }

        public async Task<string> GetProfilePictureAsync(int id)
        {
            return await _unitOfWork.ProfilePictureRepository.GetProfilePictureAsync(id);
        }

        public async Task<string> RemoveProfilePictureAsync(int id)
        {
            var pathToDelete = await _unitOfWork.ProfilePictureRepository.RemoveProfilePictureAsync(id);
            await _unitOfWork.Save();
            return pathToDelete;
        }

        public async Task<UpdatePersonCommandDto> GetPersonForUpdateAsync(int id)
        {
            return await _unitOfWork.PersonInfoRepository.GetPersonForUpdateAsync(id);
        }
    }
}
