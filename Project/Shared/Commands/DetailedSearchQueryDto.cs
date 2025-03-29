using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.CustomValidations;
using Shared.Models;

namespace Shared.Commands
{
    public class DetailedSearchQueryDto
    {
        [RegularExpression("^(?:[A-Za-z]+|[ა-ჰ]+)$", ErrorMessage = "Should only be A-z or ა-ჰ")]
        public string? Firstname { get; set; }
        [RegularExpression("^(?:[A-Za-z]+|[ა-ჰ]+)$", ErrorMessage = "Should only be A-z or ა-ჰ")]
        public string? Lastname { get; set; }
        public Gender? Gender { get; set; }
        public string? PersonalId { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? ImagePath { get; set; }
        public string? City { get; set; }
        public string? PhoneNumber { get; set; }
        public int PageNumber { get; set; } = 1;  
        public int PageSize { get; set; } = 10;   
    }
}
