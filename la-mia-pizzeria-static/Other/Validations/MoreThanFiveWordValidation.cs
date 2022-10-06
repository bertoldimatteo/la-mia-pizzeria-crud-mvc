using System.ComponentModel.DataAnnotations;

namespace la_mia_pizzeria_crud_mvc.Other.Validations
{
    public class MoreThanFiveWordValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string fieldValue = (string)value;

            if (fieldValue.Length < 5 )
            {
                return new ValidationResult("Il campo deve avere almeno 5 parole");
            }
            return ValidationResult.Success;
        }
    }
}
