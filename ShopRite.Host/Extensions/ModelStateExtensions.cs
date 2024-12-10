using Microsoft.AspNetCore.Mvc.ModelBinding;
using ShopRite.Application.Dto;
using System.Net;

namespace ShopRite.Host.Extensions
{
    public static class ModelStateExtensions
    {
        public static List<string> GetErrorMessages(this ModelStateDictionary dictionary)
        {
            return dictionary.SelectMany(m => m.Value.Errors)
                             .Select(m => m.ErrorMessage)
                             .ToList();
        }

        public static ServiceFailedResponse GetApiResponse(this ModelStateDictionary dictionary)
        {
            var errorMessages = dictionary.GetErrorMessages();

            return new ServiceFailedResponse(HttpStatusCode.BadRequest,false, string.Join("; ", errorMessages));


        }
    }
}
