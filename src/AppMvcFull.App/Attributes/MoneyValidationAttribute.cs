using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace AppMvcFull.App.Attributes
{
    public class MoneyValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                Convert.ToDecimal(value, CultureInfo.CurrentCulture);
            }
            catch
            {
                return new ValidationResult("Moeda no formato inválido");
            }
            return ValidationResult.Success;
        }
    }
}
