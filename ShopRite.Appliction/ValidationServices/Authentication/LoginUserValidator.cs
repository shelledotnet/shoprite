using FluentValidation;
using ShopRite.Application.Dto.Identity;

namespace ShopRite.Application.ValidationServices.Authentication
{
    public class LoginUserValidator : AbstractValidator<LoginUser>
    {
        public LoginUserValidator()
        {
            RuleFor(x => x.Email)
                 .NotEmpty().WithMessage("{0} is required")
                 .EmailAddress().WithMessage("invalid {0} format  ");


            RuleFor(x => x.Password)
           .NotEmpty().WithMessage("{0} is required");


        }
    }

}
