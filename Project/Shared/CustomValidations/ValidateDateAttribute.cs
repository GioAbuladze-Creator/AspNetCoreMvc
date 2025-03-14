using System.ComponentModel.DataAnnotations;

namespace Shared.CustomValidations
{
    public class ValidateDateAttribute : ValidationAttribute
    {
        public ValidateDateAttribute() : base(() => "Your age must be 18 or more.") { }
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }

            if (value is DateTime birthDate)
            {
                DateTime minBirthDate = DateTime.UtcNow.AddYears(-18);
                return birthDate <= minBirthDate;
            }

            return false;
        }
    }
}
