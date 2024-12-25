using ShopRite.Application.Dto;
using FluentValidation;

namespace ShopRite.Application.ValidationServices
{
    public class ValidationService : IValidationService
    {
        public async Task<FluentResponse> ValidateAsync<T>(T model, IValidator<T> validator)
        {
            FluentResponse fluentResponse = new();
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid) 
            {
              var errors =  validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                string errorToString = string.Join(";", errors);
                fluentResponse.Message = errorToString;
                fluentResponse.Success = false;
            }
            fluentResponse.Message = "success";
            fluentResponse.Success = false;
            return fluentResponse;
        }
    }


}
