using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportBook.Helpers
{
    public class DateValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // your validation logic
            if (value != null && (DateTime)value >= DateTime.Now && (DateTime)value <= DateTime.Now.AddYears(3))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Valid date can be chosen between now and 3 years from now");
            }
        }
    }
}
