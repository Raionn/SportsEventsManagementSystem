using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportBook.Helpers
{
    public class BirthdayValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if ((DateTime)value > DateTime.Now.AddYears(-100) && (DateTime)value < DateTime.Now.AddYears(-5))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("You cannot be older than 100 and younger than 5");
            }
        }
    }
}
