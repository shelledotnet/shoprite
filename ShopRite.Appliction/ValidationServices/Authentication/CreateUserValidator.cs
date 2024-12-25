using FluentValidation;
using ShopRite.Application.Dto.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopRite.Application.ValidationServices.Authentication
{
    public class CreateUserValidator  : AbstractValidator<CreateUser>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.FullName)
                   .NotEmpty().WithMessage("{0} is required");
            RuleFor(x => x.Email)
                  .NotEmpty().WithMessage("{0} is required")
                  .EmailAddress().WithMessage("invalid {0} format ");
            RuleFor(x => x.Password)
                 .NotEmpty().WithMessage("{0} is required")
                 .MinimumLength(8).WithMessage( "{0} must be at least 8 characters tong .")
                 .Matches(@"[A-Z]").WithMessage(" {0} must contain at least one uppercase letter.")
                 .Matches(@"[a-z]").WithMessage("{0} must contain at least one lowercase letter.")
                 .Matches(@"[\d]").WithMessage("{0} must contain at least one number.")
                .Matches(@"[^\w]").WithMessage("{0}must contain at least one special character. ") ;

            RuleFor(x => x.ConfirmPassword)
                  .Equal(x => x.Password).WithMessage("Passwords do not match." );

        }
    }

}
