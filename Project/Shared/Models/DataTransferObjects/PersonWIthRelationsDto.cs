using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.CustomValidations;

namespace Shared.Models.DataTransferObjects
{
    public class PersonWithRelationsDto
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public Gender Gender { get; set; }
        public string PersonalId { get; set; }
        public DateTime BirthDate { get; set; }
        public City? City { get; set; }
        public List<PhoneNumber>? PhoneNumbers { get; set; }
        public string? ImagePath { get; set; }
        public Dictionary<int, RelationType> RelatedFromPersonData { get; set; } = new();
        public Dictionary<int, RelationType> RelatedToPersonData { get; set; } = new();
    }
}
