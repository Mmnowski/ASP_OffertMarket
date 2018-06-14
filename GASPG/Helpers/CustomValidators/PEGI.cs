using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GASPG.Helpers.CustomValidators
{
    public class PEGI : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult(validationContext.DisplayName + " is required.");
            }

            var postalCode = value.ToString();
            if (Regex.IsMatch(postalCode, "^[0-9]{1-2}[+]$"))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Invalid PEGI rating (use XX+).");
        }
    }
}
