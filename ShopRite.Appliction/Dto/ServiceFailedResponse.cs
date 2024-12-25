using System.Net;

namespace ShopRite.Application.Dto
{
    public record ServiceFailedResponse(HttpStatusCode Code, bool Success = false, string? message = null);


}
