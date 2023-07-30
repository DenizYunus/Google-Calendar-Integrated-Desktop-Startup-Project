using MongoDB.Bson;
using OctoBackend.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace OctoBackend.API.Attributes
{
    public class ValidateTodoCategoryAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not string id)
            {
                return new ValidationResult("Invalid parameter type.");
            }

            if (!TodoCategoryConsts.categories.ContainsKey(id))
            {
                return new ValidationResult("Invalid category type.");               
            }

            return ValidationResult.Success!;
        }
    }
}
