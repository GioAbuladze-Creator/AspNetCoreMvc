using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.DataTransferObjects
{
    public class UpdatePhoneNumberDto
    {
        [Range(1, 3, ErrorMessage = "Number type must be either 1 for 'Home' or 2 for 'Work' and 3 for 'Mobile'.")]
        public PhoneNumberType Type { get; set; }
        [Length(4, 50, ErrorMessage = "The phone number must have at 4-50 digits"), RegularExpression("^[0-9]+$", ErrorMessage = "Number must be only digits")]
        public string? Number { get; set; }
    }
}
