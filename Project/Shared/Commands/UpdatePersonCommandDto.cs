using System.ComponentModel.DataAnnotations;
using Shared.CustomValidations;
using Shared.Models;
using Shared.Models.DataTransferObjects;

namespace Shared.Commands
{
    public class UpdatePersonCommandDto
    {
        [Required]
        public int Id { get; set; }
        [Length(2, 50), RegularExpression("^(?:[A-Za-z]+|[ა-ჰ]+)$", ErrorMessage = "Should only be A-z or ა-ჰ")]
        public string? Firstname { get; set; }
        [Length(2, 50), RegularExpression("^(?:[A-Za-z]+|[ა-ჰ]+)$", ErrorMessage = "Should only be A-z or ა-ჰ")]
        public string? Lastname { get; set; }
        public Gender? Gender { get; set; }
        [Length(11, 11), RegularExpression("^[0-9]+$", ErrorMessage = "Personal Id must be 11-digit number")]
        public string? PersonalId { get; set; }
        [ValidateDate]
        public DateTime? BirthDate { get; set; }
        public string? ImagePath { get; set; }
        public string? City { get; set; }
        public List<UpdatePhoneNumberDto>? PhoneNumbers { get; set; }
    }
}
