using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GASPG.Helpers.CustomValidators
{
    public class Capitalized : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult(validationContext.DisplayName + " is required.");
            }

            return char.IsUpper(value.ToString().First()) ?
                ValidationResult.Success
                : new ValidationResult(validationContext.DisplayName + " must start with capital letter.");
        }
    }
}
