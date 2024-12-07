using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ShopRite.Application.Dto
{
    public record ServiceResponse<T>(T? Data,HttpStatusCode Code , bool Success = false , string? message = null);
    public record ServiceFailedResponse(HttpStatusCode Code, bool Success = false, string? message = null);

}
