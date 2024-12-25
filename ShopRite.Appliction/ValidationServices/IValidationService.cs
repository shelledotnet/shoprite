using ShopRite.Application.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;
using FluentValidation;

namespace ShopRite.Application.ValidationServices
{
    public interface IValidationService
    {
        Task<FluentResponse> ValidateAsync<T>(T model , IValidator<T> validator);
    }


}
