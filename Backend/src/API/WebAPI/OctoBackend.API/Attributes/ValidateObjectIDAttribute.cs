using MongoDB.Bson;
using OctoBackend.Application.Abstractions.Repositories;
using System.ComponentModel.DataAnnotations;

namespace OctoBackend.API.Attributes
{
    public class ValidateObjectIDCollectionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not ICollection<string> collection)
            {
                return new ValidationResult("Invalid parameter type.");
            }

            foreach (var id in collection)
            {
                if (!ObjectId.TryParse(id, out _))
                {
                    return new ValidationResult("Invalid Id format.");
                }
            }

            return ValidationResult.Success!;
        }
    }

    public class ValidateObjectIDAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not string id)
            {
                return new ValidationResult("Invalid parameter type.");
            }

            if (!ObjectId.TryParse(id, out _))
            {
                return new ValidationResult("Invalid Id format.");
            }

            return ValidationResult.Success!;
        }
    }
}
